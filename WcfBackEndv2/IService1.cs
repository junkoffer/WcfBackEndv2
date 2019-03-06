using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfBackEndv2
{
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        string RegisterNewServiceCase();

        [OperationContract]
        List<ServiceCase> GetAllCases();

        [OperationContract]
        ServiceCase GetCase(int caseNr);

        [OperationContract]
        ServiceCase CreateCase(ServiceCase serviceCase);

        [OperationContract]
        ServiceCasePost AddPost(int caseNr, ServiceCasePost serviceCasePost);
    }
}
