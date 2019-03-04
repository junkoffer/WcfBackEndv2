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
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
        public string RegisterNewServiceCase()
        {
            using (var context = new ApplicationDbContext())
            {
                var nextCaseNr = context
                    .ServiceCases
                    .OrderByDescending(c => c.CaseNr)
                    .FirstOrDefault()
                    .CaseNr;

                var serviceCase = new ServiceCase()
                {
                    Date = DateTime.Now,
                    CaseNr = nextCaseNr + 1,
                };
                context.Entry(serviceCase).State = System.Data.Entity.EntityState.Added;
                context.SaveChanges();
                return string.Format("Nytt ärende med id {0}).",
                    serviceCase.CaseNr);
            }
        }
    }
}
