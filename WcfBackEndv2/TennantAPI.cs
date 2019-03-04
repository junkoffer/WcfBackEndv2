using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfBackEndv2
{
    public class TenantWcfService
    {
        public ServiceCase GetCase(int caseNr)
        {
            return new ServiceCase();
        }

        public int CreateCase(ServiceCase serviceCase)
        {
            return 0; // här kan vi skicka felkoder framöver 
        }

        public int AddPost(int caseNr, ServiceCasePost serviceCasePost)
        {
            return 0;  // här kan vi skicka felkoder framöver 
        }

    }
}