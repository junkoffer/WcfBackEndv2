using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfBackEndv2
{
    public class Service1 : IService1
    {
        public ServiceCase CreateCase(ServiceCase serviceCase)
        {
            using (var context = new ApplicationDbContext())
            {
                var nextCaseNr = context
                    .ServiceCases
                    .OrderByDescending(sc => sc.CaseNr)
                    .FirstOrDefault() ?? new ServiceCase();
                serviceCase.Date = DateTime.Now;
                serviceCase.CaseNr = nextCaseNr.CaseNr + 1;

                // lägg till serviceCase till context
                context.Entry(serviceCase).State = EntityState.Added;
                // och skicka ändringarna till databasen
                context.SaveChanges();
            }
            return serviceCase;
        }


        public ServiceCasePost AddPost(int caseNr, ServiceCasePost serviceCasePost)
        {
            using (var context = new ApplicationDbContext())
            {
                var serviceCase = context.ServiceCases
                    .Where(sc => sc.CaseNr == caseNr)
                    .FirstOrDefault();

                // Spara bara serviceCasePost om det finns et case med nummer 
                // som passar caseNr
                if (serviceCase != null)
                {
                    serviceCase.Posts.Add(serviceCasePost);
                    context.SaveChanges();
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

                return context.ServiceCases
                    // Leta upp case utifrån caseNr
                    .Where(serviceCase => serviceCase.CaseNr == caseNr)
                    // Inkludera alla ServiceCasePosts genom eager loading
                    .Include(sc=>sc.Posts)
                    // Ta den första posten ur träfflistan, och om ingen finns
                    // och FirstOrDefault returnerar null så skapa ett nytt
                    // ServiceCase-objekt med ett felmeddelande
                    .FirstOrDefault() ?? new ServiceCase { Name = "Finns inte" };
            }
        }
    }
}
