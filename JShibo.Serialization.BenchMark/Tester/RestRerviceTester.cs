using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace JShibo.Serialization.BenchMark.Tester
{
    public class RestRerviceTester
    {
        public static void Run()
        {

            RestDemoServices DemoServices = new RestDemoServices();

            WebHttpBinding binding = new WebHttpBinding();

            WebHttpBehavior behavior = new WebHttpBehavior();



            WebServiceHost _serviceHost = new WebServiceHost(DemoServices, new Uri("http://127.0.0.1:1986/"));

            _serviceHost.AddServiceEndpoint(typeof(IRESTDemoServices), binding, "");

            _serviceHost.Open();

            Console.ReadKey();

            _serviceHost.Close();

        }
    }

    public class Abc
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public static class Routing
    {
        public const string GetClientRoute = "/Client/{id}";

    }

    [ServiceContract(Name = "RESTDemoServices")]
    public interface IRESTDemoServices
    {

        [OperationContract]
        [WebGet(UriTemplate = Routing.GetClientRoute, BodyStyle = WebMessageBodyStyle.Bare,ResponseFormat=WebMessageFormat.Json)]
        string GetClientNameById(Abc Id);

    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single, IncludeExceptionDetailInFaults = true)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class RestDemoServices : IRESTDemoServices
    {

        public string GetClientNameById(Abc Id)
        {

            Random r = new Random();

            string ReturnString = "";

            for (int i = 0; i < Convert.ToUInt32(Id); i++)

                ReturnString += char.ConvertFromUtf32(r.Next(65, 85));



            return ReturnString;



        }

    }


}
