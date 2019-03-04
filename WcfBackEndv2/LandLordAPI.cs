using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfBackEndv2
{
    public class LandlordWcfService
    {
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

        public int AddPost(int caseNr, ServiceCasePost serviceCasePost)
        {
            return 0; // här kan vi skicka felkoder framöver 
        }
    }
}