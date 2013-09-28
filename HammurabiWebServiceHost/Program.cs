using System;
using Akkadian.Service;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Akkadian.ServiceHost
{
    /// <summary>
    /// This class provides self-hosting for the AkkadianWebService.
    /// </summary>
    class SelfHost
    {
        static void Main(string[] args)
        {
            WebService DemoServices = new WebService();
            WebHttpBinding binding = new WebHttpBinding();
            WebServiceHost serviceHost = new WebServiceHost(DemoServices, new Uri("http://localhost:8000/"));
            serviceHost.AddServiceEndpoint(typeof(IWebService), binding, "");
            serviceHost.Open();
            Console.ReadKey();
            serviceHost.Close();
        }
    }
}