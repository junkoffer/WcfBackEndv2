using System;
using System.Collections.Generic;
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
        public ServiceCasePost AddPost(int caseNr, ServiceCasePost serviceCasePost)
        {
            return new ServiceCasePost();
        }

        public ServiceCase CreateCase(ServiceCase serviceCase)
        {
            return new ServiceCase();
        }

        public List<ServiceCase> GetAllCases()
        {
            return new List<ServiceCase>();
        }

        public ServiceCase GetCase(int caseNr)
        {
            return new ServiceCase();
        }

        public string RegisterNewServiceCase()
        {
            using (var context = new ApplicationDbContext())
            {
                var nextCaseNr = context
                    .ServiceCases
                    .OrderByDescending(c => c.CaseNr)
                    .FirstOrDefault() ?? new ServiceCase();

                var serviceCase = new ServiceCase()
                {
                    Date = DateTime.Now,
                    CaseNr = nextCaseNr.CaseNr + 1,
                };
                context.Entry(serviceCase).State = System.Data.Entity.EntityState.Added;
                context.SaveChanges();
                return string.Format("Nytt ärende med id {0}).",
                    serviceCase.CaseNr);
            }
        }
    }
}
