using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JShibo.Net.WebServer;

namespace JShibo.Serialization.BenchMark.Tester
{
    public class WebServerTester
    {
        public static void Run()
        {
            //TestCase1(); 
            //TestCase2();
            TestCase3();
        }

        public static void TestCase1()
        {
            string s = ShiboRandom.GetRandomString(10000);

            var server = new HttpListener();
            server.Prefixes.Add("http://127.0.0.1:1986/");
            server.Stop();
            server.Start();
            while (true)
            {
                server.BeginGetContext(req =>
                {
                    var listener = (HttpListener)req.AsyncState;
                    var context = listener.EndGetContext(req);
                    var request = context.Request;
                    var response = context.Response;
                    string responseString = "<html><body>hello world</body></html>";
                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                    response.ContentLength64 = buffer.Length;
                    var output = response.OutputStream;
                    output.Write(buffer, 0, buffer.Length);
                    // You must close the output stream.
                    output.Close();
                    
                }, server);
                Thread.Sleep(1);
            }

            //server.BeginGetContext(req =>
            //{
            //    var listener = (HttpListener)req.AsyncState;
            //    var context = listener.EndGetContext(req);
            //    var request = context.Request;
            //    var response = context.Response;
            //    string responseString = "<html><body>hello world</body></html>";
            //    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            //    response.ContentLength64 = buffer.Length;
            //    var output = response.OutputStream;
            //    output.Write(buffer, 0, buffer.Length);
            //    // You must close the output stream.
            //    output.Close();
            //}, server);

            Console.ReadKey();
        }

        public static void TestCase2()
        {
            var server = new HWebServer();
            server.Address = "127.0.0.1";
            server.Port = 1986;
            server.Start();
            Console.ReadKey();
        }

        public static void TestCase3()
        {
            string s = ShiboRandom.GetRandomString(10000);

            var server = new HttpListener();
            server.Prefixes.Add("http://127.0.0.1:1986/");
            server.Stop();
            server.Start();
            //while (true)
            //{
            server.BeginGetContext(GetContextCallBack, server);
            //    Thread.Sleep(1);
            //}
            Console.ReadKey();
        }

        static string s = ShiboRandom.GetRandomString(100000);

        static void GetContextCallBack(IAsyncResult ar)
        {

            try
            {
                var sSocket = ar.AsyncState as HttpListener;
                HttpListenerContext context = sSocket.EndGetContext(ar);
                sSocket.BeginGetContext(new AsyncCallback(GetContextCallBack), sSocket);
                //context.Response.Close();

                var request = context.Request;
                var response = context.Response;
                // 先打开下一次context的大门 然后我在这个后面处理别的事情
                string responseString = s;// "<html><body>hello world</body></html>";
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;
                var output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
            }

            catch { }

        }

    }
}
