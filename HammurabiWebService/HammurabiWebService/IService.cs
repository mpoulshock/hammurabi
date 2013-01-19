using System.ServiceModel;
using System.ServiceModel.Web;

namespace HammurabiWebService
{
    [ServiceContract]
    public interface IWebService
    {
        // http://localhost:8000/AssessTest (for testing purposes)
        [OperationContract]
        [WebGet(UriTemplate = "/AssessTest")]
        Response AssessTest();

        // http://localhost:8000/Assess
        [OperationContract]
        Response Assess(Request request);
    }
}
