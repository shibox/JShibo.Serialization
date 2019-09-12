using JShibo.Serialization.Transpose;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace JShibo.Serialization.BenchMark.Tester
{
    public class ColumnConvertTest
    {
        //static PivotEncodeObjects o = new PivotEncodeObjects();
        static BindingFlags flag = BindingFlags.NonPublic |
                                   BindingFlags.Instance |
                                   BindingFlags.Public;


        public static void Test1()
        {
            //string conn = @"Data Source = 10.168.100.194\MSSQLSERVER2014; Initial Catalog = Search; User Id = autoWriter; Password = autohomeWriter;";
            //string sql = "select * from [AppLogInfo]";
            //IDataReader reader = SqlServerHelper.ExecuteReader(conn, sql);
            //ColumnsResult result = ShiboSerializer.Serialize(reader);
            //Console.WriteLine(result);
            //Console.ReadLine();


            ////PivotEncodeObjects o = new PivotEncodeObjects();

            //Type[] types = new Type[reader.FieldCount];
            //for (int i = 0; i < types.Length; i++)
            //    types[i] = reader.GetFieldType(i);
            ////SerializeWrite<PivotEncodeObjects> info = Instance.builder.GenerateSerializationWriteType<PivotEncodeObjects>(types);
            //PivotEncodeObjects o = new PivotEncodeObjects(types, 10000);
            ////o.objs = new object[types.Length];


            //o.ReadDateTime();
            //o.ReadString();
            //o.ReadInt32();

            A a = new Tester.ColumnConvertTest.A();
            GenerateSerializationWriteType<A>()(a);
        }

        internal static SerializeWrite<T> GenerateSerializationWriteType<T>()
        {
            DynamicMethod dynamicGet = new DynamicMethod("SerializationWrite", typeof(void), new Type[] { typeof(T) }, typeof(object), true);
            ILGenerator mthdIL = dynamicGet.GetILGenerator();

            //LocalBuilder tpmEvent = mthdIL.DeclareLocal(type);
            //mthdIL.Emit(OpCodes.Nop);
            //mthdIL.Emit(OpCodes.Ldarg_1);//PU
            //mthdIL.Emit(OpCodes.Castclass, type);//PU
            //mthdIL.Emit(OpCodes.Stloc, tpmEvent);//PP

            //mthdIL.Emit(OpCodes.Stloc_S, typeof(T));//PU
            //mthdIL.Emit(OpCodes.Ldloc_S, typeof(T));//PU
            //for (int i = 0; i < types.Length; i++)
            //{
            //    Type t = types[i];
            //    MethodInfo brRead = typeof(T).GetMethod(CreateReaderMethod(t), flag, null, new Type[] { }, null);

            //    //mthdIL.Emit(OpCodes.Ldloc, tpmRetEvent);//load new obj on ES
            //    //mthdIL.Emit(OpCodes.Ldarg_0);//PU binary reader ,load BR on ES
            //    mthdIL.Emit(OpCodes.Ldloc_S, typeof(T));//PU
            //    mthdIL.EmitCall(OpCodes.Callvirt, brRead, null);//PU
            //    mthdIL.Emit(OpCodes.Nop);
            //}

            MethodInfo brRead = typeof(T).GetMethod("Exec", flag, null, new Type[] { }, null);

            mthdIL.Emit(OpCodes.Nop);
            mthdIL.EmitCall(OpCodes.Callvirt, brRead, null);//PU

            mthdIL.Emit(OpCodes.Ret);
            return (SerializeWrite<T>)dynamicGet.CreateDelegate(typeof(SerializeWrite<T>));
        }

        public class A
        {
            public void Exec()
            {
                Console.WriteLine("aaaaa");
            }
        }

    }
}
