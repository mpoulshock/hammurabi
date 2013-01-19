using System;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace HammurabiWebService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
    public class WebService : IWebService
    {
        // Equivalent of "hello, world"
        public HammurabiResponse HammurabiAssessTemp()
        {
            return new HammurabiResponse(2);
        }

        public HammurabiResponse HammurabiAssess(HammurabiRequest request)
        {
            // Assert facts

            // Assemble goals into goal list

            // Send goal list to engine

            // Convert engine response into web service response

            // Create response (here, a dummy one)
            return new HammurabiResponse(request.Goals.Count);
        }
    }
}
