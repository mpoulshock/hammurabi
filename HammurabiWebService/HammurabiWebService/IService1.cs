using System.ServiceModel;
using System.ServiceModel.Web;

namespace HammurabiWebService
{
    [ServiceContract]
    public interface IWebService
    {
        // http://localhost:8000/HammurabiAssessTemp
        [OperationContract]
        [WebGet(UriTemplate = "/HammurabiAssessTemp")]
        HammurabiResponse HammurabiAssessTemp();

        [OperationContract]
        HammurabiResponse HammurabiAssess(HammurabiRequest request);
    }
}
