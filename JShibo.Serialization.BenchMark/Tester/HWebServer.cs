using JShibo.Net.WebServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JShibo.Serialization.BenchMark.Tester
{
    public class HWebServer : SimpleWebServer
    {
        string s = ShiboRandom.GetRandomString(100000);

        public override void Execute(System.Net.HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;
            string responseString = s;// "<html><body>hello world</body></html>";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            var output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            // You must close the output stream.
            output.Close();
            //base.Execute(context);
        }
    }
}
