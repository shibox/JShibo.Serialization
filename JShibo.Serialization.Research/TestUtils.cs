//using System;
//using System.CodeDom.Compiler;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using Microsoft.CSharp;
//using Newtonsoft.Json;
//using System.Reflection.Emit;

//namespace JShibo.Serialization.BenchMark
//{
//    public class TestUtils
//    {
//        #region 字段

//        static string DefaultClassName = "Example.RootClass";

//        #endregion

//        #region 方法

//        public static Type Compile(string code, string json, ref TestResult result)
//        {
//            Type type = null;
//            Stopwatch w = Stopwatch.StartNew();
//            CodeDomProvider privoder = new CSharpCodeProvider();
//            CompilerParameters parameters = new CompilerParameters();
//            parameters.ReferencedAssemblies.Add("System.dll");
//            parameters.ReferencedAssemblies.Add("Newtonsoft.Json.dll");
//            parameters.GenerateExecutable = false;
//            parameters.GenerateInMemory = true;
//            CompilerResults cr = privoder.CompileAssemblyFromSource(parameters, code);
//            w.Stop();
//            result.Time = (int)w.ElapsedMilliseconds;
//            if (cr.Errors.HasErrors)
//            {
//                List<string> errors = new List<string>();
//                foreach (CompilerError err in cr.Errors)
//                    errors.Add(err.ErrorText);
//                result.Success = false;
//                result.Message = JsonConvert.SerializeObject(errors);
//            }
//            else
//            {
//                Assembly objAssembly = cr.CompiledAssembly;
//                type = objAssembly.GetType(DefaultClassName, true, true);
//                #region old
//                //object objHelloWorld = objAssembly.CreateInstance("DynamicCodeGenerate.HelloWorld");
//                //if (objHelloWorld != null)
//                //{
//                //    MethodInfo objMI = objHelloWorld.GetType().GetMethod("OutPut");
//                //}
//                //else
//                //{
//                //    object o = JsonConvert.DeserializeObject(json, objAssembly.GetType("Example.RootClass", true, true), new JsonSerializerSettings());

//                //}
//                #endregion
//            }
//            return type;
//        }

//        //public static Type CreateType()
//        //{
//        //    //动态创建程序集
//        //    AssemblyName DemoName = new AssemblyName("DynamicAssembly");
//        //    AssemblyBuilder dynamicAssembly = AppDomain.CurrentDomain.DefineDynamicAssembly(DemoName, AssemblyBuilderAccess.RunAndSave);
//        //    //动态创建模块
//        //    ModuleBuilder mb = dynamicAssembly.DefineDynamicModule(DemoName.Name, DemoName.Name + ".dll");
//        //    //动态创建类MyClass
//        //    TypeBuilder tb = mb.DefineType("MyClass", TypeAttributes.Public);
//        //    //动态创建字段
//        //    FieldBuilder fb = tb.DefineField("Abc", typeof(System.String), FieldAttributes.Private);
//        //    //动态创建构造函数
//        //    Type[] clorType = new Type[] { typeof(System.String) };
//        //    ConstructorBuilder cb1 = tb.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, clorType);
//        //    //生成指令
//        //    ILGenerator ilg = cb1.GetILGenerator();//生成 Microsoft 中间语言 (MSIL) 指令
//        //    ilg.Emit(OpCodes.Ldarg_0);
//        //    ilg.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes));
//        //    ilg.Emit(OpCodes.Ldarg_0);
//        //    ilg.Emit(OpCodes.Ldarg_1);
//        //    ilg.Emit(OpCodes.Stfld, fb);
//        //    ilg.Emit(OpCodes.Ret);
//        //    //动态创建属性
//        //    PropertyBuilder pb = tb.DefineProperty("MyProperty", PropertyAttributes.HasDefault, typeof(string), null);
//        //    //动态创建方法
//        //    MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName;
//        //    MethodBuilder myMethod = tb.DefineMethod("get_Property", getSetAttr, typeof(string), Type.EmptyTypes);
//        //    //生成指令
//        //    ILGenerator numberGetIL = myMethod.GetILGenerator();
//        //    numberGetIL.Emit(OpCodes.Ldarg_0);
//        //    numberGetIL.Emit(OpCodes.Ldfld, fb);
//        //    numberGetIL.Emit(OpCodes.Ret);
//        //    //保存动态创建的程序集
//        //    dynamicAssembly.Save(DemoName.Name + ".dll");
//        //    return null;
//        //    //return dynamicAssembly.GetType(DefaultClassName, true, true);
//        //}


//        public static Type DynamicCreateType()
//        {
//            Stopwatch w = Stopwatch.StartNew();
//            //动态创建程序集
//            AssemblyName DemoName = new AssemblyName("DynamicAssembly");
//            AssemblyBuilder dynamicAssembly = AppDomain.CurrentDomain.DefineDynamicAssembly(DemoName, AssemblyBuilderAccess.RunAndSave);
//            //动态创建模块
//            ModuleBuilder mb = dynamicAssembly.DefineDynamicModule(DemoName.Name, DemoName.Name + ".dll");
//            //动态创建类MyClass
//            TypeBuilder tb = mb.DefineType("MyClass", TypeAttributes.Public);
//            //动态创建字段
//            FieldBuilder fb = tb.DefineField("myField", typeof(System.String), FieldAttributes.Public);
//            //动态创建构造函数
//            //Type[] clorType = new Type[] { typeof(System.String) };
//            Type[] clorType = new Type[] { };
//            ConstructorBuilder cb1 = tb.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, clorType);
//            //生成指令
//            ILGenerator ilg = cb1.GetILGenerator();//生成 Microsoft 中间语言 (MSIL) 指令
//            //ilg.Emit(OpCodes.Ldarg_0);
//            //ilg.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes));
//            //ilg.Emit(OpCodes.Ldarg_0);
//            //ilg.Emit(OpCodes.Ldarg_1);
//            //ilg.Emit(OpCodes.Stfld, fb);
//            ilg.Emit(OpCodes.Ret);
//            ////动态创建属性
//            //PropertyBuilder pb = tb.DefineProperty("MyProperty", PropertyAttributes.HasDefault, typeof(string), null);
//            ////动态创建方法
//            //MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName;
//            //MethodBuilder myMethod = tb.DefineMethod("get_Field", getSetAttr, typeof(string), Type.EmptyTypes);
//            ////生成指令
//            //ILGenerator numberGetIL = myMethod.GetILGenerator();
//            //numberGetIL.Emit(OpCodes.Ldarg_0);
//            //numberGetIL.Emit(OpCodes.Ldfld, fb);
//            //numberGetIL.Emit(OpCodes.Ret);
//            //使用动态类创建类型
//            Type classType = tb.CreateType();
//            //保存动态创建的程序集 (程序集将保存在程序目录下调试时就在Debug下)
//            dynamicAssembly.Save(DemoName.Name + ".dll");
//            //创建类
//            w.Stop();
//            Console.WriteLine(w.ElapsedMilliseconds);
//            return classType;
//        }

//        public static object GetGraph(Type type, string json)
//        {
//            return JsonConvert.DeserializeObject(json, type, new JsonSerializerSettings());
//        }

//        #endregion


//    }
//}
