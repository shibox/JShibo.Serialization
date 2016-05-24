using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JShibo.Serialization.BenchMark.Typer
{
    /// <summary>
    /// 随机生成成类型，用于序列化测试
    /// 实现自动化
    /// </summary>
    public class TypeCreater
    {
        #region 字段

        static Dictionary<string, Type> typeMap = new Dictionary<string, Type>();

        //static Type[] typeBase = new Type[] { typeof(bool), typeof(char), typeof(sbyte), typeof(byte), typeof(Int16), typeof(UInt16), typeof(Int32), typeof(UInt32), 
        //                                   typeof(Int64),typeof(UInt64),typeof(Single),typeof(double),typeof(decimal),typeof(DateTime),typeof(string),typeof(Guid)
        //                                   };

        //static Type[] typeBase = new Type[] { typeof(bool), typeof(char), typeof(sbyte), typeof(byte), typeof(Int16), typeof(UInt16), typeof(Int32), typeof(UInt32), 
        //                                   typeof(Int64),typeof(UInt64),typeof(Single),typeof(double),typeof(decimal),typeof(string)
        //                                   };

        static Type[] typeBase = new Type[] { typeof(bool), typeof(char), typeof(sbyte), typeof(byte), typeof(Int16), typeof(UInt16), typeof(Int32), typeof(UInt32), 
                                           typeof(Int64),typeof(UInt64),typeof(Single),typeof(double),typeof(string)
                                           };

        //static Type[] typeBase = new Type[] {typeof(string)
        //                                   };

        static Type[] typeArray = new Type[] { typeof(bool[]), typeof(char[]), typeof(sbyte[]), typeof(byte[]), typeof(Int16[]), typeof(UInt16[]), typeof(Int32[]), typeof(UInt32[]), 
                                           typeof(Int64[]),typeof(UInt64[]),typeof(Single[]),typeof(double[]),typeof(decimal[]),typeof(DateTime[]),typeof(string[]),typeof(Guid[])
                                           };

        static Type[] typeList = new Type[] { typeof(List<bool>), typeof(List<char>), typeof(List<sbyte>), typeof(List<byte>), typeof(List<Int16>), typeof(List<UInt16>), typeof(List<Int32>), typeof(List<UInt32>), 
                                           typeof(List<Int64>),typeof(List<UInt64>),typeof(List<Single>),typeof(List<double>),typeof(List<decimal>),typeof(List<DateTime>),typeof(List<string>),typeof(List<Guid>)
                                           };

        static Type[] typeIList = new Type[] { typeof(IList<bool>), typeof(IList<char>), typeof(IList<sbyte>), typeof(IList<byte>), typeof(IList<Int16>), typeof(IList<UInt16>), typeof(IList<Int32>), typeof(IList<UInt32>), 
                                           typeof(IList<Int64>),typeof(IList<UInt64>),typeof(IList<Single>),typeof(IList<double>),typeof(IList<decimal>),typeof(IList<DateTime>),typeof(IList<string>),typeof(IList<Guid>)
                                           };

        static Type[] typeDictionaryInt = new Type[] { typeof(Dictionary<int,bool>), typeof(Dictionary<int,char>), typeof(Dictionary<int,sbyte>), typeof(Dictionary<int,byte>), typeof(Dictionary<int,Int16>), typeof(Dictionary<int,UInt16>), typeof(Dictionary<int,Int32>), typeof(Dictionary<int,UInt32>), 
                                           typeof(Dictionary<int,Int64>),typeof(Dictionary<int,UInt64>),typeof(Dictionary<int,Single>),typeof(Dictionary<int,double>),typeof(Dictionary<int,decimal>),typeof(Dictionary<int,DateTime>),typeof(Dictionary<int,string>),typeof(Dictionary<int,Guid>)
                                           };

        static Type[] typeDictionaryLong = new Type[] { typeof(Dictionary<long,bool>), typeof(Dictionary<long,char>), typeof(Dictionary<long,sbyte>), typeof(Dictionary<long,byte>), typeof(Dictionary<long,Int16>), typeof(Dictionary<long,UInt16>), typeof(Dictionary<long,Int32>), typeof(Dictionary<long,UInt32>), 
                                           typeof(Dictionary<long,Int64>),typeof(Dictionary<long,UInt64>),typeof(Dictionary<long,Single>),typeof(Dictionary<long,double>),typeof(Dictionary<long,decimal>),typeof(Dictionary<long,DateTime>),typeof(Dictionary<long,string>),typeof(Dictionary<long,Guid>)
                                           };

        static Type[] typeDictionaryString = new Type[] { typeof(Dictionary<string,bool>), typeof(Dictionary<string,char>), typeof(Dictionary<string,sbyte>), typeof(Dictionary<string,byte>), typeof(Dictionary<string,Int16>), typeof(Dictionary<string,UInt16>), typeof(Dictionary<string,Int32>), typeof(Dictionary<string,UInt32>), 
                                           typeof(Dictionary<string,Int64>),typeof(Dictionary<string,UInt64>),typeof(Dictionary<string,Single>),typeof(Dictionary<string,double>),typeof(Dictionary<string,decimal>),typeof(Dictionary<string,DateTime>),typeof(Dictionary<string,string>),typeof(Dictionary<string,Guid>)
                                           };

        #endregion

        #region 方法

        public static Type TestCreate()
        {
            TypeMeta info = new TypeMeta();
            info.AssemblyName = "TestAssemblyName";
            info.ClassName = "TestClassName";
            //info.Tokens.Add(new TypeToken("TestField1",typeof(int)));
            //info.Tokens.Add(new TypeToken("TestField2", typeof(string)));
            //info.Tokens.Add(new TypeToken("TestField3", typeof(long)));
            info.Tokens.Add(new TypeToken("TestField4", typeof(DateTime)));
            return Creater(info);
        }

        public static Type Create(TypeMeta info)
        {
            Stopwatch w = Stopwatch.StartNew();
            //动态创建程序集
            AssemblyName asm = new AssemblyName(info.AssemblyName);
            AssemblyBuilder dynamicAssembly = AppDomain.CurrentDomain.DefineDynamicAssembly(asm, AssemblyBuilderAccess.RunAndSave);
            //动态创建模块
            ModuleBuilder mb = dynamicAssembly.DefineDynamicModule(asm.Name, asm.Name + ".dll");
            //动态创建类MyClass
            TypeBuilder tb = mb.DefineType(info.ClassName, TypeAttributes.Public);
            foreach (TypeToken item in info.Tokens)
            {
                //动态创建字段
                FieldBuilder fb = tb.DefineField(item.Name, item.Type, FieldAttributes.Public);
            }
            //动态创建构造函数
            //Type[] clorType = new Type[] { typeof(System.String) };
            //ConstructorBuilder cb1 = tb.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, clorType);
            Type[] clorType = new Type[] { };
            ConstructorBuilder cb1 = tb.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, clorType);
            //生成指令
            ILGenerator ilg = cb1.GetILGenerator();//生成 Microsoft 中间语言 (MSIL) 指令
            //ilg.Emit(OpCodes.Ldarg_0);
            //ilg.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes));
            //ilg.Emit(OpCodes.Ldarg_0);
            //ilg.Emit(OpCodes.Ldarg_1);
            //foreach (TypeToken item in info.Tokens)
            //{
            //    //动态创建字段
            //    FieldBuilder fb = tb.DefineField(item.Name, item.Type, FieldAttributes.Public);
            //    ilg.Emit(OpCodes.Stfld, fb);
            //}
            ilg.Emit(OpCodes.Ret);
            ////动态创建属性
            //PropertyBuilder pb = tb.DefineProperty("MyProperty", PropertyAttributes.HasDefault, typeof(string), null);
            ////动态创建方法
            //MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName;
            //MethodBuilder myMethod = tb.DefineMethod("get_Field", getSetAttr, typeof(string), Type.EmptyTypes);
            ////生成指令
            //ILGenerator numberGetIL = myMethod.GetILGenerator();
            //numberGetIL.Emit(OpCodes.Ldarg_0);
            //numberGetIL.Emit(OpCodes.Ldfld, fb);
            //numberGetIL.Emit(OpCodes.Ret);
            //使用动态类创建类型
            Type classType = tb.CreateType();
            //保存动态创建的程序集 (程序集将保存在程序目录下调试时就在Debug下)
            dynamicAssembly.Save(asm.Name + ".dll");
            //创建类
            w.Stop();
            Console.WriteLine(w.ElapsedMilliseconds);
            return classType;
        }

        public static Type Creater(TypeMeta info)
        {
            //动态创建程序集
            AssemblyName asm = new AssemblyName(info.AssemblyName);
            AssemblyBuilderAccess aba = AssemblyBuilderAccess.Run;
            if (info.IsSaveAssembly == true)
                aba = AssemblyBuilderAccess.RunAndSave;
            AssemblyBuilder asb = AppDomain.CurrentDomain.DefineDynamicAssembly(asm, aba);
            //动态创建模块
            ModuleBuilder mb = null;
            if (info.IsSaveAssembly == true)
                mb = asb.DefineDynamicModule(asm.Name, asm.Name + ".dll");
            else
                mb = asb.DefineDynamicModule(asm.Name);
            //动态创建类MyClass
            TypeBuilder tb = mb.DefineType(info.ClassName, TypeAttributes.Public);
            //动态创建字段
            foreach (TypeToken item in info.Tokens)
                tb.DefineField(item.Name, item.Type, FieldAttributes.Public);

            //动态创建构造函数
            ConstructorBuilder ctb = tb.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, Type.EmptyTypes);
            //生成 Microsoft 中间语言 (MSIL) 指令
            ILGenerator ilg = ctb.GetILGenerator();
            ilg.Emit(OpCodes.Ret);
            Type type = tb.CreateType();
            //保存动态创建的程序集
            if (info.IsSaveAssembly)
                asb.Save(asm.Name + ".dll");
            //使用动态类创建类型
            return type;
        }

        //public static Type DynamicCreateType()
        //{
        //    //动态创建程序集
        //    AssemblyName DemoName = new AssemblyName("DynamicAssembly");
        //    AssemblyBuilder dynamicAssembly = AppDomain.CurrentDomain.DefineDynamicAssembly(DemoName, AssemblyBuilderAccess.RunAndSave);
        //    //动态创建模块
        //    ModuleBuilder mb = dynamicAssembly.DefineDynamicModule(DemoName.Name, DemoName.Name + ".dll");
        //    //动态创建类MyClass
        //    TypeBuilder tb = mb.DefineType("MyClass", TypeAttributes.Public);
        //    //动态创建字段
        //    //FieldBuilder fb = tb.DefineField("myField", typeof(System.String), FieldAttributes.Public);
        //    //FieldBuilder fb2 = tb.DefineField("myField2", typeof(System.String), FieldAttributes.Public);

        //    tb.DefineField("myField", typeof(System.String), FieldAttributes.Public);
        //    tb.DefineField("myField2", typeof(System.Int16), FieldAttributes.Public);

        //    //动态创建构造函数
        //    Type[] clorType = new Type[] { typeof(System.String) };
        //    ConstructorBuilder cb1 = tb.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, clorType);
        //    ////生成指令
        //    ILGenerator ilg = cb1.GetILGenerator();//生成 Microsoft 中间语言 (MSIL) 指令
        //    //ilg.Emit(OpCodes.Ldarg_0);
        //    //ilg.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes));
        //    //ilg.Emit(OpCodes.Ldarg_0);
        //    //ilg.Emit(OpCodes.Ldarg_1);
        //    //ilg.Emit(OpCodes.Stfld, fb);
        //    ilg.Emit(OpCodes.Ret);
        //    ////动态创建属性
        //    //PropertyBuilder pb = tb.DefineProperty("MyProperty", PropertyAttributes.HasDefault, typeof(string), null);
        //    ////动态创建方法
        //    //MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName;
        //    //MethodBuilder myMethod = tb.DefineMethod("get_Field", getSetAttr, typeof(string), Type.EmptyTypes);
        //    ////生成指令
        //    //ILGenerator numberGetIL = myMethod.GetILGenerator();
        //    //numberGetIL.Emit(OpCodes.Ldarg_0);
        //    //numberGetIL.Emit(OpCodes.Ldfld, fb);
        //    //numberGetIL.Emit(OpCodes.Ret);
        //    //使用动态类创建类型
        //    Type classType = tb.CreateType();
        //    //保存动态创建的程序集 (程序集将保存在程序目录下调试时就在Debug下)
        //    dynamicAssembly.Save(DemoName.Name + ".dll");
        //    //创建类
        //    return classType;
        //}

        public static Type Create()
        {
            return Create(new Random().Next(1, 100), 1);
        }

        /// <summary>
        /// 创建一个对象
        /// </summary>
        /// <param name="count">对象中包含object的数量</param>
        /// <returns></returns>
        public static Type Create(int count)
        {
            return Create(count, 1);
        }

        public static Type Create(int count, int seed)
        {
            Dictionary<int, bool> createCache = new Dictionary<int,bool>();
            Dictionary<Type, bool> typeCache = new Dictionary<Type,bool>();
            Random rd = new Random(seed);
            Type root = null;
            int usedCount = 0;
            string assemblyName = "ass_" + GetNext(rd, createCache).ToString();
            while (usedCount < count)
            {
                int n = rd.Next(1, count - usedCount);
                TypeMeta info = new TypeMeta();
                info.ClassName = "c_" + GetNext(rd, createCache).ToString();
                info.AssemblyName = assemblyName;
                Dictionary<int, bool> fmap = new Dictionary<int, bool>();
                for (int j = 0; j < n; j++)
                {
                    Type type = typeBase[rd.Next(0, typeBase.Length)];
                    int fid = GetNext(rd, createCache);
                    string name = "f_" + GetNext(rd, createCache).ToString();
                    info.Tokens.Add(new TypeToken(name, type));
                }
                usedCount += n;

                root = Creater(info);
                break;
            }
            return root;
        }

        //public static List<Type> Creates(int typeCount, int count, int seed)
        //{
        //    List<Type> types = new List<Type>();
        //    Random rd = new Random();
        //    string assemblyName = "ass_" + rd.Next(0, int.MaxValue);
        //    for (int i = 0; i < count; )
        //    {
        //        int n = rd.Next(1, count);
        //        TypeMeta info = new TypeMeta();
        //        info.ClassName = "c_" + rd.Next(0, int.MaxValue);
        //        info.AssemblyName = assemblyName;
        //        for (int j = 0; j < n; j++)
        //        {
        //            Type type = typeBase[rd.Next(0, typeBase.Length - 1)];
        //            string name = "f_" + rd.Next(0, int.MaxValue);
        //            info.Tokens.Add(new TypeToken(name, type));
        //        }
        //        types.Add(Creater(info));
        //        //root = Creater(info);
        //        break;
        //    }
        //    return null;
        //}

        #endregion

        #region 私有

        /// <summary>
        /// 获得一个不重复的随机数
        /// </summary>
        /// <param name="rd"></param>
        /// <param name="cache"></param>
        private static int GetNext(Random rd,Dictionary<int,bool> cache)
        {
            if (rd == null)
                rd = new Random();
            if (cache == null)
                cache = new Dictionary<int, bool>();
            int rdValue = 0;
            while (true)
            {
                rdValue = rd.Next(0,int.MaxValue);
                if (cache.ContainsKey(rdValue) == false)
                {
                    cache.Add(rdValue, false);
                    break;
                }
            }
            return rdValue;
        }

        #endregion
    }
}
