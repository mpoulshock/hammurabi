using System.ServiceModel;

namespace HService
{
    [ServiceContract]
    public interface IWebService
    {
        [OperationContract]
        HammurabiResponse HammurabiAssess(HammurabiRequest request);
    }
}
