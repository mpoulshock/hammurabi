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
        Response HammurabiAssessTemp();

        [OperationContract]
        Response HammurabiAssess(Request request);
    }
}
