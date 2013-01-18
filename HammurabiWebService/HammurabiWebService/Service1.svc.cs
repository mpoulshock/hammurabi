
namespace HService
{
    public class WebService : IWebService
    {
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
