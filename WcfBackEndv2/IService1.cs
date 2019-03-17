using System.Collections.Generic;
using System.ServiceModel;
using WcfBackEndv2.Model;

namespace WcfBackEndv2
{
    [ServiceContract]
    public interface IService1
    {
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
