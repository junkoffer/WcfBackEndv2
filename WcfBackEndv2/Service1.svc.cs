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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public ServiceCase CreateCase(ServiceCase serviceCase)
        {

            using (var context = new ApplicationDbContext())
            {
                var nextCaseNr = context
                    .ServiceCases
                    .OrderByDescending(c => c.CaseNr)
                    .FirstOrDefault() ?? new ServiceCase();
                serviceCase.Date = DateTime.Now;
                serviceCase.CaseNr = nextCaseNr.CaseNr + 1;
                context.Entry(serviceCase).State = EntityState.Added;
                context.SaveChanges();
            }
            return serviceCase;
        }


        public ServiceCasePost AddPost(int caseNr, ServiceCasePost serviceCasePost)
        {
            return new ServiceCasePost();
        }

        public List<ServiceCase> GetAllCases()
        {
            using (var context = new ApplicationDbContext())
            {
                return context.ServiceCases.ToList();
            }
        }

        public ServiceCase GetCase(int caseNr)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.ServiceCases
                    .Where(serviceCase => serviceCase.CaseNr == caseNr)
                    .FirstOrDefault() ?? new ServiceCase { Name = "Finns inte" };
            }
        }
    }
}
