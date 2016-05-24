using JShibo.Serialization.BenchMark.RazorTest;
using Microsoft.CSharp;
using Newtonsoft.Json;
//using RazorEngine;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Razor;

namespace JShibo.Serialization.BenchMark.Tester
{
    public class RazorTester
    {
        public static void Run()
        {
            //string template = "Hello @Model.Name! Welcome to Razor!";
            ////string result = Razor.Parse(template, new { Name = "World" });
            ////Console.WriteLine(result);

            //System.Drawing.Image _Image = System.Drawing.Image.FromFile("i:\\favicon.ico");
            //_Image.Save("i:\\favicon.png", System.Drawing.Imaging.ImageFormat.Png);



            ////string template = File.ReadAllText("BrandBox.txt");
            //SelectCarModel model = new SelectCarModel() { BrandId = "1", BrandName = "大众", IsBrand = true };
            //model.CarList = new List<SelectCarItem>();
            //model.CarList.Add(new SelectCarItem() { Brandname = "福克斯", Seriesid = 364 });
            //model.CarList.Add(new SelectCarItem() { Brandname = "哈弗H6", Seriesid = 456 });

            ////string json = JsonConvert.SerializeObject(model);
            ////Console.WriteLine(json);

            ////Razor.Compile(template, "template");

            //Stopwatch w = Stopwatch.StartNew();
            ////string result = Razor.Parse(template, model);
            //string result = Razor.Parse(template, new { Name = "World" });
            ////Create();
            //w.Stop();
            //Console.WriteLine(w.ElapsedMilliseconds);
            ////Console.WriteLine(result);


            //w.Restart();
            ////result = Razor.Parse(template, model);
            //result = Razor.Parse(template, new { Name = "World" });
            //w.Stop();
            //Console.WriteLine(w.ElapsedMilliseconds);
        }

        /// <summary>
        /// http://www.cnblogs.com/cxd4321/p/3365500.html
        /// </summary>
        private static void Create()
        {
            //簡單的範本
            string template = @"@{var name=""Would"";}
	                                Hello @name!!";

            var input = new System.IO.StringReader(template);

            //產生Razor的TemplateEngine
            var host = new RazorEngineHost(new CSharpRazorCodeLanguage());
            host.DefaultBaseClass = "RezorCodeDomSample.MyTemplate";
            host.DefaultNamespace = "RezorCodeDomSample";
            host.DefaultClassName = "MyTemplateResult";
            var engine = new RazorTemplateEngine(host);

            //取得結果的CodeDom
            var code = engine.GenerateCode(input);
            var codeType = code.GeneratedCode.Namespaces[0].Types[0];
            var codeProvider = new CSharpCodeProvider();

            //將CodeDom輸出到檔案中
            //CodeGeneratorOptions options = new CodeGeneratorOptions();
            //options.BlankLinesBetweenMembers = true;
            //System.IO.StringWriter sw = new System.IO.StringWriter();
            //codeProvider.GenerateCodeFromCompileUnit(code.GeneratedCode, sw, options);
            //File.WriteAllText("c:\\text.cs", sw.ToString());

            //將CodeDom編譯
            var options = new CompilerParameters()
            {
                GenerateInMemory = true,
                GenerateExecutable = false,
            };
            options.ReferencedAssemblies.Add(typeof(Program).Assembly.Location);
            var asselby = codeProvider.CompileAssemblyFromDom(options, code.GeneratedCode);

            //執行Template
            var type = asselby.CompiledAssembly.GetType("RezorCodeDomSample.MyTemplateResult");
            var ins = Activator.CreateInstance(type) as MyTemplate;
            ins.Execute();
            Console.Write(ins.Reault);
        }

        //如果沒有Base類會不好處理
        public class MyTemplate
        {
            private StringBuilder sb = new StringBuilder();

            public virtual void Execute()
            {
            }

            public void Write(object value)
            {
                sb.Append(value);
            }

            public void WriteLiteral(object value)
            {
                sb.Append(value);
            }

            public string Reault
            {
                get { return sb.ToString(); }
            }
        }


    }
}
