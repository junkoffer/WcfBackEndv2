using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using WcfBackEndv2.MailKit;
using WcfBackEndv2.Model;

namespace WcfBackEndv2
{
    public class Service1 : IService1
    {
        public enum ApiErrors
        {
            ServiceCaseWasNull,
            ServiceCasePostWasNull,
            NoMatchingServiceCaseFound,
            InputValidationFailed
        }

        public ServiceCase CreateCase(ServiceCase serviceCase)
        {
            // Gör en nullcheck
            if (serviceCase == null)
            {
                serviceCase = new ServiceCase();
                serviceCase.Errors.Add(ApiErrors.ServiceCaseWasNull.ToString());
                return serviceCase;
            }

            using (var context = new ApplicationDbContext())
            {
                // Validera all indata och avbryt ifall det är något fel. 
                // Alla dom där frågeteckena är för att hantera eventuella nullvärden 
                // så att programmet inte krashar
                // Läs mer på:
                // https://msdn.microsoft.com/en-us/magazine/dn802602.aspx
                // https://davefancher.com/2014/08/14/c-6-0-null-propagation-operator/
                var validated = (serviceCase.Name?.ValidateMinMaxLength(2, 30) ?? false)
                && serviceCase.FlatNr.ValidateMinMax(1000, 9999)
                && (serviceCase.ContactEmail?.ValidateMinMaxLength(6, 40) ?? false)
                && (serviceCase.ContactEmail?.ValidateRegex(InputValidator.RegexEmail) ?? false);
                if (!validated)
                {
                    serviceCase.Errors.Add(ApiErrors.InputValidationFailed.ToString());
                    return serviceCase;
                }
                // Ta reda på högsta befintliga CaseNr och lägg till 1
                var previousCase = context
                    .ServiceCases
                    .OrderByDescending(sc => sc.CaseNr)
                    .FirstOrDefault() ?? new ServiceCase();
                serviceCase.Date = DateTime.Now;
                serviceCase.CaseNr = previousCase.CaseNr + 1;

                // lägg till serviceCase till context
                context.Entry(serviceCase).State = EntityState.Added;
                // och skicka ändringarna till databasen
                try
                {
                    context.SaveChanges();
                }
                catch (Exception ex)
                             when (ex is DbUpdateException
                             || ex is DbUpdateConcurrencyException
                             || ex is DbEntityValidationException
                             || ex is NotSupportedException
                             || ex is ObjectDisposedException
                             || ex is InvalidOperationException
                             )
                // Dessa är alla exceptions som  context.SaveChanges() kan kasta. Info hämtad ifrån:
                // https://docs.microsoft.com/en-us/dotnet/api/system.data.entity.dbcontext.savechanges?view=entity-framework-6.2.0
                {
                    serviceCase.Errors.Add($"Exception in CreateCase(): {ex.Message}");
                }
            }
            return serviceCase;
        }


        public ServiceCasePost AddPost(int caseNr, ServiceCasePost serviceCasePost)
        {
            // Gör en nullcheck
            if (serviceCasePost == null)
            {
                serviceCasePost = new ServiceCasePost();
                serviceCasePost.Errors.Add(ApiErrors.ServiceCaseWasNull.ToString());
                return serviceCasePost;
            }
            if (serviceCasePost.Errors == null)
            {
                serviceCasePost.Errors = new List<string>();
            }
            using (var context = new ApplicationDbContext())
            {
                // Kolla att det finns ett serviceärende med matchande nummer 
                var serviceCase = context.ServiceCases
                    .Where(sc => sc.CaseNr == caseNr)
                    .Include(sc => sc.Posts)
                    .FirstOrDefault();
                if (serviceCase == null)
                {
                    serviceCasePost.Errors.Add(ApiErrors.NoMatchingServiceCaseFound.ToString());
                    return serviceCasePost;
                }

                // Validera all indata och avbryt ifall det är något fel. 
                // Alla dom där frågeteckena är för att hantera eventuella nullvärden 
                // så att programmet inte krashar
                // Läs mer på:
                // https://msdn.microsoft.com/en-us/magazine/dn802602.aspx
                // https://davefancher.com/2014/08/14/c-6-0-null-propagation-operator/
                var validated = (serviceCasePost.Message?.ValidateMinMaxLength(1, 2000) ?? false)
                    && (serviceCasePost.Name?.ValidateMinMaxLength(2, 30) ?? false);
                if (serviceCasePost.ContactEmail != null && serviceCasePost.ContactEmail != "")
                {
                    validated = validated
                        && (serviceCasePost.ContactEmail?.ValidateMinMaxLength(6, 40) ?? false)
                        && (serviceCasePost.ContactEmail?.ValidateRegex(InputValidator.RegexEmail) ?? false);
                }
                if (!validated)
                {
                    serviceCasePost.Errors.Add(ApiErrors.InputValidationFailed.ToString());
                    return serviceCasePost;
                }

                // Försök att spara
                try
                {
                    serviceCase.Posts.Add(serviceCasePost);
                    context.SaveChanges();
                    // Om det bara finns ett inlägg så betyder det att caset precis har skapats, 
                    // och det ska då skickas ut ett bekräftelsemejl.
                    // i annat fall ska det skickas ut en kopia på inlägget till den som skapade ärendet
                    // förutsatt att private-flaggan inte är satt.
                    // Metoden SendLastPostToTennant kollar själv, och skickar inte något mejl
                    // om private-flaggan är satt eller om det inte finns någon riktig mejladress.
                    if (serviceCase.Posts.Count == 1)
                    {
                        SendMailSimple.SendRegistrationConfirmation(serviceCase);
                    }
                    else
                    {
                        SendMailSimple.SendLastPostToTennant(serviceCase);
                    }
                }
                catch (Exception ex)
                             when (ex is DbUpdateException
                             || ex is DbUpdateConcurrencyException
                             || ex is DbEntityValidationException
                             || ex is NotSupportedException
                             || ex is ObjectDisposedException
                             || ex is InvalidOperationException
                             )
                // Dessa är alla exceptions som  context.SaveChanges() kan kasta. Info hämtad ifrån:
                // https://docs.microsoft.com/en-us/dotnet/api/system.data.entity.dbcontext.savechanges?view=entity-framework-6.2.0
                {
                    serviceCasePost.Errors.Add($"Exception in AddPost(): {ex.Message}");
                }
                return serviceCasePost;
            }
        }

        public List<ServiceCase> GetAllCases()
        {
            using (var context = new ApplicationDbContext())
            {
                // returnerar bara ServiceCases utan några ServiceCasePosts
                // eftersom lazyloading är deaktiverat. Propertyn
                // Posts behövs ju ändå inte i en lista över ServiceCases
                return context.ServiceCases.ToList();
            }
        }

        public ServiceCase GetCase(int caseNr)
        {
            using (var context = new ApplicationDbContext())
            {
                // Här används eager loading för att hämta upp ServiceCasePosts
                // Eager loading är att föredra (enligt nedanstående länk) framför
                // lazy loading när man gör serialization.
                //
                // "Lazy loading and serialization don’t mix well, and if you 
                // aren’t careful you can end up querying for your 
                // entire database just because lazy loading is enabled."
                // 
                // Eftersom WCF bygger på att serialisera klasser till XML så 
                // har jag stängt av det för ApplicationDbContext genom att 
                // lägga till denna rad i konstruktorn:
                //     Configuration.LazyLoadingEnabled = false;
                //
                // Läs mer på:
                // https://docs.microsoft.com/en-us/ef/ef6/querying/related-data

                var serviceCase = context.ServiceCases
                    // Leta upp case utifrån caseNr
                    .Where(sCase => sCase.CaseNr == caseNr)
                    // Inkludera alla ServiceCasePosts genom eager loading
                    .Include(sc => sc.Posts)
                    // Ta den första posten ur träfflistan, och om ingen finns
                    // och FirstOrDefault returnerar null så skapa ett nytt
                    // ServiceCase-objekt med ett felmeddelande
                    .FirstOrDefault();
                if (serviceCase == null)
                {
                    serviceCase = new ServiceCase { Name = "Inget serviceärende funnet." };
                    serviceCase.Errors.Add(ApiErrors.NoMatchingServiceCaseFound.ToString());
                }
                return serviceCase;
            }
        }
    }
}
