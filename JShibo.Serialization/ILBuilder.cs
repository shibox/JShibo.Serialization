using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
//using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using JShibo.Serialization.Common;
using JShibo.Serialization.Json;

namespace JShibo.Serialization
{
    delegate void Serialize<T>(T agent, object value);
    delegate object Deserialize<T>(T agent);

    /// <summary>
    /// 中间代码生成器
    /// </summary>
    internal class ILBuilder
    {
        #region 方法

        //默认的绑定标记
        static BindingFlags flag = BindingFlags.NonPublic | 
                                   BindingFlags.Instance | 
                                   BindingFlags.Public;

        static bool IsIgnoreAttribute(object[] atts)
        {
            foreach (Attribute att in atts)
            {
                if (att is NotSerialized)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        //#region bin

        //private static string GetReaderMethod(Type type)
        //{
        //    if (type == typeof(Int32))
        //        return "ReadInt32";

        //    if (type == typeof(UInt32))
        //        return "ReadUInt32";

        //    if (type == typeof(UInt64))
        //        return "ReadUInt64";

        //    if (type == typeof(Int64))
        //        return "ReadInt64";

        //    if (type == typeof(char))
        //        return "ReadChar";

        //    if (type == typeof(char[]))
        //        return "ReadChars";

        //    if (type == typeof(UInt16))
        //        return "ReadUInt16";

        //    if (type == typeof(Int16))
        //        return "ReadInt16";

        //    else if (type == typeof(string))
        //        return "ReadString";

        //    else if (type == typeof(DateTime))
        //        return "ReadInt64";//ticks used in serialization

        //    else if (type == typeof(long))
        //        return "ReadInt64";

        //    else if (type == typeof(ulong))
        //        return "ReadUInt64";

        //    else if (type == typeof(bool))
        //        return "ReadBoolean";

        //    else if (type == typeof(byte))
        //        return "ReadByte";

        //    else if (type == typeof(sbyte))
        //        return "ReadSByte";

        //    else if (type == typeof(byte[]))
        //        return "ReadBytes";

        //    else if (type == typeof(decimal))
        //        return "ReadDecimal";

        //    else if (type == typeof(float))
        //        return "ReadSingle";

        //    else if (type == typeof(double))
        //        return "ReadDouble";

        //    else if (type == typeof(IDictionary))//currenly supports a string dictionary only
        //        return "ReadString";
        //    else if (type == typeof(IList))
        //        return "ReadString";
        //    else if (type == typeof(TimeSpan))
        //        return "ReadString";
        //    else if (type == typeof(Enum))
        //        return "ReadString";
        //    else if (type == typeof(Guid))
        //        return "ReadString";
        //    else if (type == typeof(DataTable))
        //        return "ReadString";
        //    else if (type == typeof(DataSet))
        //        return "ReadString";
        //    else if (type == typeof(Hashtable))
        //        return "ReadString";

        //    else if (type == typeof(int[]) || type == typeof(List<int>) || type == typeof(IList<int>))
        //        return "ReadInt32Array";

        //    else if (type == typeof(bool[]) || type == typeof(List<bool>) || type == typeof(IList<bool>))
        //        return "ReadInt32Array";
        //    else if (type == typeof(byte[]) || type == typeof(List<byte>) || type == typeof(IList<byte>))
        //        return "ReadInt32Array";
        //    else if (type == typeof(sbyte[]) || type == typeof(List<sbyte>) || type == typeof(IList<sbyte>))
        //        return "ReadInt32Array";
        //    else if (type == typeof(short[]) || type == typeof(List<short>) || type == typeof(IList<short>))
        //        return "ReadInt32Array";
        //    else if (type == typeof(ushort[]) || type == typeof(List<ushort>) || type == typeof(IList<ushort>))
        //        return "ReadInt32Array";
        //    else if (type == typeof(char[]) || type == typeof(List<char>) || type == typeof(IList<char>))
        //        return "ReadInt32Array";
        //    else if (type == typeof(int[]) || type == typeof(List<int>) || type == typeof(IList<int>))
        //        return "ReadInt32Array";
        //    else if (type == typeof(uint[]) || type == typeof(List<uint>) || type == typeof(IList<uint>))
        //        return "ReadInt32Array";
        //    else if (type == typeof(float[]) || type == typeof(List<float>) || type == typeof(IList<float>))
        //        return "ReadInt32Array";
        //    else if (type == typeof(double[]) || type == typeof(List<double>) || type == typeof(IList<double>))
        //        return "ReadInt32Array";
        //    else if (type == typeof(decimal[]) || type == typeof(List<decimal>) || type == typeof(IList<decimal>))
        //        return "ReadInt32Array";
        //    else if (type == typeof(DateTime[]) || type == typeof(List<DateTime>) || type == typeof(IList<DateTime>))
        //        return "ReadInt32Array";
        //    else if (type == typeof(TimeSpan[]) || type == typeof(List<TimeSpan>) || type == typeof(IList<TimeSpan>))
        //        return "ReadInt32Array";
        //    else if (type == typeof(Enum[]) || type == typeof(List<Enum>) || type == typeof(IList<Enum>))
        //        return "ReadInt32Array";
        //    else if (type == typeof(Guid[]) || type == typeof(List<Guid>) || type == typeof(IList<Guid>))
        //        return "ReadInt32Array";

        //    else if (type is object)
        //        return "ReadObject";
        //    else
        //        throw new Exception("类型无法解析到指定的方法");

        //}

        //private static MethodInfo GetWriterMethod<TStream>(Type type)
        //{
        //    MethodInfo brWrite = null;

        //    if (type == typeof(DateTime))
        //        brWrite = typeof(TStream).GetMethod("Write", new Type[] { typeof(Int64) });
        //    else if (type == typeof(IDictionary))//currenly supports a string dictionary only
        //        brWrite = typeof(TStream).GetMethod("Write", new Type[] { typeof(string) });
        //    else if (type == typeof(IList))//currenly supports a string dictionary only
        //        brWrite = typeof(TStream).GetMethod("Write", new Type[] { typeof(string) });
        //    //else if (type == typeof(List<int>))
        //    //    brWrite = typeof(TStream).GetMethod("Write", new Type[] { typeof(List<int>) });
        //    //brWrite = typeof(TStream).GetMethod("Write", new Type[] { typeof(int[]) });
        //    //else if (type == typeof(IList<int>))
        //    //    brWrite = typeof(TStream).GetMethod("Write", new Type[] { typeof(int[]) });
        //    else if (type == typeof(List<object>))
        //        brWrite = typeof(TStream).GetMethod("Write", new Type[] { typeof(object[]) });
        //    else if (type == typeof(IEnumerable))
        //        brWrite = typeof(TStream).GetMethod("Write", new Type[] { typeof(IEnumerable) });
        //    else
        //        brWrite = typeof(TStream).GetMethod("Write", new Type[] { type });

        //    return brWrite;
        //}

        //internal static Type GenerateSerializationSurrogateType<T, TStream>(Type type)
        //{
        //    #region IL Initilization Code

        //    Type retType = null;

        //    try
        //    {
        //        //AppDomain domain = Thread.GetDomain();
        //        //AssemblyName asmName = new AssemblyName();
        //        //asmName.Name = "Surrogate";
        //        //AssemblyBuilder asmBuilder = domain.DefineDynamicAssembly(
        //        //                                                   asmName,
        //        //                                                   AssemblyBuilderAccess.Run);

        //        //ModuleBuilder surrogateModule = asmBuilder.DefineDynamicModule("SurrogateModule");
        //        ////ModuleBuilder surrogateModule = myAsmBuilder.DefineDynamicModule( "SurrogateModule", "Surrogate.dll");

        //        //TypeBuilder typeBuilder = surrogateModule.DefineType(type.Name + "_EventSurrogate",
        //        //                                                    TypeAttributes.Public);
        //        //typeBuilder.AddInterfaceImplementation(typeof(T));

        //        ////TypeBuilder eventTypeBuilder = surrogateModule.DefineType(EventType.Name, TypeAttributes.Public);
        //        ////TypeBuilder eventTypeBuilder = BuildTypeHierarchy(surrogateModule, EventType);

        //        AppDomain domain = Thread.GetDomain();
        //        AssemblyName asmName = new AssemblyName();
        //        asmName.Name = typeof(TStream).Name + "Surrogate";
        //        AssemblyBuilder asmBuilder = domain.DefineDynamicAssembly(
        //                                                           asmName,
        //                                                           AssemblyBuilderAccess.Run);

        //        ModuleBuilder surrogateModule = asmBuilder.DefineDynamicModule(typeof(TStream).Name + "SurrogateModule");
        //        //ModuleBuilder surrogateModule = myAsmBuilder.DefineDynamicModule( "SurrogateModule", "Surrogate.dll");

        //        TypeBuilder typeBuilder = surrogateModule.DefineType(typeof(TStream).Name + "_" + type.Name + "_EventSurrogate",
        //                                                            TypeAttributes.Public);
        //        typeBuilder.AddInterfaceImplementation(typeof(T));

        //        //TypeBuilder eventTypeBuilder = surrogateModule.DefineType(EventType.Name, TypeAttributes.Public);
        //        //TypeBuilder eventTypeBuilder = BuildTypeHierarchy(surrogateModule, EventType);

        //    #endregion

        //        #region Build Type Handle Property

        //        //FieldBuilder typeHandleFldBuilder = typeBuilder.DefineField("_typeHandle", typeof(int), FieldAttributes.Private);

        //        //// Define the 'get_TypeHandle' method.
        //        //MethodBuilder getTypeHandleMethod = typeBuilder.DefineMethod("GetTypeHandle",
        //        //   MethodAttributes.Public | MethodAttributes.Virtual,
        //        //   typeof(int), null);

        //        //// Generate IL code for 'get_TypeHandle' method.
        //        //ILGenerator methodIL = getTypeHandleMethod.GetILGenerator();
        //        //methodIL.Emit(OpCodes.Ldarg_0);
        //        //methodIL.Emit(OpCodes.Ldfld, typeHandleFldBuilder);
        //        //methodIL.Emit(OpCodes.Ret);
        //        ////typeHandlePropertyBuilder.SetGetMethod(getTypeHandleMethod);

        //        //// Define the set_TypeHandle method.
        //        //Type[] methodArgs = { typeof(int) };
        //        //MethodBuilder setTypeHandleMethod = typeBuilder.DefineMethod("SetTypeHandle",
        //        //   MethodAttributes.Public | MethodAttributes.Virtual,
        //        //   typeof(void), methodArgs);
        //        //// Generate IL code for set_TypeHandle method.
        //        //methodIL = setTypeHandleMethod.GetILGenerator();
        //        //methodIL.Emit(OpCodes.Ldarg_0);
        //        //methodIL.Emit(OpCodes.Ldarg_1);
        //        //methodIL.Emit(OpCodes.Stfld, typeHandleFldBuilder);
        //        //methodIL.Emit(OpCodes.Ret);
        //        ////typeHandlePropertyBuilder.SetSetMethod(setTypeHandleMethod);


        //        #endregion

        //        #region Serialize Method Builder

        //        Type[] dpParams = new Type[] { typeof(TStream), typeof(object) };
        //        MethodBuilder serializeMethod = typeBuilder.DefineMethod(
        //                                               "Serialize",
        //                                                MethodAttributes.Public | MethodAttributes.Virtual,
        //                                                typeof(void),
        //                                                dpParams);

        //        //MethodInfo dateTimeTicks = typeof(DateTime).GetMethod("get_Ticks");
        //        //MethodInfo int32ArrayList = typeof(List<int>).GetMethod("ToArray");
        //        //MethodInfo objectArrayList = typeof(List<object>).GetMethod("ToArray");
        //        //MethodInfo convertToStringMI = typeof(Convert).GetMethod("ToString", new Type[] { typeof(int) });

        //        ILGenerator mthdIL = serializeMethod.GetILGenerator();
        //        LocalBuilder tpmEvent = mthdIL.DeclareLocal(type);
        //        Label labelFinally = mthdIL.DefineLabel();


        //        mthdIL.Emit(OpCodes.Nop);
        //        mthdIL.Emit(OpCodes.Ldarg_2);//PU
        //        mthdIL.Emit(OpCodes.Castclass, type);//PU
        //        mthdIL.Emit(OpCodes.Stloc, tpmEvent);//PP


        //        //------------
        //        //if (type == typeof(int))
        //        //{
        //        //    MethodInfo brWrite = GetBinaryWriterMethod<TStream>(typeof(int));
        //        //    mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU
        //        //}
        //        //------------


        //        #region old

        //        //Dictionary<string, FieldInfo> fldMap = new Dictionary<string, FieldInfo>();

        //        //foreach (FieldInfo fi in type.GetFields())
        //        //{
        //        //    if (fi.FieldType == typeof(string[]))
        //        //        continue;
        //        //    if (IsIgnoreAttribute(fi.GetCustomAttributes(true)))
        //        //        continue;

        //        //    MethodInfo brWrite = GetWriterMethod<TStream>(fi.FieldType);

        //        //    //FieldBuilder fld = eventTypeBuilder.DefineField(fi.Name, fi.FieldType, fi.Attributes);
        //        //    //TypeBuilder bb = declaringTypeMap[fi.DeclaringType.FullName];
        //        //    //FieldInfo fld = null;
        //        //    FieldInfo fld = fi;// bb.GetField(fi.Name);


        //        //    fldMap[fi.Name] = fld;

        //        //    mthdIL.Emit(OpCodes.Ldarg_1);//PU binary writer
        //        //    mthdIL.Emit(OpCodes.Ldloc, tpmEvent);//PU binary writer

        //        //    #region OPCodes for DateTime serialization

        //        //    if (fi.FieldType == typeof(DateTime))
        //        //    {
        //        //        mthdIL.Emit(OpCodes.Ldflda, fld);
        //        //        mthdIL.EmitCall(OpCodes.Call, dateTimeTicks, null);//PU
        //        //        mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU
        //        //    }

        //        //    #endregion

        //        //    #region  OPCodes for IDictionary serialization

        //        //    else if (fi.FieldType == typeof(IDictionary))
        //        //    {

        //        //        Label loopLabelBegin = mthdIL.DefineLabel();
        //        //        Label loopLabelEnd = mthdIL.DefineLabel();
        //        //        Label endFinally = mthdIL.DefineLabel();

        //        //        LocalBuilder dictionaryEntry = mthdIL.DeclareLocal(typeof(DictionaryEntry));
        //        //        LocalBuilder dicEnumerator = mthdIL.DeclareLocal(typeof(IDictionaryEnumerator));
        //        //        LocalBuilder comparsionResult = mthdIL.DeclareLocal(typeof(bool));
        //        //        LocalBuilder locIDisposable = mthdIL.DeclareLocal(typeof(IDisposable));

        //        //        MethodInfo toString = typeof(object).GetMethod("ToString", new Type[0]);
        //        //        MethodInfo getEnumerator = typeof(IDictionary).GetMethod("GetEnumerator", new Type[0]);
        //        //        MethodInfo moveNext = typeof(IEnumerator).GetMethod("MoveNext", new Type[0]);
        //        //        MethodInfo getCurrent = typeof(IEnumerator).GetMethod("get_Current", new Type[0]);
        //        //        MethodInfo dispose = typeof(IDisposable).GetMethod("Dispose", new Type[0]);
        //        //        MethodInfo get_Key = typeof(DictionaryEntry).GetMethod("get_Key", new Type[0]);
        //        //        MethodInfo get_Value = typeof(DictionaryEntry).GetMethod("get_Value", new Type[0]);
        //        //        MethodInfo get_Count = typeof(ICollection).GetMethod("get_Count");
        //        //        MethodInfo brWriteInt = GetWriterMethod<TStream>(typeof(int));

        //        //        mthdIL.Emit(OpCodes.Ldfld, fld);
        //        //        mthdIL.EmitCall(OpCodes.Callvirt, get_Count, null);// get the array count
        //        //        mthdIL.EmitCall(OpCodes.Callvirt, brWriteInt, null);// write the count
        //        //        mthdIL.Emit(OpCodes.Nop);
        //        //        mthdIL.Emit(OpCodes.Nop);


        //        //        mthdIL.Emit(OpCodes.Ldloc, tpmEvent);//PU load the event object
        //        //        //mthdIL.EmitCall(OpCodes.Callvirt, mi, null);//PU load  the  proprety again  into ES
        //        //        mthdIL.Emit(OpCodes.Ldfld, fld);
        //        //        mthdIL.EmitCall(OpCodes.Callvirt, getEnumerator, null);// get the enumerator
        //        //        mthdIL.Emit(OpCodes.Stloc, dicEnumerator);//save the enumerator

        //        //        mthdIL.BeginExceptionBlock();

        //        //        mthdIL.Emit(OpCodes.Br, loopLabelEnd);// start the loop

        //        //        mthdIL.MarkLabel(loopLabelBegin);//begin for each loop

        //        //        mthdIL.Emit(OpCodes.Ldloc, dicEnumerator);//PU load the enumerator
        //        //        mthdIL.EmitCall(OpCodes.Callvirt, getCurrent, null);// call get_Current
        //        //        mthdIL.Emit(OpCodes.Unbox_Any, typeof(DictionaryEntry));
        //        //        mthdIL.Emit(OpCodes.Stloc, dictionaryEntry);// save the DictionaryEntry

        //        //        //get key
        //        //        mthdIL.Emit(OpCodes.Nop);
        //        //        mthdIL.Emit(OpCodes.Ldarg_1);//PU binary writer
        //        //        mthdIL.Emit(OpCodes.Ldloca_S, dictionaryEntry);//load the DictionaryEntry
        //        //        mthdIL.EmitCall(OpCodes.Call, get_Key, null);// call get_Key
        //        //        mthdIL.EmitCall(OpCodes.Callvirt, toString, null);// call toString
        //        //        mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU call binary writer write

        //        //        //get value
        //        //        mthdIL.Emit(OpCodes.Nop);
        //        //        mthdIL.Emit(OpCodes.Ldarg_1);//PU binary writer
        //        //        mthdIL.Emit(OpCodes.Ldloca_S, dictionaryEntry);//load the DictionaryEntry
        //        //        mthdIL.EmitCall(OpCodes.Call, get_Value, null);// call get_Value
        //        //        mthdIL.EmitCall(OpCodes.Callvirt, toString, null);// call toString
        //        //        mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU call binary writer write

        //        //        mthdIL.Emit(OpCodes.Nop);
        //        //        mthdIL.Emit(OpCodes.Nop);

        //        //        mthdIL.MarkLabel(loopLabelEnd);//end for each loop
        //        //        mthdIL.Emit(OpCodes.Ldloc, dicEnumerator);//PU load the enumerator
        //        //        mthdIL.EmitCall(OpCodes.Callvirt, moveNext, null);// call move next
        //        //        mthdIL.Emit(OpCodes.Stloc, comparsionResult);//save the result
        //        //        mthdIL.Emit(OpCodes.Ldloc, comparsionResult);//load the result
        //        //        mthdIL.Emit(OpCodes.Brtrue_S, loopLabelBegin);//loop if true
        //        //        //mthdIL.Emit(OpCodes.Leave_S, labelFinally);//leave if false

        //        //        mthdIL.BeginFinallyBlock();

        //        //        mthdIL.Emit(OpCodes.Ldloc, dicEnumerator);//PU load the enumerator
        //        //        mthdIL.Emit(OpCodes.Isinst, typeof(System.IDisposable));
        //        //        mthdIL.Emit(OpCodes.Stloc_S, locIDisposable);
        //        //        mthdIL.Emit(OpCodes.Ldloc_S, locIDisposable);
        //        //        mthdIL.Emit(OpCodes.Ldnull);
        //        //        mthdIL.Emit(OpCodes.Ceq);
        //        //        mthdIL.Emit(OpCodes.Stloc, comparsionResult);
        //        //        mthdIL.Emit(OpCodes.Ldloc, comparsionResult);
        //        //        mthdIL.Emit(OpCodes.Brtrue_S, endFinally);
        //        //        mthdIL.Emit(OpCodes.Ldloc_S, locIDisposable);//load IDisposable
        //        //        mthdIL.EmitCall(OpCodes.Callvirt, dispose, null);// call IDisposable::Dispose
        //        //        mthdIL.Emit(OpCodes.Nop);

        //        //        mthdIL.MarkLabel(endFinally);
        //        //        mthdIL.EndExceptionBlock();

        //        //    }

        //        //    #endregion

        //        //    #region List

        //        //    //else if (fi.FieldType == typeof(int[]))
        //        //    //{
        //        //    //    LocalBuilder tmpTicks = mthdIL.DeclareLocal(typeof(int[]));
        //        //    //    mthdIL.Emit(OpCodes.Stloc, tmpTicks);
        //        //    //    mthdIL.Emit(OpCodes.Ldloca_S, tmpTicks);
        //        //    //    mthdIL.EmitCall(OpCodes.Call, int32ArrayList, null);//PU
        //        //    //    mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU
        //        //    //}

        //        //    #endregion

        //        //    #region OPCodes for other types serialization
        //        //    else
        //        //    {
        //        //        mthdIL.Emit(OpCodes.Ldfld, fld);
        //        //        mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU
        //        //    }
        //        //    #endregion

        //        //    mthdIL.Emit(OpCodes.Nop);
        //        //}

        //        //foreach (PropertyInfo pi in type.GetProperties())
        //        //{
        //        //    if (pi.PropertyType == typeof(string[]))
        //        //        continue;
        //        //    if (IsIgnoreAttribute(pi.GetCustomAttributes(true)))
        //        //        continue;

        //        //    MethodInfo mi = type.GetMethod("get_" + pi.Name);
        //        //    MethodInfo brWrite = GetWriterMethod<TStream>(pi.PropertyType);

        //        //    mthdIL.Emit(OpCodes.Ldarg_1);//PU binary writer
        //        //    mthdIL.Emit(OpCodes.Ldloc, tpmEvent);//PU load the event object
        //        //    mthdIL.EmitCall(OpCodes.Callvirt, mi, null);//PU get the value of the proprty

        //        //    #region OPCodes for DateTime serialization

        //        //    if (pi.PropertyType == typeof(DateTime))
        //        //    {
        //        //        LocalBuilder tmpTicks = mthdIL.DeclareLocal(typeof(DateTime));
        //        //        mthdIL.Emit(OpCodes.Stloc, tmpTicks);
        //        //        mthdIL.Emit(OpCodes.Ldloca_S, tmpTicks);
        //        //        mthdIL.EmitCall(OpCodes.Call, dateTimeTicks, null);//PU
        //        //        mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU
        //        //    }
        //        //    #endregion

        //        //    #region OPCodes for IDictionary serialization

        //        //    else if (pi.PropertyType == typeof(IDictionary))
        //        //    {

        //        //        Label loopLabelBegin = mthdIL.DefineLabel();
        //        //        Label loopLabelEnd = mthdIL.DefineLabel();
        //        //        Label endFinally = mthdIL.DefineLabel();

        //        //        LocalBuilder dictionaryEntry = mthdIL.DeclareLocal(typeof(DictionaryEntry));
        //        //        LocalBuilder dicEnumerator = mthdIL.DeclareLocal(typeof(IDictionaryEnumerator));
        //        //        LocalBuilder comparsionResult = mthdIL.DeclareLocal(typeof(bool));
        //        //        LocalBuilder locIDisposable = mthdIL.DeclareLocal(typeof(IDisposable));

        //        //        MethodInfo toString = typeof(object).GetMethod("ToString", new Type[0]);
        //        //        MethodInfo getEnumerator = typeof(IDictionary).GetMethod("GetEnumerator", new Type[0]);
        //        //        MethodInfo moveNext = typeof(IEnumerator).GetMethod("MoveNext", new Type[0]);
        //        //        MethodInfo getCurrent = typeof(IEnumerator).GetMethod("get_Current", new Type[0]);
        //        //        MethodInfo dispose = typeof(IDisposable).GetMethod("Dispose", new Type[0]);
        //        //        MethodInfo get_Key = typeof(DictionaryEntry).GetMethod("get_Key", new Type[0]);
        //        //        MethodInfo get_Value = typeof(DictionaryEntry).GetMethod("get_Value", new Type[0]);
        //        //        MethodInfo get_Count = typeof(ICollection).GetMethod("get_Count");
        //        //        MethodInfo brWriteInt = GetWriterMethod<TStream>(typeof(int));

        //        //        mthdIL.EmitCall(OpCodes.Callvirt, get_Count, null);// get the array count
        //        //        mthdIL.EmitCall(OpCodes.Callvirt, brWriteInt, null);// write the count
        //        //        mthdIL.Emit(OpCodes.Nop);
        //        //        mthdIL.Emit(OpCodes.Nop);


        //        //        mthdIL.Emit(OpCodes.Ldloc, tpmEvent);//PU load the event object
        //        //        mthdIL.EmitCall(OpCodes.Callvirt, mi, null);//PU load  the  proprety again  into ES
        //        //        mthdIL.EmitCall(OpCodes.Callvirt, getEnumerator, null);// get the enumerator
        //        //        mthdIL.Emit(OpCodes.Stloc, dicEnumerator);//save the enumerator

        //        //        mthdIL.BeginExceptionBlock();

        //        //        mthdIL.Emit(OpCodes.Br, loopLabelEnd);// start the loop

        //        //        mthdIL.MarkLabel(loopLabelBegin);//begin for each loop

        //        //        mthdIL.Emit(OpCodes.Ldloc, dicEnumerator);//PU load the enumerator
        //        //        mthdIL.EmitCall(OpCodes.Callvirt, getCurrent, null);// call get_Current
        //        //        mthdIL.Emit(OpCodes.Unbox_Any, typeof(DictionaryEntry));
        //        //        mthdIL.Emit(OpCodes.Stloc, dictionaryEntry);// save the DictionaryEntry

        //        //        //get key
        //        //        mthdIL.Emit(OpCodes.Nop);
        //        //        mthdIL.Emit(OpCodes.Ldarg_1);//PU binary writer
        //        //        mthdIL.Emit(OpCodes.Ldloca_S, dictionaryEntry);//load the DictionaryEntry
        //        //        mthdIL.EmitCall(OpCodes.Call, get_Key, null);// call get_Key
        //        //        mthdIL.EmitCall(OpCodes.Callvirt, toString, null);// call toString
        //        //        mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU call binary writer write

        //        //        //get value
        //        //        mthdIL.Emit(OpCodes.Nop);
        //        //        mthdIL.Emit(OpCodes.Ldarg_1);//PU binary writer
        //        //        mthdIL.Emit(OpCodes.Ldloca_S, dictionaryEntry);//load the DictionaryEntry
        //        //        mthdIL.EmitCall(OpCodes.Call, get_Value, null);// call get_Value
        //        //        mthdIL.EmitCall(OpCodes.Callvirt, toString, null);// call toString
        //        //        mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU call binary writer write

        //        //        mthdIL.Emit(OpCodes.Nop);
        //        //        mthdIL.Emit(OpCodes.Nop);

        //        //        mthdIL.MarkLabel(loopLabelEnd);//end for each loop
        //        //        mthdIL.Emit(OpCodes.Ldloc, dicEnumerator);//PU load the enumerator
        //        //        mthdIL.EmitCall(OpCodes.Callvirt, moveNext, null);// call move next
        //        //        mthdIL.Emit(OpCodes.Stloc, comparsionResult);//save the result
        //        //        mthdIL.Emit(OpCodes.Ldloc, comparsionResult);//load the result
        //        //        mthdIL.Emit(OpCodes.Brtrue_S, loopLabelBegin);//loop if true
        //        //        //mthdIL.Emit(OpCodes.Leave_S, labelFinally);//leave if false

        //        //        mthdIL.BeginFinallyBlock();

        //        //        mthdIL.Emit(OpCodes.Ldloc, dicEnumerator);//PU load the enumerator
        //        //        mthdIL.Emit(OpCodes.Isinst, typeof(System.IDisposable));
        //        //        mthdIL.Emit(OpCodes.Stloc_S, locIDisposable);
        //        //        mthdIL.Emit(OpCodes.Ldloc_S, locIDisposable);
        //        //        mthdIL.Emit(OpCodes.Ldnull);
        //        //        mthdIL.Emit(OpCodes.Ceq);
        //        //        mthdIL.Emit(OpCodes.Stloc, comparsionResult);
        //        //        mthdIL.Emit(OpCodes.Ldloc, comparsionResult);
        //        //        mthdIL.Emit(OpCodes.Brtrue_S, endFinally);
        //        //        mthdIL.Emit(OpCodes.Ldloc_S, locIDisposable);//load IDisposable
        //        //        mthdIL.EmitCall(OpCodes.Callvirt, dispose, null);// call IDisposable::Dispose
        //        //        mthdIL.Emit(OpCodes.Nop);

        //        //        mthdIL.MarkLabel(endFinally);
        //        //        mthdIL.EndExceptionBlock();

        //        //    }

        //        //    #endregion

        //        //    #region List

        //        //    //else if (pi.PropertyType == typeof(List<int>))
        //        //    //{
        //        //    //    //LocalBuilder tmpTicks = mthdIL.DeclareLocal(typeof(List<int>));
        //        //    //    //mthdIL.Emit(OpCodes.Stloc, tmpTicks);
        //        //    //    //mthdIL.Emit(OpCodes.Ldloca_S, tmpTicks);
        //        //    //    //mthdIL.EmitCall(OpCodes.Callvirt, int32ArrayList, null);//PU
        //        //    //    //mthdIL.Emit(OpCodes.Stloc_2, tmpTicks);
        //        //    //    //mthdIL.Emit(OpCodes.Ldloc_0, tmpTicks);
        //        //    //    //mthdIL.Emit(OpCodes.Ldloc_2, tmpTicks);
        //        //    //    //mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU

        //        //    //    //LocalBuilder tmpTicks = mthdIL.DeclareLocal(typeof(List<int>));
        //        //    //    //mthdIL.Emit(OpCodes.Stloc, tmpTicks);
        //        //    //    //mthdIL.Emit(OpCodes.Ldloca_S, tmpTicks);

        //        //    //    //mthdIL.Emit(OpCodes.Ldloc_1, tpmEvent);
        //        //    //    //mthdIL.EmitCall(OpCodes.Callvirt, int32ArrayList, null);//PU
        //        //    //    //mthdIL.Emit(OpCodes.Stloc_2, tpmEvent);
        //        //    //    //mthdIL.Emit(OpCodes.Ldloc_0, tpmEvent);
        //        //    //    //mthdIL.Emit(OpCodes.Ldloc_2, tpmEvent);
        //        //    //    //mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU

        //        //    //    LocalBuilder tmpArray = mthdIL.DeclareLocal(typeof(int[]));
        //        //    //    mthdIL.Emit(OpCodes.Ldnull, tmpArray);
        //        //    //    mthdIL.Emit(OpCodes.Stloc, tmpArray);
        //        //    //    //mthdIL.Emit(OpCodes.Stloc_2, tpmEvent);
        //        //    //    //mthdIL.Emit(OpCodes.Stloc_1, tpmEvent);
        //        //    //    mthdIL.EmitCall(OpCodes.Callvirt, int32ArrayList, null);//PU
        //        //    //    //mthdIL.Emit(OpCodes.Stloc_2, tpmEvent);
        //        //    //    //mthdIL.Emit(OpCodes.Ldloc_0, tpmEvent);
        //        //    //    //mthdIL.Emit(OpCodes.Ldloc_2, tpmEvent);
        //        //    //    mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU
        //        //    //}
        //        //    //else if (pi.PropertyType == typeof(IList<int>))
        //        //    //{
        //        //    //    LocalBuilder tmpArray = mthdIL.DeclareLocal(typeof(int[]));
        //        //    //    mthdIL.Emit(OpCodes.Ldnull, tmpArray);
        //        //    //    mthdIL.Emit(OpCodes.Stloc, tmpArray);
        //        //    //    //mthdIL.Emit(OpCodes.Stloc_2, tpmEvent);
        //        //    //    //mthdIL.Emit(OpCodes.Stloc_1, tpmEvent);
        //        //    //    mthdIL.EmitCall(OpCodes.Callvirt, int32ArrayList, null);//PU
        //        //    //    //mthdIL.Emit(OpCodes.Stloc_2, tpmEvent);
        //        //    //    //mthdIL.Emit(OpCodes.Ldloc_0, tpmEvent);
        //        //    //    //mthdIL.Emit(OpCodes.Ldloc_2, tpmEvent);
        //        //    //    mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU
        //        //    //}
        //        //    ////else if (pi.PropertyType.GetGenericArguments().Length > 0)
        //        //    else if (pi.PropertyType.GetInterface("IEnumerable") == typeof(IEnumerable))
        //        //    {
        //        //        bool isExplicit = false;
        //        //        Type t = pi.PropertyType;
        //        //        //可能是List及相关
        //        //        if (pi.PropertyType.GetGenericArguments().Length == 1)
        //        //        {
        //        //            if (t == typeof(List<byte>) || t == typeof(IList<byte>) ||
        //        //                t == typeof(List<sbyte>) || t == typeof(IList<sbyte>) ||
        //        //                t == typeof(List<short>) || t == typeof(IList<short>) ||
        //        //                t == typeof(List<ushort>) || t == typeof(IList<ushort>) ||
        //        //                t == typeof(List<int>) || t == typeof(IList<int>) ||
        //        //                t == typeof(List<uint>) || t == typeof(IList<uint>) ||
        //        //                t == typeof(List<long>) || t == typeof(IList<long>) ||
        //        //                t == typeof(List<ulong>) || t == typeof(IList<ulong>) ||
        //        //                t == typeof(List<float>) || t == typeof(IList<float>) ||
        //        //                t == typeof(List<double>) || t == typeof(IList<double>) ||
        //        //                t == typeof(List<decimal>) || t == typeof(IList<decimal>)
        //        //                )
        //        //            {
        //        //                isExplicit = true;
        //        //            }
        //        //        }
        //        //        //可能是Dictionary及相关
        //        //        else if (pi.PropertyType.GetGenericArguments().Length == 2)
        //        //        {

        //        //        }
        //        //        else
        //        //        {
        //        //            if (t == typeof(string))
        //        //                isExplicit = true;
        //        //        }
        //        //        if (isExplicit == true)
        //        //            mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU
        //        //        else
        //        //        {
        //        //            LocalBuilder tmpArray = mthdIL.DeclareLocal(typeof(object[]));
        //        //            mthdIL.Emit(OpCodes.Ldnull, tmpArray);
        //        //            mthdIL.Emit(OpCodes.Stloc, tmpArray);
        //        //            //mthdIL.Emit(OpCodes.Stloc_2, tpmEvent);
        //        //            //mthdIL.Emit(OpCodes.Stloc_1, tpmEvent);
        //        //            mthdIL.EmitCall(OpCodes.Callvirt, objectArrayList, null);//PU
        //        //            //mthdIL.Emit(OpCodes.Stloc_2, tpmEvent);
        //        //            //mthdIL.Emit(OpCodes.Ldloc_0, tpmEvent);
        //        //            //mthdIL.Emit(OpCodes.Ldloc_2, tpmEvent);
        //        //            mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU
        //        //        }
        //        //    }

        //        //    #endregion

        //        //    #region OPCodes for all other type serialization

        //        //    else
        //        //        mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU

        //        //    #endregion

        //        //    mthdIL.Emit(OpCodes.Nop);
        //        //}

        //        #endregion




        //        SerializeFields<TStream>(type, mthdIL, tpmEvent);
        //        SerializePropertys<TStream>(type, mthdIL, tpmEvent);

        //        mthdIL.MarkLabel(labelFinally);
        //        mthdIL.Emit(OpCodes.Ret);
        //        mthdIL = null;

        //        #endregion

        //        #region Deserialize Method Builder

        //        dpParams = new Type[] { typeof(TStream) };
        //        MethodBuilder deserializeMthd = typeBuilder.DefineMethod(
        //                                               "DeSerialize",
        //                                                MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.Final | MethodAttributes.NewSlot,
        //                                                typeof(object),
        //                                                dpParams);

        //        ILGenerator deserializeIL = deserializeMthd.GetILGenerator();
        //        LocalBuilder tpmRetEvent = deserializeIL.DeclareLocal(type);
        //        LocalBuilder tpmRetEvent2 = deserializeIL.DeclareLocal(type);

        //        //MethodInfo readString = typeof(ObjectStream).GetMethod("ReadString");
        //        //MethodInfo readInt = typeof(ObjectStream).GetMethod("ReadInt32");



        //        Label ret = deserializeIL.DefineLabel();

        //        ConstructorInfo ctorEvent = type.GetConstructor(new Type[0]);
        //        if (ctorEvent == null)
        //            throw new Exception("The event class is missing a default constructor with 0 params");

        //        deserializeIL.Emit(OpCodes.Newobj, ctorEvent);
        //        deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent);


        //        DeserializeFields<TStream>(type, deserializeIL, tpmRetEvent, tpmRetEvent2);
        //        DeserializePropertys<TStream>(type, deserializeIL, tpmRetEvent, tpmRetEvent2);

        //        deserializeIL.Emit(OpCodes.Br_S, ret);
        //        deserializeIL.MarkLabel(ret);
        //        deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent2);
        //        deserializeIL.Emit(OpCodes.Ret);

        //        #endregion

        //        retType = typeBuilder.CreateType();

        //        #region old
        //        //是否保存编译好的程序集，不是太好判断，暂时不用
        //        //typeBuilder.CreateType();
        //        //foreach (KeyValuePair<string, TypeBuilder> d in declaringTypeMap)
        //        //{
        //        //    d.Value.CreateType();
        //        //}
        //        //asmBuilder.Save(asmBuilder.GetName().Name + ".dll");
        //        #endregion
        //    }
        //    catch (Exception x)
        //    {
        //        throw x;
        //    }
        //    return retType;
        //}

        //private static void SerializePropertys<TStream>(Type type, ILGenerator mthdIL, LocalBuilder tpmEvent)
        //{
        //    MethodInfo dateTimeTicks = typeof(DateTime).GetMethod("get_Ticks");
        //    MethodInfo objectArrayList = typeof(List<object>).GetMethod("ToArray");
        //    foreach (PropertyInfo pi in type.GetProperties())
        //    {
        //        if (pi.PropertyType == typeof(string[]))
        //            continue;
        //        if (IsIgnoreAttribute(pi.GetCustomAttributes(true)))
        //            continue;

        //        MethodInfo mi = type.GetMethod("get_" + pi.Name);
        //        MethodInfo brWrite = GetWriterMethod<TStream>(pi.PropertyType);

        //        mthdIL.Emit(OpCodes.Ldarg_1);//PU binary writer
        //        mthdIL.Emit(OpCodes.Ldloc, tpmEvent);//PU load the event object
        //        mthdIL.EmitCall(OpCodes.Callvirt, mi, null);//PU get the value of the proprty

        //        #region OPCodes for DateTime serialization

        //        if (pi.PropertyType == typeof(DateTime))
        //        {
        //            LocalBuilder tmpTicks = mthdIL.DeclareLocal(typeof(DateTime));
        //            mthdIL.Emit(OpCodes.Stloc, tmpTicks);
        //            mthdIL.Emit(OpCodes.Ldloca_S, tmpTicks);
        //            mthdIL.EmitCall(OpCodes.Call, dateTimeTicks, null);//PU
        //            mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU
        //        }
        //        #endregion

        //        #region OPCodes for IDictionary serialization

        //        else if (pi.PropertyType == typeof(IDictionary))
        //        {

        //            Label loopLabelBegin = mthdIL.DefineLabel();
        //            Label loopLabelEnd = mthdIL.DefineLabel();
        //            Label endFinally = mthdIL.DefineLabel();

        //            LocalBuilder dictionaryEntry = mthdIL.DeclareLocal(typeof(DictionaryEntry));
        //            LocalBuilder dicEnumerator = mthdIL.DeclareLocal(typeof(IDictionaryEnumerator));
        //            LocalBuilder comparsionResult = mthdIL.DeclareLocal(typeof(bool));
        //            LocalBuilder locIDisposable = mthdIL.DeclareLocal(typeof(IDisposable));

        //            MethodInfo toString = typeof(object).GetMethod("ToString", new Type[0]);
        //            MethodInfo getEnumerator = typeof(IDictionary).GetMethod("GetEnumerator", new Type[0]);
        //            MethodInfo moveNext = typeof(IEnumerator).GetMethod("MoveNext", new Type[0]);
        //            MethodInfo getCurrent = typeof(IEnumerator).GetMethod("get_Current", new Type[0]);
        //            MethodInfo dispose = typeof(IDisposable).GetMethod("Dispose", new Type[0]);
        //            MethodInfo get_Key = typeof(DictionaryEntry).GetMethod("get_Key", new Type[0]);
        //            MethodInfo get_Value = typeof(DictionaryEntry).GetMethod("get_Value", new Type[0]);
        //            MethodInfo get_Count = typeof(ICollection).GetMethod("get_Count");
        //            MethodInfo brWriteInt = GetWriterMethod<TStream>(typeof(int));

        //            mthdIL.EmitCall(OpCodes.Callvirt, get_Count, null);// get the array count
        //            mthdIL.EmitCall(OpCodes.Callvirt, brWriteInt, null);// write the count
        //            mthdIL.Emit(OpCodes.Nop);
        //            mthdIL.Emit(OpCodes.Nop);


        //            mthdIL.Emit(OpCodes.Ldloc, tpmEvent);//PU load the event object
        //            mthdIL.EmitCall(OpCodes.Callvirt, mi, null);//PU load  the  proprety again  into ES
        //            mthdIL.EmitCall(OpCodes.Callvirt, getEnumerator, null);// get the enumerator
        //            mthdIL.Emit(OpCodes.Stloc, dicEnumerator);//save the enumerator

        //            mthdIL.BeginExceptionBlock();

        //            mthdIL.Emit(OpCodes.Br, loopLabelEnd);// start the loop

        //            mthdIL.MarkLabel(loopLabelBegin);//begin for each loop

        //            mthdIL.Emit(OpCodes.Ldloc, dicEnumerator);//PU load the enumerator
        //            mthdIL.EmitCall(OpCodes.Callvirt, getCurrent, null);// call get_Current
        //            mthdIL.Emit(OpCodes.Unbox_Any, typeof(DictionaryEntry));
        //            mthdIL.Emit(OpCodes.Stloc, dictionaryEntry);// save the DictionaryEntry

        //            //get key
        //            mthdIL.Emit(OpCodes.Nop);
        //            mthdIL.Emit(OpCodes.Ldarg_1);//PU binary writer
        //            mthdIL.Emit(OpCodes.Ldloca_S, dictionaryEntry);//load the DictionaryEntry
        //            mthdIL.EmitCall(OpCodes.Call, get_Key, null);// call get_Key
        //            mthdIL.EmitCall(OpCodes.Callvirt, toString, null);// call toString
        //            mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU call binary writer write

        //            //get value
        //            mthdIL.Emit(OpCodes.Nop);
        //            mthdIL.Emit(OpCodes.Ldarg_1);//PU binary writer
        //            mthdIL.Emit(OpCodes.Ldloca_S, dictionaryEntry);//load the DictionaryEntry
        //            mthdIL.EmitCall(OpCodes.Call, get_Value, null);// call get_Value
        //            mthdIL.EmitCall(OpCodes.Callvirt, toString, null);// call toString
        //            mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU call binary writer write

        //            mthdIL.Emit(OpCodes.Nop);
        //            mthdIL.Emit(OpCodes.Nop);

        //            mthdIL.MarkLabel(loopLabelEnd);//end for each loop
        //            mthdIL.Emit(OpCodes.Ldloc, dicEnumerator);//PU load the enumerator
        //            mthdIL.EmitCall(OpCodes.Callvirt, moveNext, null);// call move next
        //            mthdIL.Emit(OpCodes.Stloc, comparsionResult);//save the result
        //            mthdIL.Emit(OpCodes.Ldloc, comparsionResult);//load the result
        //            mthdIL.Emit(OpCodes.Brtrue_S, loopLabelBegin);//loop if true
        //            //mthdIL.Emit(OpCodes.Leave_S, labelFinally);//leave if false

        //            mthdIL.BeginFinallyBlock();

        //            mthdIL.Emit(OpCodes.Ldloc, dicEnumerator);//PU load the enumerator
        //            mthdIL.Emit(OpCodes.Isinst, typeof(System.IDisposable));
        //            mthdIL.Emit(OpCodes.Stloc_S, locIDisposable);
        //            mthdIL.Emit(OpCodes.Ldloc_S, locIDisposable);
        //            mthdIL.Emit(OpCodes.Ldnull);
        //            mthdIL.Emit(OpCodes.Ceq);
        //            mthdIL.Emit(OpCodes.Stloc, comparsionResult);
        //            mthdIL.Emit(OpCodes.Ldloc, comparsionResult);
        //            mthdIL.Emit(OpCodes.Brtrue_S, endFinally);
        //            mthdIL.Emit(OpCodes.Ldloc_S, locIDisposable);//load IDisposable
        //            mthdIL.EmitCall(OpCodes.Callvirt, dispose, null);// call IDisposable::Dispose
        //            mthdIL.Emit(OpCodes.Nop);

        //            mthdIL.MarkLabel(endFinally);
        //            mthdIL.EndExceptionBlock();

        //        }

        //        #endregion

        //        #region List

        //        #region old
        //        //else if (pi.PropertyType == typeof(List<int>))
        //        //{
        //        //    //LocalBuilder tmpTicks = mthdIL.DeclareLocal(typeof(List<int>));
        //        //    //mthdIL.Emit(OpCodes.Stloc, tmpTicks);
        //        //    //mthdIL.Emit(OpCodes.Ldloca_S, tmpTicks);
        //        //    //mthdIL.EmitCall(OpCodes.Callvirt, int32ArrayList, null);//PU
        //        //    //mthdIL.Emit(OpCodes.Stloc_2, tmpTicks);
        //        //    //mthdIL.Emit(OpCodes.Ldloc_0, tmpTicks);
        //        //    //mthdIL.Emit(OpCodes.Ldloc_2, tmpTicks);
        //        //    //mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU

        //        //    //LocalBuilder tmpTicks = mthdIL.DeclareLocal(typeof(List<int>));
        //        //    //mthdIL.Emit(OpCodes.Stloc, tmpTicks);
        //        //    //mthdIL.Emit(OpCodes.Ldloca_S, tmpTicks);

        //        //    //mthdIL.Emit(OpCodes.Ldloc_1, tpmEvent);
        //        //    //mthdIL.EmitCall(OpCodes.Callvirt, int32ArrayList, null);//PU
        //        //    //mthdIL.Emit(OpCodes.Stloc_2, tpmEvent);
        //        //    //mthdIL.Emit(OpCodes.Ldloc_0, tpmEvent);
        //        //    //mthdIL.Emit(OpCodes.Ldloc_2, tpmEvent);
        //        //    //mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU

        //        //    LocalBuilder tmpArray = mthdIL.DeclareLocal(typeof(int[]));
        //        //    mthdIL.Emit(OpCodes.Ldnull, tmpArray);
        //        //    mthdIL.Emit(OpCodes.Stloc, tmpArray);
        //        //    //mthdIL.Emit(OpCodes.Stloc_2, tpmEvent);
        //        //    //mthdIL.Emit(OpCodes.Stloc_1, tpmEvent);
        //        //    mthdIL.EmitCall(OpCodes.Callvirt, int32ArrayList, null);//PU
        //        //    //mthdIL.Emit(OpCodes.Stloc_2, tpmEvent);
        //        //    //mthdIL.Emit(OpCodes.Ldloc_0, tpmEvent);
        //        //    //mthdIL.Emit(OpCodes.Ldloc_2, tpmEvent);
        //        //    mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU
        //        //}
        //        //else if (pi.PropertyType == typeof(IList<int>))
        //        //{
        //        //    LocalBuilder tmpArray = mthdIL.DeclareLocal(typeof(int[]));
        //        //    mthdIL.Emit(OpCodes.Ldnull, tmpArray);
        //        //    mthdIL.Emit(OpCodes.Stloc, tmpArray);
        //        //    //mthdIL.Emit(OpCodes.Stloc_2, tpmEvent);
        //        //    //mthdIL.Emit(OpCodes.Stloc_1, tpmEvent);
        //        //    mthdIL.EmitCall(OpCodes.Callvirt, int32ArrayList, null);//PU
        //        //    //mthdIL.Emit(OpCodes.Stloc_2, tpmEvent);
        //        //    //mthdIL.Emit(OpCodes.Ldloc_0, tpmEvent);
        //        //    //mthdIL.Emit(OpCodes.Ldloc_2, tpmEvent);
        //        //    mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU
        //        //}
        //        ////else if (pi.PropertyType.GetGenericArguments().Length > 0)
        //        #endregion

        //        else if (pi.PropertyType.GetInterface("IEnumerable") == typeof(IEnumerable))
        //        {
        //            bool isExplicit = false;
        //            Type t = pi.PropertyType;
        //            //可能是List及相关
        //            if (pi.PropertyType.GetGenericArguments().Length == 1)
        //            {
        //                if (t == typeof(List<byte>) || t == typeof(IList<byte>) ||
        //                    t == typeof(List<sbyte>) || t == typeof(IList<sbyte>) ||
        //                    t == typeof(List<short>) || t == typeof(IList<short>) ||
        //                    t == typeof(List<ushort>) || t == typeof(IList<ushort>) ||
        //                    t == typeof(List<int>) || t == typeof(IList<int>) ||
        //                    t == typeof(List<uint>) || t == typeof(IList<uint>) ||
        //                    t == typeof(List<long>) || t == typeof(IList<long>) ||
        //                    t == typeof(List<ulong>) || t == typeof(IList<ulong>) ||
        //                    t == typeof(List<float>) || t == typeof(IList<float>) ||
        //                    t == typeof(List<double>) || t == typeof(IList<double>) ||
        //                    t == typeof(List<decimal>) || t == typeof(IList<decimal>)
        //                    )
        //                {
        //                    isExplicit = true;
        //                }
        //            }
        //            //可能是Dictionary及相关
        //            else if (pi.PropertyType.GetGenericArguments().Length == 2)
        //            {

        //            }
        //            else
        //            {
        //                if (t == typeof(string))
        //                    isExplicit = true;
        //            }
        //            if (isExplicit == true)
        //                mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU
        //            else
        //            {
        //                LocalBuilder tmpArray = mthdIL.DeclareLocal(typeof(object[]));
        //                mthdIL.Emit(OpCodes.Ldnull, tmpArray);
        //                mthdIL.Emit(OpCodes.Stloc, tmpArray);
        //                //mthdIL.Emit(OpCodes.Stloc_2, tpmEvent);
        //                //mthdIL.Emit(OpCodes.Stloc_1, tpmEvent);
        //                mthdIL.EmitCall(OpCodes.Callvirt, objectArrayList, null);//PU
        //                //mthdIL.Emit(OpCodes.Stloc_2, tpmEvent);
        //                //mthdIL.Emit(OpCodes.Ldloc_0, tpmEvent);
        //                //mthdIL.Emit(OpCodes.Ldloc_2, tpmEvent);
        //                mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU
        //            }
        //        }

        //        #endregion

        //        #region OPCodes for all other type serialization

        //        else
        //            mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU

        //        #endregion

        //        mthdIL.Emit(OpCodes.Nop);
        //    }
        //}

        //private static void SerializeFields<TStream>(Type type, ILGenerator mthdIL, LocalBuilder tpmEvent)
        //{
        //    Dictionary<string, FieldInfo> fldMap = new Dictionary<string, FieldInfo>();
        //    MethodInfo dateTimeTicks = typeof(DateTime).GetMethod("get_Ticks");
        //    MethodInfo objectArrayList = typeof(List<object>).GetMethod("ToArray");
        //    foreach (FieldInfo fi in type.GetFields())
        //    {
        //        if (fi.FieldType == typeof(string[]))
        //            continue;
        //        if (IsIgnoreAttribute(fi.GetCustomAttributes(true)))
        //            continue;

        //        MethodInfo brWrite = GetWriterMethod<TStream>(fi.FieldType);

        //        //FieldBuilder fld = eventTypeBuilder.DefineField(fi.Name, fi.FieldType, fi.Attributes);
        //        //TypeBuilder bb = declaringTypeMap[fi.DeclaringType.FullName];
        //        //FieldInfo fld = null;
        //        FieldInfo fld = fi;// bb.GetField(fi.Name);


        //        fldMap[fi.Name] = fld;

        //        mthdIL.Emit(OpCodes.Ldarg_1);//PU binary writer
        //        mthdIL.Emit(OpCodes.Ldloc, tpmEvent);//PU binary writer

        //        #region OPCodes for DateTime serialization

        //        if (fi.FieldType == typeof(DateTime))
        //        {
        //            mthdIL.Emit(OpCodes.Ldflda, fld);
        //            mthdIL.EmitCall(OpCodes.Call, dateTimeTicks, null);//PU
        //            mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU
        //        }

        //        #endregion

        //        #region  OPCodes for IDictionary serialization

        //        else if (fi.FieldType == typeof(IDictionary))
        //        {

        //            Label loopLabelBegin = mthdIL.DefineLabel();
        //            Label loopLabelEnd = mthdIL.DefineLabel();
        //            Label endFinally = mthdIL.DefineLabel();

        //            LocalBuilder dictionaryEntry = mthdIL.DeclareLocal(typeof(DictionaryEntry));
        //            LocalBuilder dicEnumerator = mthdIL.DeclareLocal(typeof(IDictionaryEnumerator));
        //            LocalBuilder comparsionResult = mthdIL.DeclareLocal(typeof(bool));
        //            LocalBuilder locIDisposable = mthdIL.DeclareLocal(typeof(IDisposable));

        //            MethodInfo toString = typeof(object).GetMethod("ToString", new Type[0]);
        //            MethodInfo getEnumerator = typeof(IDictionary).GetMethod("GetEnumerator", new Type[0]);
        //            MethodInfo moveNext = typeof(IEnumerator).GetMethod("MoveNext", new Type[0]);
        //            MethodInfo getCurrent = typeof(IEnumerator).GetMethod("get_Current", new Type[0]);
        //            MethodInfo dispose = typeof(IDisposable).GetMethod("Dispose", new Type[0]);
        //            MethodInfo get_Key = typeof(DictionaryEntry).GetMethod("get_Key", new Type[0]);
        //            MethodInfo get_Value = typeof(DictionaryEntry).GetMethod("get_Value", new Type[0]);
        //            MethodInfo get_Count = typeof(ICollection).GetMethod("get_Count");
        //            MethodInfo brWriteInt = GetWriterMethod<TStream>(typeof(int));

        //            mthdIL.Emit(OpCodes.Ldfld, fld);
        //            mthdIL.EmitCall(OpCodes.Callvirt, get_Count, null);// get the array count
        //            mthdIL.EmitCall(OpCodes.Callvirt, brWriteInt, null);// write the count
        //            mthdIL.Emit(OpCodes.Nop);
        //            mthdIL.Emit(OpCodes.Nop);


        //            mthdIL.Emit(OpCodes.Ldloc, tpmEvent);//PU load the event object
        //            //mthdIL.EmitCall(OpCodes.Callvirt, mi, null);//PU load  the  proprety again  into ES
        //            mthdIL.Emit(OpCodes.Ldfld, fld);
        //            mthdIL.EmitCall(OpCodes.Callvirt, getEnumerator, null);// get the enumerator
        //            mthdIL.Emit(OpCodes.Stloc, dicEnumerator);//save the enumerator

        //            mthdIL.BeginExceptionBlock();

        //            mthdIL.Emit(OpCodes.Br, loopLabelEnd);// start the loop

        //            mthdIL.MarkLabel(loopLabelBegin);//begin for each loop

        //            mthdIL.Emit(OpCodes.Ldloc, dicEnumerator);//PU load the enumerator
        //            mthdIL.EmitCall(OpCodes.Callvirt, getCurrent, null);// call get_Current
        //            mthdIL.Emit(OpCodes.Unbox_Any, typeof(DictionaryEntry));
        //            mthdIL.Emit(OpCodes.Stloc, dictionaryEntry);// save the DictionaryEntry

        //            //get key
        //            mthdIL.Emit(OpCodes.Nop);
        //            mthdIL.Emit(OpCodes.Ldarg_1);//PU binary writer
        //            mthdIL.Emit(OpCodes.Ldloca_S, dictionaryEntry);//load the DictionaryEntry
        //            mthdIL.EmitCall(OpCodes.Call, get_Key, null);// call get_Key
        //            mthdIL.EmitCall(OpCodes.Callvirt, toString, null);// call toString
        //            mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU call binary writer write

        //            //get value
        //            mthdIL.Emit(OpCodes.Nop);
        //            mthdIL.Emit(OpCodes.Ldarg_1);//PU binary writer
        //            mthdIL.Emit(OpCodes.Ldloca_S, dictionaryEntry);//load the DictionaryEntry
        //            mthdIL.EmitCall(OpCodes.Call, get_Value, null);// call get_Value
        //            mthdIL.EmitCall(OpCodes.Callvirt, toString, null);// call toString
        //            mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU call binary writer write

        //            mthdIL.Emit(OpCodes.Nop);
        //            mthdIL.Emit(OpCodes.Nop);

        //            mthdIL.MarkLabel(loopLabelEnd);//end for each loop
        //            mthdIL.Emit(OpCodes.Ldloc, dicEnumerator);//PU load the enumerator
        //            mthdIL.EmitCall(OpCodes.Callvirt, moveNext, null);// call move next
        //            mthdIL.Emit(OpCodes.Stloc, comparsionResult);//save the result
        //            mthdIL.Emit(OpCodes.Ldloc, comparsionResult);//load the result
        //            mthdIL.Emit(OpCodes.Brtrue_S, loopLabelBegin);//loop if true
        //            //mthdIL.Emit(OpCodes.Leave_S, labelFinally);//leave if false

        //            mthdIL.BeginFinallyBlock();

        //            mthdIL.Emit(OpCodes.Ldloc, dicEnumerator);//PU load the enumerator
        //            mthdIL.Emit(OpCodes.Isinst, typeof(System.IDisposable));
        //            mthdIL.Emit(OpCodes.Stloc_S, locIDisposable);
        //            mthdIL.Emit(OpCodes.Ldloc_S, locIDisposable);
        //            mthdIL.Emit(OpCodes.Ldnull);
        //            mthdIL.Emit(OpCodes.Ceq);
        //            mthdIL.Emit(OpCodes.Stloc, comparsionResult);
        //            mthdIL.Emit(OpCodes.Ldloc, comparsionResult);
        //            mthdIL.Emit(OpCodes.Brtrue_S, endFinally);
        //            mthdIL.Emit(OpCodes.Ldloc_S, locIDisposable);//load IDisposable
        //            mthdIL.EmitCall(OpCodes.Callvirt, dispose, null);// call IDisposable::Dispose
        //            mthdIL.Emit(OpCodes.Nop);

        //            mthdIL.MarkLabel(endFinally);
        //            mthdIL.EndExceptionBlock();

        //        }

        //        #endregion

        //        #region List

        //        //else if (fi.FieldType == typeof(int[]))
        //        //{
        //        //    LocalBuilder tmpTicks = mthdIL.DeclareLocal(typeof(int[]));
        //        //    mthdIL.Emit(OpCodes.Stloc, tmpTicks);
        //        //    mthdIL.Emit(OpCodes.Ldloca_S, tmpTicks);
        //        //    mthdIL.EmitCall(OpCodes.Call, int32ArrayList, null);//PU
        //        //    mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU
        //        //}

        //        #endregion

        //        #region OPCodes for other types serialization
        //        else
        //        {
        //            mthdIL.Emit(OpCodes.Ldfld, fld);
        //            mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU
        //        }
        //        #endregion

        //        mthdIL.Emit(OpCodes.Nop);
        //    }
        //}

        //private static void DeserializePropertys<TStream>(Type type, ILGenerator deserializeIL, LocalBuilder tpmRetEvent, LocalBuilder tpmRetEvent2)
        //{
        //    foreach (PropertyInfo pi in type.GetProperties())
        //    {
        //        if (pi.PropertyType == typeof(string[]))
        //            continue;
        //        if (IsIgnoreAttribute(pi.GetCustomAttributes(true)))
        //            continue;

        //        MethodInfo brRead = typeof(TStream).GetMethod(GetReaderMethod(pi.PropertyType));
        //        MethodInfo setProp = type.GetMethod("set_" + pi.Name);
        //        MethodInfo getProp = type.GetMethod("get_" + pi.Name);

        //        #region OPCodes for DateTime DeSerialization
        //        if (pi.PropertyType == typeof(DateTime))
        //        {
        //            deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);//load new obj on ES
        //            deserializeIL.Emit(OpCodes.Ldarg_1);//PU binary reader ,load BR on ES
        //            deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);//PU

        //            //似乎没用
        //            //FieldBuilder dateTimeFld = typeBuilder.DefineField("Ticks ", typeof(Int64), FieldAttributes.Public);
        //            ConstructorInfo ctorDtTime = typeof(DateTime).GetConstructor(new Type[] { typeof(Int64) });
        //            deserializeIL.Emit(OpCodes.Newobj, ctorDtTime);//PU
        //            deserializeIL.EmitCall(OpCodes.Callvirt, setProp, null);//PU

        //            deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
        //            deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent2);
        //        }
        //        #endregion

        //        #region OPCodes for IDictionary DeSerialization

        //        else if (pi.PropertyType == typeof(IDictionary))
        //        {
        //            Label loopLabelBegin = deserializeIL.DefineLabel();
        //            Label loopLabelEnd = deserializeIL.DefineLabel();

        //            LocalBuilder count = deserializeIL.DeclareLocal(typeof(Int32));
        //            LocalBuilder key = deserializeIL.DeclareLocal(typeof(string));
        //            LocalBuilder value = deserializeIL.DeclareLocal(typeof(string));
        //            LocalBuilder boolVal = deserializeIL.DeclareLocal(typeof(bool));

        //            MethodInfo toString = typeof(object).GetMethod("ToString", new Type[0]);
        //            MethodInfo dicAdd = typeof(IDictionary).GetMethod("Add", new Type[] { typeof(object), typeof(object) });
        //            MethodInfo brReadInt = typeof(TStream).GetMethod(GetReaderMethod(typeof(int)));

        //            deserializeIL.Emit(OpCodes.Ldarg_1);//PU binary reader ,load BR on ES
        //            deserializeIL.EmitCall(OpCodes.Callvirt, brReadInt, null);//PU
        //            deserializeIL.Emit(OpCodes.Stloc, count);

        //            deserializeIL.Emit(OpCodes.Br, loopLabelEnd);

        //            deserializeIL.MarkLabel(loopLabelBegin); //begin loop 
        //            deserializeIL.Emit(OpCodes.Nop);
        //            deserializeIL.Emit(OpCodes.Ldarg_1);
        //            deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);
        //            deserializeIL.Emit(OpCodes.Stloc, key);
        //            deserializeIL.Emit(OpCodes.Ldarg_1);
        //            deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);
        //            deserializeIL.Emit(OpCodes.Stloc, value);

        //            deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
        //            deserializeIL.EmitCall(OpCodes.Callvirt, getProp, null);
        //            deserializeIL.Emit(OpCodes.Ldloc, key);
        //            deserializeIL.Emit(OpCodes.Ldloc, value);
        //            deserializeIL.EmitCall(OpCodes.Callvirt, dicAdd, null);//call add method
        //            deserializeIL.Emit(OpCodes.Nop);

        //            deserializeIL.Emit(OpCodes.Ldloc, count);
        //            deserializeIL.Emit(OpCodes.Ldc_I4_1);
        //            deserializeIL.Emit(OpCodes.Sub);
        //            deserializeIL.Emit(OpCodes.Stloc, count);

        //            deserializeIL.MarkLabel(loopLabelEnd); //end loop 
        //            deserializeIL.Emit(OpCodes.Nop);
        //            deserializeIL.Emit(OpCodes.Ldloc, count);
        //            deserializeIL.Emit(OpCodes.Ldc_I4_0);
        //            deserializeIL.Emit(OpCodes.Ceq);
        //            deserializeIL.Emit(OpCodes.Ldc_I4_0);
        //            deserializeIL.Emit(OpCodes.Ceq);
        //            deserializeIL.Emit(OpCodes.Stloc_S, boolVal);
        //            deserializeIL.Emit(OpCodes.Ldloc_S, boolVal);
        //            deserializeIL.Emit(OpCodes.Brtrue_S, loopLabelBegin);

        //            deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
        //            deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent2);


        //        }
        //        #endregion

        //        #region List

        //        else if (pi.PropertyType == typeof(List<int>))
        //        {
        //            deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);//load new obj on ES
        //            deserializeIL.Emit(OpCodes.Ldarg_1);//PU binary reader ,load BR on ES
        //            deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);//PU

        //            //FieldBuilder dateTimeFld = typeBuilder.DefineField("Ticks ", typeof(Int64), FieldAttributes.Public);
        //            ConstructorInfo ctorDtTime = typeof(List<int>).GetConstructor(new Type[] { typeof(int[]) });
        //            deserializeIL.Emit(OpCodes.Newobj, ctorDtTime);//PU
        //            deserializeIL.EmitCall(OpCodes.Callvirt, setProp, null);//PU

        //            deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
        //            deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent2);
        //        }
        //        else if (pi.PropertyType == typeof(IList<int>))
        //        {
        //            deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);//load new obj on ES
        //            deserializeIL.Emit(OpCodes.Ldarg_1);//PU binary reader ,load BR on ES
        //            deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);//PU

        //            //FieldBuilder dateTimeFld = typeBuilder.DefineField("Ticks ", typeof(Int64), FieldAttributes.Public);
        //            ConstructorInfo ctorDtTime = typeof(List<int>).GetConstructor(new Type[] { typeof(int[]) });
        //            deserializeIL.Emit(OpCodes.Newobj, ctorDtTime);//PU
        //            deserializeIL.EmitCall(OpCodes.Callvirt, setProp, null);//PU

        //            deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
        //            deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent2);
        //        }

        //        #endregion

        //        #region OPCodes for other types DeSerialization
        //        else
        //        {
        //            deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);//load new obj on ES
        //            deserializeIL.Emit(OpCodes.Ldarg_1);//PU binary reader ,load BR on ES
        //            deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);//PU

        //            deserializeIL.EmitCall(OpCodes.Callvirt, setProp, null);//PU


        //            deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
        //            deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent2);

        //        }
        //        #endregion
        //    }
        //}

        //private static void DeserializeFields<TStream>(Type type, ILGenerator deserializeIL, LocalBuilder tpmRetEvent, LocalBuilder tpmRetEvent2)
        //{
        //    foreach (FieldInfo fi in type.GetFields())
        //    {
        //        if (fi.FieldType == typeof(string[]))
        //            continue;
        //        if (IsIgnoreAttribute(fi.GetCustomAttributes(true)))
        //            continue;

        //        LocalBuilder locTyp = deserializeIL.DeclareLocal(fi.FieldType);
        //        MethodInfo brRead = typeof(TStream).GetMethod(GetReaderMethod(fi.FieldType));
        //        //FieldInfo fld = fldMap[fi.Name];
        //        FieldInfo fld = fi;


        //        #region OPCodes for DateTime DeSerialization

        //        if (fi.FieldType == typeof(DateTime))
        //        {
        //            ConstructorInfo ctorDtTime = typeof(DateTime).GetConstructor(new Type[] { typeof(Int64) });

        //            deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);//load new obj on ES
        //            deserializeIL.Emit(OpCodes.Ldarg_1);//PU binary reader ,load BR on ES

        //            deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);//PU
        //            deserializeIL.Emit(OpCodes.Newobj, ctorDtTime);//PU
        //            deserializeIL.Emit(OpCodes.Stfld, fld);//PU
        //            deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
        //            deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent2);

        //        }

        //        #endregion

        //        #region OPCodes for IDictionary DeSerialization

        //        else if (fi.FieldType == typeof(IDictionary))
        //        {
        //            Label loopLabelBegin = deserializeIL.DefineLabel();
        //            Label loopLabelEnd = deserializeIL.DefineLabel();

        //            LocalBuilder count = deserializeIL.DeclareLocal(typeof(Int32));
        //            LocalBuilder key = deserializeIL.DeclareLocal(typeof(string));
        //            LocalBuilder value = deserializeIL.DeclareLocal(typeof(string));
        //            LocalBuilder boolVal = deserializeIL.DeclareLocal(typeof(bool));

        //            MethodInfo toString = typeof(object).GetMethod("ToString", new Type[0]);
        //            MethodInfo dicAdd = typeof(IDictionary).GetMethod("Add", new Type[] { typeof(object), typeof(object) });
        //            MethodInfo brReadInt = typeof(TStream).GetMethod(GetReaderMethod(typeof(int)));

        //            deserializeIL.Emit(OpCodes.Ldarg_1);//PU binary reader ,load BR on ES
        //            deserializeIL.EmitCall(OpCodes.Callvirt, brReadInt, null);//PU
        //            deserializeIL.Emit(OpCodes.Stloc, count);

        //            deserializeIL.Emit(OpCodes.Br, loopLabelEnd);

        //            deserializeIL.MarkLabel(loopLabelBegin); //begin loop 
        //            deserializeIL.Emit(OpCodes.Nop);
        //            deserializeIL.Emit(OpCodes.Ldarg_1);
        //            deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);
        //            deserializeIL.Emit(OpCodes.Stloc, key);
        //            deserializeIL.Emit(OpCodes.Ldarg_1);
        //            deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);
        //            deserializeIL.Emit(OpCodes.Stloc, value);

        //            deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
        //            //deserializeIL.EmitCall(OpCodes.Callvirt, getProp, null);
        //            deserializeIL.Emit(OpCodes.Ldfld, fld);
        //            deserializeIL.Emit(OpCodes.Ldloc, key);
        //            deserializeIL.Emit(OpCodes.Ldloc, value);
        //            deserializeIL.EmitCall(OpCodes.Callvirt, dicAdd, null);//call add method
        //            deserializeIL.Emit(OpCodes.Nop);

        //            deserializeIL.Emit(OpCodes.Ldloc, count);
        //            deserializeIL.Emit(OpCodes.Ldc_I4_1);
        //            deserializeIL.Emit(OpCodes.Sub);
        //            deserializeIL.Emit(OpCodes.Stloc, count);

        //            deserializeIL.MarkLabel(loopLabelEnd); //end loop 
        //            deserializeIL.Emit(OpCodes.Nop);
        //            deserializeIL.Emit(OpCodes.Ldloc, count);
        //            deserializeIL.Emit(OpCodes.Ldc_I4_0);
        //            deserializeIL.Emit(OpCodes.Ceq);
        //            deserializeIL.Emit(OpCodes.Ldc_I4_0);
        //            deserializeIL.Emit(OpCodes.Ceq);
        //            deserializeIL.Emit(OpCodes.Stloc_S, boolVal);
        //            deserializeIL.Emit(OpCodes.Ldloc_S, boolVal);
        //            deserializeIL.Emit(OpCodes.Brtrue_S, loopLabelBegin);

        //            deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
        //            deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent2);
        //        }
        //        #endregion

        //        #region OPCodes for other types DeSerialization

        //        else
        //        {
        //            deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);//load new obj on ES
        //            deserializeIL.Emit(OpCodes.Ldarg_1);//PU binary reader ,load BR on ES

        //            deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);//PU
        //            deserializeIL.Emit(OpCodes.Stfld, fld);//PU
        //            deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
        //            deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent2);
        //        }

        //        #endregion

        //    }
        //}

        //#endregion

        #region json

        internal static Type GenerateJsonSerializationSurrogateType<T, TStream>(Type type)
        {
            #region IL Initilization Code

            Type retType = null;

            try
            {
                AppDomain domain = Thread.GetDomain();
                AssemblyName asmName = new AssemblyName();
                asmName.Name = typeof(TStream).Name + "Surrogate";
                AssemblyBuilder asmBuilder = domain.DefineDynamicAssembly(
                                                                   asmName,
                                                                   AssemblyBuilderAccess.Run);

                ModuleBuilder surrogateModule = asmBuilder.DefineDynamicModule(typeof(TStream).Name + "SurrogateModule");
                //ModuleBuilder surrogateModule = myAsmBuilder.DefineDynamicModule( "SurrogateModule", "Surrogate.dll");

                TypeBuilder typeBuilder = surrogateModule.DefineType(typeof(TStream).Name + "_" + type.Name + "_EventSurrogate",
                                                                    TypeAttributes.Public);
                typeBuilder.AddInterfaceImplementation(typeof(T));

                //TypeBuilder eventTypeBuilder = surrogateModule.DefineType(EventType.Name, TypeAttributes.Public);
                //TypeBuilder eventTypeBuilder = BuildTypeHierarchy(surrogateModule, EventType);

            #endregion

                #region Serialize Method Builder

                Type[] dpParams = new Type[] { typeof(TStream), typeof(object) };
                MethodBuilder serializeMethod = typeBuilder.DefineMethod(
                                                       "Serialize",
                                                        MethodAttributes.Public | MethodAttributes.Virtual,
                                                        typeof(void),
                                                        dpParams);

                ILGenerator mthdIL = serializeMethod.GetILGenerator();


                Label labelFinally = mthdIL.DefineLabel();
                if (type.IsClass)
                {
                    LocalBuilder tpmEvent = mthdIL.DeclareLocal(type);
                    
                    mthdIL.Emit(OpCodes.Nop);
                    mthdIL.Emit(OpCodes.Ldarg_2);//PU
                    mthdIL.Emit(OpCodes.Castclass, type);//PU
                    mthdIL.Emit(OpCodes.Stloc, tpmEvent);//PP

                    SerializeJsonFields<TStream>(type, mthdIL, tpmEvent);
                    SerializeJsonPropertys<TStream>(type, mthdIL, tpmEvent);
                }
                else
                {
                    MethodInfo brWrite = CreateWriterMethod<TStream>(type);
                    mthdIL.Emit(OpCodes.Ldarg_0);
                    mthdIL.Emit(OpCodes.Ldarg_1);
                    mthdIL.Emit(OpCodes.Unbox_Any, type);
                    mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);
                    mthdIL.Emit(OpCodes.Ret);
                }


                mthdIL.MarkLabel(labelFinally);
                mthdIL.Emit(OpCodes.Ret);
                mthdIL = null;

                #endregion

                #region Deserialize Method Builder

                //dpParams = new Type[] { typeof(TStream) };
                //MethodBuilder deserializeMthd = typeBuilder.DefineMethod(
                //                                       "DeSerialize",
                //                                        MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.Final | MethodAttributes.NewSlot,
                //                                        typeof(object),
                //                                        dpParams);

                //ILGenerator deserializeIL = deserializeMthd.GetILGenerator();

                //if (type.IsClass == true)
                //{
                //    LocalBuilder tpmRetEvent = deserializeIL.DeclareLocal(type);
                //    LocalBuilder tpmRetEvent2 = deserializeIL.DeclareLocal(type);
                //    Label ret = deserializeIL.DefineLabel();

                //    ConstructorInfo ctorEvent = type.GetConstructor(new Type[0]);
                //    if (ctorEvent == null)
                //        throw new Exception("The event class is missing a default constructor with 0 params");

                //    deserializeIL.Emit(OpCodes.Newobj, ctorEvent);
                //    deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent);


                //    DeserializeJsonFields<TStream>(type, deserializeIL, tpmRetEvent, tpmRetEvent2);
                //    DeserializeJsonPropertys<TStream>(type, deserializeIL, tpmRetEvent, tpmRetEvent2);

                //    deserializeIL.Emit(OpCodes.Br_S, ret);
                //    deserializeIL.MarkLabel(ret);
                //    deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent2);
                //    deserializeIL.Emit(OpCodes.Ret);
                //}
                //else
                //{
                //    //ConstructorInfo ctorEvent = type.GetConstructor(new Type[0]);
                //    //if (ctorEvent == null)
                //    //    throw new Exception("The event class is missing a default constructor with 0 params");

                //    //deserializeIL.Emit(OpCodes.Newobj, ctorEvent);
                //    //deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent);

                //    //deserializeIL.Emit(OpCodes.Br_S, ret);
                //    //deserializeIL.MarkLabel(ret);
                //    //deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent2);
                //    //deserializeIL.Emit(OpCodes.Ret);

                //    deserializeIL.Emit(OpCodes.Ldnull);
                //    deserializeIL.Emit(OpCodes.Stloc_0);
                //    deserializeIL.Emit(OpCodes.Ldloc_0);
                //    deserializeIL.Emit(OpCodes.Ret);
                //}
                
                #endregion

                retType = typeBuilder.CreateType();
            }
            catch (Exception x)
            {
                throw x;
            }
            return retType;
        }

        private static void SerializeJsonFields<T>(Type type, ILGenerator mthdIL, LocalBuilder tpmEvent)
        {
            //Dictionary<string, FieldInfo> fldMap = new Dictionary<string, FieldInfo>();
            MethodInfo dateTimeTicks = typeof(DateTime).GetMethod("get_Ticks");
            MethodInfo objectArrayList = typeof(List<object>).GetMethod("ToArray");
            MethodInfo brWriteObjects = CreateWriterMethod<T>(typeof(object[]));
            
            foreach (FieldInfo fi in type.GetFields())
            {
                //if (fi.FieldType == typeof(string[]))
                //    continue;
                if (IsIgnoreAttribute(fi.GetCustomAttributes(true)))
                    continue;

                MethodInfo brWrite = CreateWriterMethod<T>(fi.FieldType);

                //FieldBuilder fld = eventTypeBuilder.DefineField(fi.Name, fi.FieldType, fi.Attributes);
                //TypeBuilder bb = declaringTypeMap[fi.DeclaringType.FullName];
                //FieldInfo fld = null;
                FieldInfo fld = fi;// bb.GetField(fi.Name);


                //fldMap[fi.Name] = fld;

                mthdIL.Emit(OpCodes.Ldarg_1);//PU binary writer
                mthdIL.Emit(OpCodes.Ldloc, tpmEvent);//PU binary writer

                #region OPCodes for DateTime serialization

                if (fi.FieldType == typeof(DateTime))
                {
                    mthdIL.Emit(OpCodes.Ldflda, fld);
                    mthdIL.EmitCall(OpCodes.Call, dateTimeTicks, null);//PU
                    mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU
                }

                #endregion


                #region  OPCodes for IDictionary serialization

                else if (fi.FieldType == typeof(IDictionary))
                {

                    Label loopLabelBegin = mthdIL.DefineLabel();
                    Label loopLabelEnd = mthdIL.DefineLabel();
                    Label endFinally = mthdIL.DefineLabel();

                    LocalBuilder dictionaryEntry = mthdIL.DeclareLocal(typeof(DictionaryEntry));
                    LocalBuilder dicEnumerator = mthdIL.DeclareLocal(typeof(IDictionaryEnumerator));
                    LocalBuilder comparsionResult = mthdIL.DeclareLocal(typeof(bool));
                    LocalBuilder locIDisposable = mthdIL.DeclareLocal(typeof(IDisposable));

                    MethodInfo toString = typeof(object).GetMethod("ToString", new Type[0]);
                    MethodInfo getEnumerator = typeof(IDictionary).GetMethod("GetEnumerator", new Type[0]);
                    MethodInfo moveNext = typeof(IEnumerator).GetMethod("MoveNext", new Type[0]);
                    MethodInfo getCurrent = typeof(IEnumerator).GetMethod("get_Current", new Type[0]);
                    MethodInfo dispose = typeof(IDisposable).GetMethod("Dispose", new Type[0]);
                    MethodInfo get_Key = typeof(DictionaryEntry).GetMethod("get_Key", new Type[0]);
                    MethodInfo get_Value = typeof(DictionaryEntry).GetMethod("get_Value", new Type[0]);
                    MethodInfo get_Count = typeof(ICollection).GetMethod("get_Count");
                    MethodInfo brWriteInt = CreateWriterMethod<T>(typeof(int));

                    mthdIL.Emit(OpCodes.Ldfld, fld);
                    mthdIL.EmitCall(OpCodes.Callvirt, get_Count, null);// get the array count
                    mthdIL.EmitCall(OpCodes.Callvirt, brWriteInt, null);// write the count
                    mthdIL.Emit(OpCodes.Nop);
                    mthdIL.Emit(OpCodes.Nop);


                    mthdIL.Emit(OpCodes.Ldloc, tpmEvent);//PU load the event object
                    //mthdIL.EmitCall(OpCodes.Callvirt, mi, null);//PU load  the  proprety again  into ES
                    mthdIL.Emit(OpCodes.Ldfld, fld);
                    mthdIL.EmitCall(OpCodes.Callvirt, getEnumerator, null);// get the enumerator
                    mthdIL.Emit(OpCodes.Stloc, dicEnumerator);//save the enumerator

                    mthdIL.BeginExceptionBlock();

                    mthdIL.Emit(OpCodes.Br, loopLabelEnd);// start the loop

                    mthdIL.MarkLabel(loopLabelBegin);//begin for each loop

                    mthdIL.Emit(OpCodes.Ldloc, dicEnumerator);//PU load the enumerator
                    mthdIL.EmitCall(OpCodes.Callvirt, getCurrent, null);// call get_Current
                    mthdIL.Emit(OpCodes.Unbox_Any, typeof(DictionaryEntry));
                    mthdIL.Emit(OpCodes.Stloc, dictionaryEntry);// save the DictionaryEntry

                    //get key
                    mthdIL.Emit(OpCodes.Nop);
                    mthdIL.Emit(OpCodes.Ldarg_1);//PU binary writer
                    mthdIL.Emit(OpCodes.Ldloca_S, dictionaryEntry);//load the DictionaryEntry
                    mthdIL.EmitCall(OpCodes.Call, get_Key, null);// call get_Key
                    mthdIL.EmitCall(OpCodes.Callvirt, toString, null);// call toString
                    mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU call binary writer write

                    //get value
                    mthdIL.Emit(OpCodes.Nop);
                    mthdIL.Emit(OpCodes.Ldarg_1);//PU binary writer
                    mthdIL.Emit(OpCodes.Ldloca_S, dictionaryEntry);//load the DictionaryEntry
                    mthdIL.EmitCall(OpCodes.Call, get_Value, null);// call get_Value
                    mthdIL.EmitCall(OpCodes.Callvirt, toString, null);// call toString
                    mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU call binary writer write

                    mthdIL.Emit(OpCodes.Nop);
                    mthdIL.Emit(OpCodes.Nop);

                    mthdIL.MarkLabel(loopLabelEnd);//end for each loop
                    mthdIL.Emit(OpCodes.Ldloc, dicEnumerator);//PU load the enumerator
                    mthdIL.EmitCall(OpCodes.Callvirt, moveNext, null);// call move next
                    mthdIL.Emit(OpCodes.Stloc, comparsionResult);//save the result
                    mthdIL.Emit(OpCodes.Ldloc, comparsionResult);//load the result
                    mthdIL.Emit(OpCodes.Brtrue_S, loopLabelBegin);//loop if true
                    //mthdIL.Emit(OpCodes.Leave_S, labelFinally);//leave if false

                    mthdIL.BeginFinallyBlock();

                    mthdIL.Emit(OpCodes.Ldloc, dicEnumerator);//PU load the enumerator
                    mthdIL.Emit(OpCodes.Isinst, typeof(System.IDisposable));
                    mthdIL.Emit(OpCodes.Stloc_S, locIDisposable);
                    mthdIL.Emit(OpCodes.Ldloc_S, locIDisposable);
                    mthdIL.Emit(OpCodes.Ldnull);
                    mthdIL.Emit(OpCodes.Ceq);
                    mthdIL.Emit(OpCodes.Stloc, comparsionResult);
                    mthdIL.Emit(OpCodes.Ldloc, comparsionResult);
                    mthdIL.Emit(OpCodes.Brtrue_S, endFinally);
                    mthdIL.Emit(OpCodes.Ldloc_S, locIDisposable);//load IDisposable
                    mthdIL.EmitCall(OpCodes.Callvirt, dispose, null);// call IDisposable::Dispose
                    mthdIL.Emit(OpCodes.Nop);

                    mthdIL.MarkLabel(endFinally);
                    mthdIL.EndExceptionBlock();

                }

                #endregion

                #region List

                //else if (fi.FieldType == typeof(int[]))
                //{
                //    LocalBuilder tmpTicks = mthdIL.DeclareLocal(typeof(int[]));
                //    mthdIL.Emit(OpCodes.Stloc, tmpTicks);
                //    mthdIL.Emit(OpCodes.Ldloca_S, tmpTicks);
                //    mthdIL.EmitCall(OpCodes.Call, int32ArrayList, null);//PU
                //    mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU
                //}
                else if (fi.FieldType.GetInterface("IEnumerable") == typeof(IEnumerable))
                {
                    bool isExplicit = false;
                    Type t = fi.FieldType;
                    //可能是List及相关
                    if (t.GetGenericArguments().Length == 1)
                    {
                        if (t == typeof(List<byte>) || t == typeof(IList<byte>) ||
                            t == typeof(List<sbyte>) || t == typeof(IList<sbyte>) ||
                            t == typeof(List<short>) || t == typeof(IList<short>) ||
                            t == typeof(List<ushort>) || t == typeof(IList<ushort>) ||
                            t == typeof(List<int>) || t == typeof(IList<int>) ||
                            t == typeof(List<uint>) || t == typeof(IList<uint>) ||
                            t == typeof(List<long>) || t == typeof(IList<long>) ||
                            t == typeof(List<ulong>) || t == typeof(IList<ulong>) ||
                            t == typeof(List<float>) || t == typeof(IList<float>) ||
                            t == typeof(List<double>) || t == typeof(IList<double>) ||
                            t == typeof(List<decimal>) || t == typeof(IList<decimal>) ||
                            t == typeof(List<string>) || t == typeof(IList<string>)
                            )
                        {
                            isExplicit = true;
                        }
                    }
                    //可能是Dictionary及相关
                    else if (t.GetGenericArguments().Length == 2)
                    {
                        MethodInfo brWriteIEnumable = CreateWriterMethod<T>(typeof(IDictionary));
                        mthdIL.Emit(OpCodes.Ldfld, fld);
                        mthdIL.EmitCall(OpCodes.Callvirt, brWriteIEnumable, null);//PU
                        continue;
                    }
                    else
                    {
                        if (t == typeof(string) || t == typeof(string[]) ||
                            t == typeof(byte[]) ||
                            t == typeof(sbyte[]) ||
                            t == typeof(short[]) ||
                            t == typeof(ushort[]) ||
                            t == typeof(int[]) ||
                            t == typeof(uint[]) ||
                            t == typeof(long[]) ||
                            t == typeof(ulong[]) ||
                            t == typeof(float[]) ||
                            t == typeof(double[]) ||
                            t == typeof(decimal[]) ||
                            t == typeof(bool[]) ||
                            t == typeof(DateTime[]) ||
                            t == typeof(DateTimeOffset[]) ||
                            t == typeof(Guid[]) ||
                            t == typeof(Enum[]) ||
                            t == typeof(Uri[]) ||
                            t == typeof(TimeSpan[])
                            )
                            isExplicit = true;
                    }
                    if (isExplicit == true)
                    {
                        mthdIL.Emit(OpCodes.Ldfld, fld);
                        mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU
                        //mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU
                    }
                        //属性的Object写入还存在问题
                    else
                    {
                        //if (fi.FieldType != typeof(IList<object>))
                        //{
                        MethodInfo brWriteIEnumable = CreateWriterMethod<T>(typeof(IEnumerable));
                        mthdIL.Emit(OpCodes.Ldfld, fld);
                        mthdIL.EmitCall(OpCodes.Callvirt, brWriteIEnumable, null);//PU
                        //}
                        //else
                        //{
                        //    MethodInfo brWriteIEnumable = typeof(TStream).GetMethod("Write", new Type[] { typeof(IEnumerable), typeof(bool) });
                        //    mthdIL.Emit(OpCodes.Ldfld, fld);
                        //    mthdIL.Emit(OpCodes.Ldfld, true);
                        //    mthdIL.EmitCall(OpCodes.Callvirt, brWriteIEnumable,null);//PU
                        //}

                        //LocalBuilder tmpArray = mthdIL.DeclareLocal(typeof(object[]));
                        //mthdIL.Emit(OpCodes.Ldnull, tmpArray);
                        //mthdIL.Emit(OpCodes.Stloc, tmpArray);
                        ////mthdIL.Emit(OpCodes.Stloc_2, tpmEvent);
                        ////mthdIL.Emit(OpCodes.Stloc_1, tpmEvent);
                        //mthdIL.EmitCall(OpCodes.Callvirt, objectArrayList, null);//PU
                        ////mthdIL.Emit(OpCodes.Stloc_2, tpmEvent);
                        ////mthdIL.Emit(OpCodes.Ldloc_0, tpmEvent);
                        ////mthdIL.Emit(OpCodes.Ldloc_2, tpmEvent);
                        //mthdIL.EmitCall(OpCodes.Callvirt, brWriteObjects, null);//PU
                    }
                }

                #endregion

                #region OPCodes for other types serialization
                else
                {
                    mthdIL.Emit(OpCodes.Ldfld, fld);
                    mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU
                }
                #endregion

                mthdIL.Emit(OpCodes.Nop);
            }
        }

        private static void SerializeJsonPropertys<T>(Type type, ILGenerator mthdIL, LocalBuilder tpmEvent)
        {
            MethodInfo dateTimeTicks = typeof(DateTime).GetMethod("get_Ticks");
            MethodInfo objectArrayList = typeof(List<object>).GetMethod("ToArray");
            MethodInfo brWriteObjects = CreateWriterMethod<T>(typeof(object[]));
            foreach (PropertyInfo pi in type.GetProperties())
            {
                if (pi.PropertyType == typeof(string[]))
                    continue;
                if (IsIgnoreAttribute(pi.GetCustomAttributes(true)))
                    continue;

                MethodInfo mi = type.GetMethod("get_" + pi.Name);
                MethodInfo brWrite = CreateWriterMethod<T>(pi.PropertyType);

                mthdIL.Emit(OpCodes.Ldarg_1);//PU binary writer
                mthdIL.Emit(OpCodes.Ldloc, tpmEvent);//PU load the event object
                mthdIL.EmitCall(OpCodes.Callvirt, mi, null);//PU get the value of the proprty

                #region OPCodes for DateTime serialization

                if (pi.PropertyType == typeof(DateTime))
                {
                    LocalBuilder tmpTicks = mthdIL.DeclareLocal(typeof(DateTime));
                    mthdIL.Emit(OpCodes.Stloc, tmpTicks);
                    mthdIL.Emit(OpCodes.Ldloca_S, tmpTicks);
                    mthdIL.EmitCall(OpCodes.Call, dateTimeTicks, null);//PU
                    mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU
                }
                #endregion

                #region OPCodes for IDictionary serialization

                else if (pi.PropertyType == typeof(IDictionary))
                {

                    Label loopLabelBegin = mthdIL.DefineLabel();
                    Label loopLabelEnd = mthdIL.DefineLabel();
                    Label endFinally = mthdIL.DefineLabel();

                    LocalBuilder dictionaryEntry = mthdIL.DeclareLocal(typeof(DictionaryEntry));
                    LocalBuilder dicEnumerator = mthdIL.DeclareLocal(typeof(IDictionaryEnumerator));
                    LocalBuilder comparsionResult = mthdIL.DeclareLocal(typeof(bool));
                    LocalBuilder locIDisposable = mthdIL.DeclareLocal(typeof(IDisposable));

                    MethodInfo toString = typeof(object).GetMethod("ToString", new Type[0]);
                    MethodInfo getEnumerator = typeof(IDictionary).GetMethod("GetEnumerator", new Type[0]);
                    MethodInfo moveNext = typeof(IEnumerator).GetMethod("MoveNext", new Type[0]);
                    MethodInfo getCurrent = typeof(IEnumerator).GetMethod("get_Current", new Type[0]);
                    MethodInfo dispose = typeof(IDisposable).GetMethod("Dispose", new Type[0]);
                    MethodInfo get_Key = typeof(DictionaryEntry).GetMethod("get_Key", new Type[0]);
                    MethodInfo get_Value = typeof(DictionaryEntry).GetMethod("get_Value", new Type[0]);
                    MethodInfo get_Count = typeof(ICollection).GetMethod("get_Count");
                    MethodInfo brWriteInt = CreateWriterMethod<T>(typeof(int));

                    mthdIL.EmitCall(OpCodes.Callvirt, get_Count, null);// get the array count
                    mthdIL.EmitCall(OpCodes.Callvirt, brWriteInt, null);// write the count
                    mthdIL.Emit(OpCodes.Nop);
                    mthdIL.Emit(OpCodes.Nop);


                    mthdIL.Emit(OpCodes.Ldloc, tpmEvent);//PU load the event object
                    mthdIL.EmitCall(OpCodes.Callvirt, mi, null);//PU load  the  proprety again  into ES
                    mthdIL.EmitCall(OpCodes.Callvirt, getEnumerator, null);// get the enumerator
                    mthdIL.Emit(OpCodes.Stloc, dicEnumerator);//save the enumerator

                    mthdIL.BeginExceptionBlock();

                    mthdIL.Emit(OpCodes.Br, loopLabelEnd);// start the loop

                    mthdIL.MarkLabel(loopLabelBegin);//begin for each loop

                    mthdIL.Emit(OpCodes.Ldloc, dicEnumerator);//PU load the enumerator
                    mthdIL.EmitCall(OpCodes.Callvirt, getCurrent, null);// call get_Current
                    mthdIL.Emit(OpCodes.Unbox_Any, typeof(DictionaryEntry));
                    mthdIL.Emit(OpCodes.Stloc, dictionaryEntry);// save the DictionaryEntry

                    //get key
                    mthdIL.Emit(OpCodes.Nop);
                    mthdIL.Emit(OpCodes.Ldarg_1);//PU binary writer
                    mthdIL.Emit(OpCodes.Ldloca_S, dictionaryEntry);//load the DictionaryEntry
                    mthdIL.EmitCall(OpCodes.Call, get_Key, null);// call get_Key
                    mthdIL.EmitCall(OpCodes.Callvirt, toString, null);// call toString
                    mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU call binary writer write

                    //get value
                    mthdIL.Emit(OpCodes.Nop);
                    mthdIL.Emit(OpCodes.Ldarg_1);//PU binary writer
                    mthdIL.Emit(OpCodes.Ldloca_S, dictionaryEntry);//load the DictionaryEntry
                    mthdIL.EmitCall(OpCodes.Call, get_Value, null);// call get_Value
                    mthdIL.EmitCall(OpCodes.Callvirt, toString, null);// call toString
                    mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU call binary writer write

                    mthdIL.Emit(OpCodes.Nop);
                    mthdIL.Emit(OpCodes.Nop);

                    mthdIL.MarkLabel(loopLabelEnd);//end for each loop
                    mthdIL.Emit(OpCodes.Ldloc, dicEnumerator);//PU load the enumerator
                    mthdIL.EmitCall(OpCodes.Callvirt, moveNext, null);// call move next
                    mthdIL.Emit(OpCodes.Stloc, comparsionResult);//save the result
                    mthdIL.Emit(OpCodes.Ldloc, comparsionResult);//load the result
                    mthdIL.Emit(OpCodes.Brtrue_S, loopLabelBegin);//loop if true
                    //mthdIL.Emit(OpCodes.Leave_S, labelFinally);//leave if false

                    mthdIL.BeginFinallyBlock();

                    mthdIL.Emit(OpCodes.Ldloc, dicEnumerator);//PU load the enumerator
                    mthdIL.Emit(OpCodes.Isinst, typeof(System.IDisposable));
                    mthdIL.Emit(OpCodes.Stloc_S, locIDisposable);
                    mthdIL.Emit(OpCodes.Ldloc_S, locIDisposable);
                    mthdIL.Emit(OpCodes.Ldnull);
                    mthdIL.Emit(OpCodes.Ceq);
                    mthdIL.Emit(OpCodes.Stloc, comparsionResult);
                    mthdIL.Emit(OpCodes.Ldloc, comparsionResult);
                    mthdIL.Emit(OpCodes.Brtrue_S, endFinally);
                    mthdIL.Emit(OpCodes.Ldloc_S, locIDisposable);//load IDisposable
                    mthdIL.EmitCall(OpCodes.Callvirt, dispose, null);// call IDisposable::Dispose
                    mthdIL.Emit(OpCodes.Nop);

                    mthdIL.MarkLabel(endFinally);
                    mthdIL.EndExceptionBlock();

                }

                #endregion

                #region List

                #region old
                //else if (pi.PropertyType == typeof(List<int>))
                //{
                //    //LocalBuilder tmpTicks = mthdIL.DeclareLocal(typeof(List<int>));
                //    //mthdIL.Emit(OpCodes.Stloc, tmpTicks);
                //    //mthdIL.Emit(OpCodes.Ldloca_S, tmpTicks);
                //    //mthdIL.EmitCall(OpCodes.Callvirt, int32ArrayList, null);//PU
                //    //mthdIL.Emit(OpCodes.Stloc_2, tmpTicks);
                //    //mthdIL.Emit(OpCodes.Ldloc_0, tmpTicks);
                //    //mthdIL.Emit(OpCodes.Ldloc_2, tmpTicks);
                //    //mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU

                //    //LocalBuilder tmpTicks = mthdIL.DeclareLocal(typeof(List<int>));
                //    //mthdIL.Emit(OpCodes.Stloc, tmpTicks);
                //    //mthdIL.Emit(OpCodes.Ldloca_S, tmpTicks);

                //    //mthdIL.Emit(OpCodes.Ldloc_1, tpmEvent);
                //    //mthdIL.EmitCall(OpCodes.Callvirt, int32ArrayList, null);//PU
                //    //mthdIL.Emit(OpCodes.Stloc_2, tpmEvent);
                //    //mthdIL.Emit(OpCodes.Ldloc_0, tpmEvent);
                //    //mthdIL.Emit(OpCodes.Ldloc_2, tpmEvent);
                //    //mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU

                //    LocalBuilder tmpArray = mthdIL.DeclareLocal(typeof(int[]));
                //    mthdIL.Emit(OpCodes.Ldnull, tmpArray);
                //    mthdIL.Emit(OpCodes.Stloc, tmpArray);
                //    //mthdIL.Emit(OpCodes.Stloc_2, tpmEvent);
                //    //mthdIL.Emit(OpCodes.Stloc_1, tpmEvent);
                //    mthdIL.EmitCall(OpCodes.Callvirt, int32ArrayList, null);//PU
                //    //mthdIL.Emit(OpCodes.Stloc_2, tpmEvent);
                //    //mthdIL.Emit(OpCodes.Ldloc_0, tpmEvent);
                //    //mthdIL.Emit(OpCodes.Ldloc_2, tpmEvent);
                //    mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU
                //}
                //else if (pi.PropertyType == typeof(IList<int>))
                //{
                //    LocalBuilder tmpArray = mthdIL.DeclareLocal(typeof(int[]));
                //    mthdIL.Emit(OpCodes.Ldnull, tmpArray);
                //    mthdIL.Emit(OpCodes.Stloc, tmpArray);
                //    //mthdIL.Emit(OpCodes.Stloc_2, tpmEvent);
                //    //mthdIL.Emit(OpCodes.Stloc_1, tpmEvent);
                //    mthdIL.EmitCall(OpCodes.Callvirt, int32ArrayList, null);//PU
                //    //mthdIL.Emit(OpCodes.Stloc_2, tpmEvent);
                //    //mthdIL.Emit(OpCodes.Ldloc_0, tpmEvent);
                //    //mthdIL.Emit(OpCodes.Ldloc_2, tpmEvent);
                //    mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU
                //}
                ////else if (pi.PropertyType.GetGenericArguments().Length > 0)
                #endregion

                else if (pi.PropertyType.GetInterface("IEnumerable") == typeof(IEnumerable))
                {
                    bool isExplicit = false;
                    Type t = pi.PropertyType;
                    //可能是List及相关
                    if (pi.PropertyType.GetGenericArguments().Length == 1)
                    {
                        if (t == typeof(List<byte>) || t == typeof(IList<byte>) ||
                            t == typeof(List<sbyte>) || t == typeof(IList<sbyte>) ||
                            t == typeof(List<short>) || t == typeof(IList<short>) ||
                            t == typeof(List<ushort>) || t == typeof(IList<ushort>) ||
                            t == typeof(List<int>) || t == typeof(IList<int>) ||
                            t == typeof(List<uint>) || t == typeof(IList<uint>) ||
                            t == typeof(List<long>) || t == typeof(IList<long>) ||
                            t == typeof(List<ulong>) || t == typeof(IList<ulong>) ||
                            t == typeof(List<float>) || t == typeof(IList<float>) ||
                            t == typeof(List<double>) || t == typeof(IList<double>) ||
                            t == typeof(List<decimal>) || t == typeof(IList<decimal>)
                            )
                        {
                            isExplicit = true;
                        }
                    }
                    //可能是Dictionary及相关
                    else if (pi.PropertyType.GetGenericArguments().Length == 2)
                    {

                    }
                    else
                    {
                        if (t == typeof(string) || t == typeof(string[]) ||
                            t == typeof(byte[]) ||
                            t == typeof(sbyte[]) ||
                            t == typeof(short[]) ||
                            t == typeof(ushort[]) ||
                            t == typeof(int[]) ||
                            t == typeof(uint[]) ||
                            t == typeof(long[]) ||
                            t == typeof(ulong[]) ||
                            t == typeof(float[]) ||
                            t == typeof(double[]) ||
                            t == typeof(decimal[]) ||
                            t == typeof(bool[]) ||
                            t == typeof(DateTime[]) ||
                            t == typeof(DateTimeOffset[]) ||
                            t == typeof(Guid[]) ||
                            t == typeof(Enum[]) ||
                            t == typeof(Uri[]) ||
                            t == typeof(TimeSpan[])
                            )
                            isExplicit = true;
                    }
                    if (isExplicit == true)
                        mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU
                    //对于纯Object类型可能特殊处理比较好
                    else
                    {
                        LocalBuilder tmpArray = mthdIL.DeclareLocal(typeof(object[]));
                        mthdIL.Emit(OpCodes.Ldnull, tmpArray);
                        mthdIL.Emit(OpCodes.Stloc, tmpArray);
                        //mthdIL.Emit(OpCodes.Stloc_2, tpmEvent);
                        //mthdIL.Emit(OpCodes.Stloc_1, tpmEvent);
                        mthdIL.EmitCall(OpCodes.Callvirt, objectArrayList, null);//PU
                        //mthdIL.Emit(OpCodes.Stloc_2, tpmEvent);
                        //mthdIL.Emit(OpCodes.Ldloc_0, tpmEvent);
                        //mthdIL.Emit(OpCodes.Ldloc_2, tpmEvent);
                        mthdIL.EmitCall(OpCodes.Callvirt, brWriteObjects, null);//PU
                    }
                }

                #endregion

                #region OPCodes for all other type serialization

                else
                    mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU

                #endregion

                mthdIL.Emit(OpCodes.Nop);
            }
        }

        private static void DeserializeJsonFields<T>(Type type, ILGenerator deserializeIL, LocalBuilder tpmRetEvent, LocalBuilder tpmRetEvent2)
        {
            foreach (FieldInfo fi in type.GetFields())
            {
                if (fi.FieldType == typeof(string[]))
                    continue;
                if (IsIgnoreAttribute(fi.GetCustomAttributes(true)))
                    continue;

                LocalBuilder locTyp = deserializeIL.DeclareLocal(fi.FieldType);
                //MethodInfo brRead = typeof(TStream).GetMethod(GetJsonReaderMethod(fi.FieldType));
                MethodInfo brRead = typeof(T).GetMethod(CreateReaderMethod(fi.FieldType), flag, null, new Type[] { }, null);

                //FieldInfo fld = fldMap[fi.Name];
                FieldInfo fld = fi;


                #region OPCodes for DateTime DeSerialization

                if (fi.FieldType == typeof(DateTime))
                {
                    ConstructorInfo ctorDtTime = typeof(DateTime).GetConstructor(new Type[] { typeof(Int64) });

                    deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);//load new obj on ES
                    deserializeIL.Emit(OpCodes.Ldarg_1);//PU binary reader ,load BR on ES

                    deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);//PU
                    deserializeIL.Emit(OpCodes.Newobj, ctorDtTime);//PU
                    deserializeIL.Emit(OpCodes.Stfld, fld);//PU
                    deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
                    deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent2);

                }

                #endregion

                #region OPCodes for IDictionary DeSerialization

                else if (fi.FieldType == typeof(IDictionary))
                {
                    Label loopLabelBegin = deserializeIL.DefineLabel();
                    Label loopLabelEnd = deserializeIL.DefineLabel();

                    LocalBuilder count = deserializeIL.DeclareLocal(typeof(Int32));
                    LocalBuilder key = deserializeIL.DeclareLocal(typeof(string));
                    LocalBuilder value = deserializeIL.DeclareLocal(typeof(string));
                    LocalBuilder boolVal = deserializeIL.DeclareLocal(typeof(bool));

                    MethodInfo toString = typeof(object).GetMethod("ToString", new Type[0]);
                    MethodInfo dicAdd = typeof(IDictionary).GetMethod("Add", new Type[] { typeof(object), typeof(object) });
                    MethodInfo brReadInt = typeof(T).GetMethod(CreateReaderMethod(typeof(int)));

                    deserializeIL.Emit(OpCodes.Ldarg_1);//PU binary reader ,load BR on ES
                    deserializeIL.EmitCall(OpCodes.Callvirt, brReadInt, null);//PU
                    deserializeIL.Emit(OpCodes.Stloc, count);

                    deserializeIL.Emit(OpCodes.Br, loopLabelEnd);

                    deserializeIL.MarkLabel(loopLabelBegin); //begin loop 
                    deserializeIL.Emit(OpCodes.Nop);
                    deserializeIL.Emit(OpCodes.Ldarg_1);
                    deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);
                    deserializeIL.Emit(OpCodes.Stloc, key);
                    deserializeIL.Emit(OpCodes.Ldarg_1);
                    deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);
                    deserializeIL.Emit(OpCodes.Stloc, value);

                    deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
                    //deserializeIL.EmitCall(OpCodes.Callvirt, getProp, null);
                    deserializeIL.Emit(OpCodes.Ldfld, fld);
                    deserializeIL.Emit(OpCodes.Ldloc, key);
                    deserializeIL.Emit(OpCodes.Ldloc, value);
                    deserializeIL.EmitCall(OpCodes.Callvirt, dicAdd, null);//call add method
                    deserializeIL.Emit(OpCodes.Nop);

                    deserializeIL.Emit(OpCodes.Ldloc, count);
                    deserializeIL.Emit(OpCodes.Ldc_I4_1);
                    deserializeIL.Emit(OpCodes.Sub);
                    deserializeIL.Emit(OpCodes.Stloc, count);

                    deserializeIL.MarkLabel(loopLabelEnd); //end loop 
                    deserializeIL.Emit(OpCodes.Nop);
                    deserializeIL.Emit(OpCodes.Ldloc, count);
                    deserializeIL.Emit(OpCodes.Ldc_I4_0);
                    deserializeIL.Emit(OpCodes.Ceq);
                    deserializeIL.Emit(OpCodes.Ldc_I4_0);
                    deserializeIL.Emit(OpCodes.Ceq);
                    deserializeIL.Emit(OpCodes.Stloc_S, boolVal);
                    deserializeIL.Emit(OpCodes.Ldloc_S, boolVal);
                    deserializeIL.Emit(OpCodes.Brtrue_S, loopLabelBegin);

                    deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
                    deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent2);
                }
                #endregion

                #region OPCodes for other types DeSerialization

                else
                {
                    deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);//load new obj on ES
                    deserializeIL.Emit(OpCodes.Ldarg_1);//PU binary reader ,load BR on ES

                    deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);//PU
                    deserializeIL.Emit(OpCodes.Stfld, fld);//PU
                    deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
                    deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent2);
                }

                #endregion

            }
        }

        private static void DeserializeJsonPropertys<T>(Type type, ILGenerator deserializeIL, LocalBuilder tpmRetEvent, LocalBuilder tpmRetEvent2)
        {
            foreach (PropertyInfo pi in type.GetProperties())
            {
                if (pi.PropertyType == typeof(string[]))
                    continue;
                if (IsIgnoreAttribute(pi.GetCustomAttributes(true)))
                    continue;

                MethodInfo brRead = typeof(T).GetMethod(CreateReaderMethod(pi.PropertyType));
                MethodInfo setProp = type.GetMethod("set_" + pi.Name);
                MethodInfo getProp = type.GetMethod("get_" + pi.Name);

                #region OPCodes for DateTime DeSerialization
                if (pi.PropertyType == typeof(DateTime))
                {
                    deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);//load new obj on ES
                    deserializeIL.Emit(OpCodes.Ldarg_1);//PU binary reader ,load BR on ES
                    deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);//PU

                    //似乎没用
                    //FieldBuilder dateTimeFld = typeBuilder.DefineField("Ticks ", typeof(Int64), FieldAttributes.Public);
                    ConstructorInfo ctorDtTime = typeof(DateTime).GetConstructor(new Type[] { typeof(Int64) });
                    deserializeIL.Emit(OpCodes.Newobj, ctorDtTime);//PU
                    deserializeIL.EmitCall(OpCodes.Callvirt, setProp, null);//PU

                    deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
                    deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent2);
                }
                #endregion

                #region OPCodes for IDictionary DeSerialization

                else if (pi.PropertyType == typeof(IDictionary))
                {
                    Label loopLabelBegin = deserializeIL.DefineLabel();
                    Label loopLabelEnd = deserializeIL.DefineLabel();

                    LocalBuilder count = deserializeIL.DeclareLocal(typeof(Int32));
                    LocalBuilder key = deserializeIL.DeclareLocal(typeof(string));
                    LocalBuilder value = deserializeIL.DeclareLocal(typeof(string));
                    LocalBuilder boolVal = deserializeIL.DeclareLocal(typeof(bool));

                    MethodInfo toString = typeof(object).GetMethod("ToString", new Type[0]);
                    MethodInfo dicAdd = typeof(IDictionary).GetMethod("Add", new Type[] { typeof(object), typeof(object) });
                    MethodInfo brReadInt = typeof(T).GetMethod(CreateReaderMethod(typeof(int)));

                    deserializeIL.Emit(OpCodes.Ldarg_1);//PU binary reader ,load BR on ES
                    deserializeIL.EmitCall(OpCodes.Callvirt, brReadInt, null);//PU
                    deserializeIL.Emit(OpCodes.Stloc, count);

                    deserializeIL.Emit(OpCodes.Br, loopLabelEnd);

                    deserializeIL.MarkLabel(loopLabelBegin); //begin loop 
                    deserializeIL.Emit(OpCodes.Nop);
                    deserializeIL.Emit(OpCodes.Ldarg_1);
                    deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);
                    deserializeIL.Emit(OpCodes.Stloc, key);
                    deserializeIL.Emit(OpCodes.Ldarg_1);
                    deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);
                    deserializeIL.Emit(OpCodes.Stloc, value);

                    deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
                    deserializeIL.EmitCall(OpCodes.Callvirt, getProp, null);
                    deserializeIL.Emit(OpCodes.Ldloc, key);
                    deserializeIL.Emit(OpCodes.Ldloc, value);
                    deserializeIL.EmitCall(OpCodes.Callvirt, dicAdd, null);//call add method
                    deserializeIL.Emit(OpCodes.Nop);

                    deserializeIL.Emit(OpCodes.Ldloc, count);
                    deserializeIL.Emit(OpCodes.Ldc_I4_1);
                    deserializeIL.Emit(OpCodes.Sub);
                    deserializeIL.Emit(OpCodes.Stloc, count);

                    deserializeIL.MarkLabel(loopLabelEnd); //end loop 
                    deserializeIL.Emit(OpCodes.Nop);
                    deserializeIL.Emit(OpCodes.Ldloc, count);
                    deserializeIL.Emit(OpCodes.Ldc_I4_0);
                    deserializeIL.Emit(OpCodes.Ceq);
                    deserializeIL.Emit(OpCodes.Ldc_I4_0);
                    deserializeIL.Emit(OpCodes.Ceq);
                    deserializeIL.Emit(OpCodes.Stloc_S, boolVal);
                    deserializeIL.Emit(OpCodes.Ldloc_S, boolVal);
                    deserializeIL.Emit(OpCodes.Brtrue_S, loopLabelBegin);

                    deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
                    deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent2);


                }
                #endregion

                #region List

                else if (pi.PropertyType == typeof(List<int>))
                {
                    deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);//load new obj on ES
                    deserializeIL.Emit(OpCodes.Ldarg_1);//PU binary reader ,load BR on ES
                    deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);//PU

                    //FieldBuilder dateTimeFld = typeBuilder.DefineField("Ticks ", typeof(Int64), FieldAttributes.Public);
                    ConstructorInfo ctorDtTime = typeof(List<int>).GetConstructor(new Type[] { typeof(int[]) });
                    deserializeIL.Emit(OpCodes.Newobj, ctorDtTime);//PU
                    deserializeIL.EmitCall(OpCodes.Callvirt, setProp, null);//PU

                    deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
                    deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent2);
                }
                else if (pi.PropertyType == typeof(IList<int>))
                {
                    deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);//load new obj on ES
                    deserializeIL.Emit(OpCodes.Ldarg_1);//PU binary reader ,load BR on ES
                    deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);//PU

                    //FieldBuilder dateTimeFld = typeBuilder.DefineField("Ticks ", typeof(Int64), FieldAttributes.Public);
                    ConstructorInfo ctorDtTime = typeof(List<int>).GetConstructor(new Type[] { typeof(int[]) });
                    deserializeIL.Emit(OpCodes.Newobj, ctorDtTime);//PU
                    deserializeIL.EmitCall(OpCodes.Callvirt, setProp, null);//PU

                    deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
                    deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent2);
                }

                #endregion

                #region OPCodes for other types DeSerialization
                else
                {
                    deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);//load new obj on ES
                    deserializeIL.Emit(OpCodes.Ldarg_1);//PU binary reader ,load BR on ES
                    deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);//PU

                    deserializeIL.EmitCall(OpCodes.Callvirt, setProp, null);//PU


                    deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
                    deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent2);

                }
                #endregion
            }
        }

        #endregion

        #region 读写方法

        private static string CreateReaderMethod(Type type)
        {
            if (type == typeof(Int32))
                return "ReadInt32";

            if (type == typeof(UInt32))
                return "ReadUInt32";

            if (type == typeof(UInt64))
                return "ReadUInt64";

            if (type == typeof(Int64))
                return "ReadInt64";

            if (type == typeof(char))
                return "ReadChar";

            if (type == typeof(char[]))
                return "ReadChars";

            if (type == typeof(UInt16))
                return "ReadUInt16";

            if (type == typeof(Int16))
                return "ReadInt16";

            else if (type == typeof(string))
                return "ReadString";

            else if (type == typeof(DateTime))
                return "ReadInt64";//ticks used in serialization

            else if (type == typeof(long))
                return "ReadInt64";

            else if (type == typeof(ulong))
                return "ReadUInt64";

            else if (type == typeof(bool))
                return "ReadBoolean";

            else if (type == typeof(byte))
                return "ReadByte";

            else if (type == typeof(sbyte))
                return "ReadSByte";

            else if (type == typeof(byte[]))
                return "ReadBytes";

            else if (type == typeof(decimal))
                return "ReadDecimal";

            else if (type == typeof(float))
                return "ReadSingle";

            else if (type == typeof(double))
                return "ReadDouble";

            else if (type == typeof(IDictionary))//currenly supports a string dictionary only
                return "ReadString";
            else if (type == typeof(IList))
                return "ReadString";
            else if (type == typeof(TimeSpan))
                return "ReadString";
            else if (type == typeof(DateTimeOffset))
                return "ReadString";
            else if (type == typeof(Enum))
                return "ReadString";
            else if (type == typeof(Guid))
                return "ReadString";
            else if (type == typeof(DataTable))
                return "ReadString";
            else if (type == typeof(DataSet))
                return "ReadString";
            else if (type == typeof(Hashtable))
                return "ReadString";

            
                //读取数组数据
            else if (type == typeof(bool[]) || type == typeof(List<bool>) || type == typeof(IList<bool>) || type == typeof(IEnumerable<bool>))
                return "ReadArrayBoolean";
            else if (type == typeof(byte[]) || type == typeof(List<byte>) || type == typeof(IList<byte>) || type == typeof(IEnumerable<byte>))
                return "ReadArrayInt8";
            else if (type == typeof(sbyte[]) || type == typeof(List<sbyte>) || type == typeof(IList<sbyte>) || type == typeof(IEnumerable<sbyte>))
                return "ReadArrayUInt8";
            else if (type == typeof(short[]) || type == typeof(List<short>) || type == typeof(IList<short>) || type == typeof(IEnumerable<short>))
                return "ReadArrayInt16";
            else if (type == typeof(ushort[]) || type == typeof(List<ushort>) || type == typeof(IList<ushort>) || type == typeof(IEnumerable<ushort>))
                return "ReadArrayUInt16";
            else if (type == typeof(char[]) || type == typeof(List<char>) || type == typeof(IList<char>) || type == typeof(IEnumerable<char>))
                return "ReadArrayChar";
            else if (type == typeof(int[]) || type == typeof(List<int>) || type == typeof(IList<int>) || type == typeof(IEnumerable<int>))
                return "ReadArrayInt32";
            else if (type == typeof(uint[]) || type == typeof(List<uint>) || type == typeof(IList<uint>) || type == typeof(IEnumerable<uint>))
                return "ReadArrayUInt";
            else if (type == typeof(float[]) || type == typeof(List<float>) || type == typeof(IList<float>) || type == typeof(IEnumerable<float>))
                return "ReadArrayFloat";
            else if (type == typeof(double[]) || type == typeof(List<double>) || type == typeof(IList<double>) || type == typeof(IEnumerable<double>))
                return "ReadArrayDouble";
            else if (type == typeof(decimal[]) || type == typeof(List<decimal>) || type == typeof(IList<decimal>) || type == typeof(IEnumerable<decimal>))
                return "ReadArrayDecimal";
            else if (type == typeof(DateTime[]) || type == typeof(List<DateTime>) || type == typeof(IList<DateTime>) || type == typeof(IEnumerable<DateTime>))
                return "ReadArrayDateTime";
            else if (type == typeof(TimeSpan[]) || type == typeof(List<TimeSpan>) || type == typeof(IList<TimeSpan>) || type == typeof(IEnumerable<TimeSpan>))
                return "ReadArrayTimeSpan";
            else if (type == typeof(DateTimeOffset[]) || type == typeof(List<DateTimeOffset>) || type == typeof(IList<DateTimeOffset>) || type == typeof(IEnumerable<DateTimeOffset>))
                return "ReadArrayDateTimeOffset";
            else if (type == typeof(Enum[]) || type == typeof(List<Enum>) || type == typeof(IList<Enum>) || type == typeof(IEnumerable<Enum>))
                return "ReadArrayEnum";
            else if (type == typeof(Guid[]) || type == typeof(List<Guid>) || type == typeof(IList<Guid>) || type == typeof(IEnumerable<Guid>))
                return "ReadArrayGuid";

            else if (type is object)
                return "ReadObject";
            else
                throw new Exception("类型无法解析到指定的方法");

        }

        private static MethodInfo CreateWriterMethod<T>(Type type)
        {
            string method = "Write";
            MethodInfo brWrite = null;

            //-----------------------------集合类型
            //char
            if (type == typeof(char[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(char[]) }, null);
            else if (type == typeof(List<char>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(List<char>) }, null);
            else if (type == typeof(IList<char>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<char>) }, null);

            //bool
            else if (type == typeof(bool[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(bool[]) }, null);
            else if (type == typeof(List<bool>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(List<bool>) }, null);
            else if (type == typeof(IList<bool>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<bool>) }, null);

            //byte
            else if (type == typeof(byte[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(byte[]) }, null);
            else if (type == typeof(List<byte>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(List<byte>) }, null);
            else if (type == typeof(IList<byte>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<byte>) }, null);

            //sbyte
            else if (type == typeof(sbyte[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(sbyte[]) }, null);
            else if (type == typeof(List<sbyte>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(List<sbyte>) }, null);
            else if (type == typeof(IList<sbyte>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<sbyte>) }, null);

            //short
            else if (type == typeof(short[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(short[]) }, null);
            else if (type == typeof(List<short>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(List<short>) }, null);
            else if (type == typeof(IList<short>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<short>) }, null);

            //ushort
            else if (type == typeof(ushort[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(ushort[]) }, null);
            else if (type == typeof(List<ushort>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(List<ushort>) }, null);
            else if (type == typeof(IList<ushort>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<ushort>) }, null);

            //int
            else if (type == typeof(int[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(int[]) }, null);
            else if (type == typeof(List<int>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(List<int>) }, null);
            else if (type == typeof(IList<int>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<int>) }, null);

            //uint
            else if (type == typeof(uint[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(uint[]) }, null);
            else if (type == typeof(List<uint>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(List<uint>) }, null);
            else if (type == typeof(IList<uint>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<uint>) }, null);

            //long
            else if (type == typeof(long[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(long[]) }, null);
            else if (type == typeof(List<long>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(List<long>) }, null);
            else if (type == typeof(IList<long>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<long>) }, null);

            //ulng
            else if (type == typeof(ulong[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(ulong[]) }, null);
            else if (type == typeof(List<ulong>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(List<ulong>) }, null);
            else if (type == typeof(IList<ulong>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<ulong>) }, null);

            //float
            else if (type == typeof(float[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(float[]) }, null);
            else if (type == typeof(List<float>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(List<float>) }, null);
            else if (type == typeof(IList<float>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<float>) }, null);

            //double
            else if (type == typeof(double[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(double[]) }, null);
            else if (type == typeof(List<double>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(List<double>) }, null);
            else if (type == typeof(IList<double>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<double>) }, null);

            //decimal
            else if (type == typeof(decimal[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(decimal[]) }, null);
            else if (type == typeof(List<decimal>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(List<decimal>) }, null);
            else if (type == typeof(IList<decimal>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<decimal>) }, null);

            //string
            else if (type == typeof(string[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(string[]) }, null);
            else if (type == typeof(List<string>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(List<string>) }, null);
            else if (type == typeof(IList<string>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<string>) }, null);

            //DateTime
            else if (type == typeof(DateTime[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(DateTime[]) }, null);
            else if (type == typeof(List<DateTime>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(List<DateTime>) }, null);
            else if (type == typeof(IList<DateTime>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<DateTime>) }, null);

            //DateTimeOffset
            else if (type == typeof(DateTimeOffset[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(DateTimeOffset[]) }, null);
            else if (type == typeof(List<DateTimeOffset>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(List<DateTimeOffset>) }, null);
            else if (type == typeof(IList<DateTimeOffset>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<DateTimeOffset>) }, null);

            //TimeSpan
            else if (type == typeof(TimeSpan[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(TimeSpan[]) }, null);
            else if (type == typeof(List<TimeSpan>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(List<TimeSpan>) }, null);
            else if (type == typeof(IList<TimeSpan>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<TimeSpan>) }, null);

            //Uri
            else if (type == typeof(Uri[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(Uri[]) }, null);
            else if (type == typeof(List<Uri>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(List<Uri>) }, null);
            else if (type == typeof(IList<Uri>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<Uri>) }, null);

            //Guid
            else if (type == typeof(Guid[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(Guid[]) }, null);
            else if (type == typeof(List<Guid>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(List<Guid>) }, null);
            else if (type == typeof(IList<Guid>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<Guid>) }, null);

            //Enum
            else if (type == typeof(Enum[]))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(Enum[]) }, null);
            else if (type == typeof(List<Enum>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(List<Enum>) }, null);
            else if (type == typeof(IList<Enum>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<Enum>) }, null);

            //-----------------------------词典类型
            else if (type == typeof(Dictionary<int, int>) || type == typeof(IDictionary<int, int>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<int, int>) }, null);

            else if (type == typeof(Dictionary<string, bool>) || type == typeof(IDictionary<string, bool>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, bool>) }, null);

            else if (type == typeof(Dictionary<string, byte>) || type == typeof(IDictionary<string, byte>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, byte>) }, null);

            else if (type == typeof(Dictionary<string, sbyte>) || type == typeof(IDictionary<string, sbyte>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, sbyte>) }, null);

            else if (type == typeof(Dictionary<string, char>) || type == typeof(IDictionary<string, char>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, char>) }, null);

            else if (type == typeof(Dictionary<string, short>) || type == typeof(IDictionary<string, short>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, short>) }, null);

            else if (type == typeof(Dictionary<string, ushort>) || type == typeof(IDictionary<string, ushort>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, ushort>) }, null);

            else if (type == typeof(Dictionary<string, int>) || type == typeof(IDictionary<string, int>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, int>) }, null);

            else if (type == typeof(Dictionary<string, uint>) || type == typeof(IDictionary<string, uint>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, uint>) }, null);

            else if (type == typeof(Dictionary<string, long>) || type == typeof(IDictionary<string, long>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, long>) }, null);

            else if (type == typeof(Dictionary<string, ulong>) || type == typeof(IDictionary<string, ulong>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, ulong>) }, null);

            else if (type == typeof(Dictionary<string, float>) || type == typeof(IDictionary<string, float>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, float>) }, null);

            else if (type == typeof(Dictionary<string, double>) || type == typeof(IDictionary<string, double>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, double>) }, null);

            else if (type == typeof(Dictionary<string, decimal>) || type == typeof(IDictionary<string, decimal>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, decimal>) }, null);

            else if (type == typeof(Dictionary<string, string>) || type == typeof(IDictionary<string, string>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, string>) }, null);

            else if (type == typeof(Dictionary<string, object>) || type == typeof(IDictionary<string, object>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, object>) }, null);

                //-----------------------------其它类型
            //对于纯Object类型，使用该方法
            else if (type == typeof(object))
                brWrite = typeof(T).GetMethod(method + "Object", flag, null, new Type[] { typeof(object) }, null);
            //else if (type == typeof(List<object>))
            //    brWrite = typeof(TStream).GetMethod(method, BF, null, new Type[] { typeof(object[]) }, null);
            else if (type.IsEnum)
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(int) }, null);
            else
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { type }, null);

            return brWrite;

            //return GetJsonWriterMethod<TStream>(type, "Write");

            #region old
            //MethodInfo brWrite = null;

            //if (type == typeof(DateTime))
            //    brWrite = typeof(TStream).GetMethod("Write", new Type[] { typeof(Int64) });

            //if (type == typeof(IDictionary))
            //    brWrite = typeof(TStream).GetMethod("Write", new Type[] { typeof(IDictionary) });
            //else if (type == typeof(IList))
            //    brWrite = typeof(TStream).GetMethod("Write", new Type[] { typeof(string) });
            //else if (type == typeof(List<object>))
            //    brWrite = typeof(TStream).GetMethod("Write", new Type[] { typeof(object[]) });
            //else if (type == typeof(IEnumerable))
            //    brWrite = typeof(TStream).GetMethod("Write", new Type[] { typeof(IEnumerable) });
            //else
            //    brWrite = typeof(TStream).GetMethod("Write", new Type[] { type });

            //if (type == typeof(IDictionary))
            //    brWrite = typeof(TStream).GetMethod("Write", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(IDictionary) }, new ParameterModifier[0]);
            //else if (type == typeof(IList))
            //    brWrite = typeof(TStream).GetMethod("Write", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(string) }, new ParameterModifier[0]);
            //else if (type == typeof(List<object>))
            //    brWrite = typeof(TStream).GetMethod("Write", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(object[]) }, new ParameterModifier[0]);
            //else if (type == typeof(IEnumerable))
            //    brWrite = typeof(TStream).GetMethod("Write", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(IEnumerable) }, new ParameterModifier[0]);
            //else
            //    brWrite = typeof(TStream).GetMethod("Write", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { type }, new ParameterModifier[0]);


            //MethodInfo brWrite = null;

            //if (type == typeof(IDictionary))
            //    brWrite = typeof(TStream).GetMethod("Write", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(IDictionary) }, null);
            //else if (type == typeof(IList))
            //    brWrite = typeof(TStream).GetMethod("Write", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(IList) }, null);
            ////else if (type == typeof(List<object>))
            ////    brWrite = typeof(TStream).GetMethod("Write", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(object[]) }, null);
            //else if (type == typeof(IEnumerable))
            //    brWrite = typeof(TStream).GetMethod("Write", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(IEnumerable) }, null);
            ////else if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
            ////    brWrite = typeof(TStream).GetMethod("Write", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(Nullable<>) }, null);
            //else
            //    brWrite = typeof(TStream).GetMethod("Write", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { type }, null);

            //return brWrite;
            #endregion
        }

        private static MethodInfo CreateWriterMethod<T>(Type type, string method)
        {
            MethodInfo brWrite = null;

            if (type == typeof(IList<char>) || type == typeof(List<char>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<char>) }, null);

            else if (type == typeof(IList<bool>) || type == typeof(List<bool>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<bool>) }, null);

            if (type == typeof(IList<byte>) || type == typeof(List<byte>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<byte>) }, null);

            else if (type == typeof(IList<sbyte>) || type == typeof(List<sbyte>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<sbyte>) }, null);

            if (type == typeof(IList<short>) || type == typeof(List<short>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<short>) }, null);

            else if (type == typeof(IList<ushort>) || type == typeof(List<ushort>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<ushort>) }, null);

            else if (type == typeof(IList<int>) || type == typeof(List<int>))
                //brWrite = typeof(TStream).GetMethod(method, BF, null, new Type[] { typeof(IList<int>) }, null);
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(List<int>) }, null);

            else if (type == typeof(IList<uint>) || type == typeof(List<uint>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<uint>) }, null);

            else if (type == typeof(IList<long>) || type == typeof(List<long>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<long>) }, null);

            else if (type == typeof(IList<ulong>) || type == typeof(List<ulong>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<ulong>) }, null);

            else if (type == typeof(IList<float>) || type == typeof(List<float>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<float>) }, null);

            else if (type == typeof(IList<double>) || type == typeof(List<double>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<double>) }, null);

            else if (type == typeof(IList<decimal>) || type == typeof(List<decimal>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<decimal>) }, null);

            else if (type == typeof(IList<string>) || type == typeof(List<string>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<string>) }, null);

            else if (type == typeof(IList<DateTime>) || type == typeof(List<DateTime>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<DateTime>) }, null);

            else if (type == typeof(IList<DateTimeOffset>) || type == typeof(List<DateTimeOffset>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<DateTimeOffset>) }, null);

            else if (type == typeof(IList<TimeSpan>) || type == typeof(List<TimeSpan>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<TimeSpan>) }, null);

            else if (type == typeof(IList<Uri>) || type == typeof(List<Uri>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<Uri>) }, null);

            else if (type == typeof(IList<Guid>) || type == typeof(List<Guid>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<Guid>) }, null);

            else if (type == typeof(IList<Enum>) || type == typeof(List<Enum>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IList<Enum>) }, null);



            else if (type == typeof(Dictionary<int, int>) || type == typeof(IDictionary<int, int>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<int, int>) }, null);



            else if (type == typeof(Dictionary<string, bool>) || type == typeof(IDictionary<string, bool>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, bool>) }, null);

            else if (type == typeof(Dictionary<string, byte>) || type == typeof(IDictionary<string, byte>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, byte>) }, null);

            else if (type == typeof(Dictionary<string, sbyte>) || type == typeof(IDictionary<string, sbyte>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, sbyte>) }, null);

            else if (type == typeof(Dictionary<string, char>) || type == typeof(IDictionary<string, char>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, char>) }, null);

            else if (type == typeof(Dictionary<string, short>) || type == typeof(IDictionary<string, short>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, short>) }, null);

            else if (type == typeof(Dictionary<string, ushort>) || type == typeof(IDictionary<string, ushort>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, ushort>) }, null);

            else if (type == typeof(Dictionary<string, int>) || type == typeof(IDictionary<string, int>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, int>) }, null);

            else if (type == typeof(Dictionary<string, uint>) || type == typeof(IDictionary<string, uint>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, uint>) }, null);

            else if (type == typeof(Dictionary<string, long>) || type == typeof(IDictionary<string, long>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, long>) }, null);

            else if (type == typeof(Dictionary<string, ulong>) || type == typeof(IDictionary<string, ulong>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, ulong>) }, null);

            else if (type == typeof(Dictionary<string, float>) || type == typeof(IDictionary<string, float>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, float>) }, null);

            else if (type == typeof(Dictionary<string, double>) || type == typeof(IDictionary<string, double>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, double>) }, null);

            else if (type == typeof(Dictionary<string, decimal>) || type == typeof(IDictionary<string, decimal>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, decimal>) }, null);

            else if (type == typeof(Dictionary<string, string>) || type == typeof(IDictionary<string, string>))
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(IDictionary<string, string>) }, null);

                //对于纯Object类型，使用该方法
            else if (type == typeof(object))
                brWrite = typeof(T).GetMethod(method + "Object", flag, null, new Type[] { typeof(object) }, null);
            //else if (type == typeof(List<object>))
            //    brWrite = typeof(TStream).GetMethod(method, BF, null, new Type[] { typeof(object[]) }, null);
            else if (type.IsEnum)
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { typeof(int) }, null);
            else
                brWrite = typeof(T).GetMethod(method, flag, null, new Type[] { type }, null);

            return brWrite;
        }
        
        #endregion

        internal Serialize<T> GenerateSerializationType<T>(Type type)
        {
            #region Serialize Method Builder

            DynamicMethod dynamicGet = new DynamicMethod("Serialization_" + type.Name, typeof(void), new Type[] { typeof(T), typeof(object) }, typeof(object), true);//new DynamicMethod("DynamicGet", typeof(object), new Type[] { typeof(object) }, type, true);
            ILGenerator mthdIL = dynamicGet.GetILGenerator();

            if (IsBaseType(type) == true)
            {
                SerializeJsonBaseType<T>(type, mthdIL);
                CutTail<T>(mthdIL);
            }
            else if (type.IsClass)
            {
                LocalBuilder tpmEvent = mthdIL.DeclareLocal(type);
                mthdIL.Emit(OpCodes.Nop);
                mthdIL.Emit(OpCodes.Ldarg_1);//PU
                mthdIL.Emit(OpCodes.Castclass, type);//PU
                mthdIL.Emit(OpCodes.Stloc, tpmEvent);//PP

                SerializeFields<T>(type, mthdIL, tpmEvent);
                SerializePropertys<T>(type, mthdIL, tpmEvent);
            }
            //目前还无法处理值类型
            else if (type.BaseType == typeof(ValueType))
            {
                LocalBuilder tpmEvent = mthdIL.DeclareLocal(type);
                mthdIL.Emit(OpCodes.Nop);
                mthdIL.Emit(OpCodes.Ldarg_1);//PU
                mthdIL.Emit(OpCodes.Castclass, type);//PU
                mthdIL.Emit(OpCodes.Stloc, tpmEvent);//PP

                SerializeValueFields<T>(type, mthdIL, tpmEvent);
                SerializeValuePropertys<T>(type, mthdIL, tpmEvent);
            }
            else
            {
                SerializeJsonBaseType<T>(type, mthdIL);
                CutTail<T>(mthdIL);
            }

            #region old

            //if (type.IsClass)
            //{
            //    #region old
            //    //LocalBuilder tpmEvent = mthdIL.DeclareLocal(type);

            //    //mthdIL.Emit(OpCodes.Nop);
            //    //mthdIL.Emit(OpCodes.Ldarg_2);//PU
            //    //mthdIL.Emit(OpCodes.Castclass, type);//PU
            //    //mthdIL.Emit(OpCodes.Stloc, tpmEvent);//PP

            //    ////SerializeJsonFields<TStream>(type, mthdIL, tpmEvent);
            //    ////SerializeJsonPropertys<TStream>(type, mthdIL, tpmEvent);

            //    //foreach (FieldInfo fi in type.GetFields())
            //    //{
            //    //    MethodInfo brWrite = GetJsonWriterMethod<JsonStreamTest>(fi.FieldType);
            //    //    mthdIL.Emit(OpCodes.Ldarg_1);//PU binary writer
            //    //    mthdIL.Emit(OpCodes.Ldloc, tpmEvent);//PU binary writer

            //    //    mthdIL.Emit(OpCodes.Ldfld, fi);
            //    //    mthdIL.EmitCall(OpCodes.Call, brWrite, null);//PU

            //    //    mthdIL.Emit(OpCodes.Nop);



            //    //    //MethodInfo brWrite = GetJsonWriterMethod<JsonStreamTest>(fi.FieldType);
            //    //    //var lv = mthdIL.DeclareLocal(type);
            //    //    //mthdIL.Emit(OpCodes.Ldarg_0);
            //    //    //mthdIL.Emit(OpCodes.Unbox_Any, type);
            //    //    //mthdIL.Emit(OpCodes.Stloc_0);
            //    //    //mthdIL.Emit(OpCodes.Ldloca_S, lv);
            //    //    //mthdIL.Emit(OpCodes.Ldfld, fi);
            //    //    //if (fi.FieldType.IsValueType)
            //    //    //    mthdIL.Emit(OpCodes.Box, fi.FieldType);
            //    //    //mthdIL.Emit(OpCodes.Ldfld, fi);
            //    //    //mthdIL.EmitCall(OpCodes.Call, brWrite, null);//PU

            //    //    //MethodInfo brWrite = GetJsonWriterMethod<JsonStreamTest>(fi.FieldType);
            //    //    //var lv = mthdIL.DeclareLocal(type);
            //    //    //mthdIL.Emit(OpCodes.Ldarg_1);
            //    //    //mthdIL.Emit(OpCodes.Unbox_Any, type);
            //    //    //mthdIL.Emit(OpCodes.Stloc_1);
            //    //    //mthdIL.Emit(OpCodes.Ldloca_S, lv);
            //    //    //mthdIL.Emit(OpCodes.Ldfld, fi);
            //    //    //if (fi.FieldType.IsValueType)
            //    //    //    mthdIL.Emit(OpCodes.Box, fi.FieldType);
            //    //    //mthdIL.Emit(OpCodes.Ldfld, fi);
            //    //    //mthdIL.EmitCall(OpCodes.Call, brWrite, null);//PU

            //    //}
            //    #endregion

            //    if (type.GetInterface("IList") == typeof(IList) ||
            //        type.GetInterface("IDictionary") == typeof(IDictionary) ||
            //        type.GetInterface("ICollection") == typeof(ICollection) ||
            //        type.GetInterface("IEnumerable") == typeof(IEnumerable))
            //    {
            //        WriteUnFlag<TStream>(mthdIL);
            //        SerializeUnPackage<TStream>(type, mthdIL);
            //    }
            //    else
            //    {
            //        LocalBuilder tpmEvent = mthdIL.DeclareLocal(type);
            //        mthdIL.Emit(OpCodes.Nop);
            //        mthdIL.Emit(OpCodes.Ldarg_1);//PU
            //        mthdIL.Emit(OpCodes.Castclass, type);//PU
            //        mthdIL.Emit(OpCodes.Stloc, tpmEvent);//PP

            //        SerializeFields<TStream>(type, mthdIL, tpmEvent);
            //        SerializePropertys<TStream>(type, mthdIL, tpmEvent);
            //    }
            //}
            //    //目前还无法处理值类型
            //else if (type.BaseType ==typeof(ValueType))
            //{
            //    if (type.IsPrimitive)
            //    {
            //        WriteUnFlag<TStream>(mthdIL);
            //        SerializeUnPackage<TStream>(type, mthdIL);
            //    }
            //    else
            //    {
            //        //LocalBuilder tpmEvent = mthdIL.DeclareLocal(type);
            //        //mthdIL.Emit(OpCodes.Nop);
            //        //mthdIL.Emit(OpCodes.Ldarg_1);//PU
            //        //mthdIL.Emit(OpCodes.Castclass, type);//PU
            //        //mthdIL.Emit(OpCodes.Stloc, tpmEvent);//PP

            //        //SerializeFields<TStream>(type, mthdIL, tpmEvent);
            //        //SerializePropertys<TStream>(type, mthdIL, tpmEvent);


            //        LocalBuilder tpmEvent = mthdIL.DeclareLocal(type);
            //        mthdIL.Emit(OpCodes.Nop);
            //        mthdIL.Emit(OpCodes.Ldarg_1);//PU
            //        mthdIL.Emit(OpCodes.Castclass, type);//PU
            //        mthdIL.Emit(OpCodes.Stloc, tpmEvent);//PP

            //        SerializeValueFields<TStream>(type, mthdIL, tpmEvent);
            //        SerializeValuePropertys<TStream>(type, mthdIL, tpmEvent);
            //    }
            //}
            //else
            //{
            //    WriteUnFlag<TStream>(mthdIL);
            //    SerializeUnPackage<TStream>(type, mthdIL);
            //}

            #endregion

            #endregion

            return (Serialize<T>)dynamicGet.CreateDelegate(typeof(Serialize<T>));
        }

        internal Deserialize<T> GenerateDeserializationType<T>(Type type)
        {
            #region Serialize Method Builder

            DynamicMethod dynamicGet = new DynamicMethod("Deserialization_" + type.Name, type, new Type[] { typeof(T) }, typeof(object), true);
            ILGenerator deserializeIL = dynamicGet.GetILGenerator();

            if (IsBaseType(type) == true)
            {
                //LocalBuilder tpmRetEvent = deserializeIL.DeclareLocal(type);
                //LocalBuilder tpmRetEvent2 = deserializeIL.DeclareLocal(type);
                Label ret = deserializeIL.DefineLabel();
                DeserializeJsonBaseType<T>(type, deserializeIL);
                deserializeIL.Emit(OpCodes.Br_S, ret);
                deserializeIL.MarkLabel(ret);
                //deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent2);
                deserializeIL.Emit(OpCodes.Ret);
                return (Deserialize<T>)dynamicGet.CreateDelegate(typeof(Deserialize<T>));
            }
            else if (type.IsClass)
            {
                if (type.GetInterface("IList") == typeof(IList) ||
                    type.GetInterface("IDictionary") == typeof(IDictionary) ||
                    type.GetInterface("ICollection") == typeof(ICollection) ||
                    type.GetInterface("IEnumerable") == typeof(IEnumerable))
                {
                    //WriteUnFlag<TStream>(deserializeIL);
                    //SerializeJsonBaseType<TStream>(type, deserializeIL);
                }
                else
                {
                    LocalBuilder tpmRetEvent = deserializeIL.DeclareLocal(type);
                    LocalBuilder tpmRetEvent2 = deserializeIL.DeclareLocal(type);
                    Label ret = deserializeIL.DefineLabel();

                    ConstructorInfo ctorEvent = type.GetConstructor(new Type[0]);
                    if (ctorEvent == null)
                        throw new Exception("The event class is missing a default constructor with 0 params");

                    deserializeIL.Emit(OpCodes.Newobj, ctorEvent);
                    deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent);


                    DeserializeFields<T>(type, deserializeIL, tpmRetEvent, tpmRetEvent2);
                    //DeserializeJsonPropertys<TStream>(type, deserializeIL, tpmRetEvent, tpmRetEvent2);

                    deserializeIL.Emit(OpCodes.Br_S, ret);
                    deserializeIL.MarkLabel(ret);
                    deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent2);
                    deserializeIL.Emit(OpCodes.Ret);
                }

                return (Deserialize<T>)dynamicGet.CreateDelegate(typeof(Deserialize<T>));
            }
            //目前还无法处理值类型
            else if (type.BaseType == typeof(ValueType))
            {
                //if (type.IsPrimitive)
                //{
                //    WriteUnFlag<TStream>(deserializeIL);
                //    SerializeUnPackage<TStream>(type, deserializeIL);
                //}
                //else
                //{
                //    //LocalBuilder tpmEvent = deserializeIL.DeclareLocal(type);
                //    //deserializeIL.Emit(OpCodes.Nop);
                //    //deserializeIL.Emit(OpCodes.Ldarg_1);//PU
                //    //deserializeIL.Emit(OpCodes.Castclass, type);//PU
                //    //deserializeIL.Emit(OpCodes.Stloc, tpmEvent);//PP

                //    //SerializeFields<TStream>(type, deserializeIL, tpmEvent);
                //    //SerializePropertys<TStream>(type, deserializeIL, tpmEvent);
                //}


                //deserializeIL.Emit(OpCodes.Ldobj);
                //deserializeIL.Emit(OpCodes.Stloc_0);
                //deserializeIL.Emit(OpCodes.Ldloc_0);
                //deserializeIL.Emit(OpCodes.Ret);
            }
            else
            {
                //WriteUnFlag<TStream>(deserializeIL);
                //SerializeUnPackage<TStream>(type, deserializeIL);
            }

            #endregion

            return null;
            //return (Deserialize<TStream>)dynamicGet.CreateDelegate(typeof(Deserialize<TStream>));
            
        }



        private void SerializeValueFields<T>(Type type, ILGenerator mthdIL, LocalBuilder tpmEvent)
        {
            foreach (FieldInfo info in type.GetFields())
            {
                if (Utils.IsIgnoreAttribute(info))
                    continue;

                Type ctype = info.FieldType;
                MethodInfo brWrite = CreateWriterMethod<T>(ctype);
                mthdIL.Emit(OpCodes.Ldarg_0);//PU binary writer
                mthdIL.Emit(OpCodes.Ldloc, tpmEvent);//PU binary writer

                bool isExplicit = false;
                //不是期待的类型
                if (
                    IsExplicitList(ctype) == true ||
                    IsExplicitEnumerable(ctype) == true ||
                    IsExplicitDictionary(ctype) == true)
                {
                    isExplicit = true;
                }
                if (isExplicit == false)
                {
                    if (Utils.IsJsonBaseType(type) == false)
                    {
                        if (ctype.GetInterface("IList") == typeof(IList) || ctype == typeof(IList))
                            brWrite = CreateWriterMethod<T>(typeof(IList));
                        else if (ctype.GetInterface("IDictionary") == typeof(IDictionary) || ctype == typeof(IDictionary))
                            brWrite = CreateWriterMethod<T>(typeof(IDictionary));
                        else if (ctype.GetInterface("IEnumerable") == typeof(IEnumerable) || ctype == typeof(IEnumerable))
                            brWrite = CreateWriterMethod<T>(typeof(IEnumerable));
                    }
                }

                LocalBuilder dictionaryEntry = mthdIL.DeclareLocal(type);
                mthdIL.Emit(OpCodes.Unbox_Any, type);
                mthdIL.Emit(OpCodes.Stloc, dictionaryEntry);// save the DictionaryEntry
                
                mthdIL.Emit(OpCodes.Nop);
                mthdIL.Emit(OpCodes.Ldarg_0);//PU binary writer
                mthdIL.Emit(OpCodes.Ldloca_S, dictionaryEntry);//load the DictionaryEntry
                mthdIL.Emit(OpCodes.Ldfld, info);
                mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU call binary writer write
            }
        }

        private static void SerializeValuePropertys<T>(Type type, ILGenerator mthdIL, LocalBuilder tpmEvent)
        {
            foreach (PropertyInfo info in type.GetProperties())
            {
                if (Utils.IsIgnoreAttribute(info))
                    continue;

                Type ctype = info.PropertyType;
                MethodInfo brWrite = CreateWriterMethod<T>(ctype);
                mthdIL.Emit(OpCodes.Ldarg_0);//PU binary writer
                mthdIL.Emit(OpCodes.Ldloc, tpmEvent);//PU binary writer

                bool isExplicit = false;
                //不是期待的类型
                if (
                    IsExplicitList(ctype) == true ||
                    IsExplicitEnumerable(ctype) == true ||
                    IsExplicitDictionary(ctype) == true)
                {
                    isExplicit = true;
                }
                if (isExplicit == false)
                {
                    if (Utils.IsJsonBaseType(type) == false)
                    {
                        if (ctype.GetInterface("IList") == typeof(IList) || ctype == typeof(IList))
                            brWrite = CreateWriterMethod<T>(typeof(IList));
                        else if (ctype.GetInterface("IDictionary") == typeof(IDictionary) || ctype == typeof(IDictionary))
                            brWrite = CreateWriterMethod<T>(typeof(IDictionary));
                        else if (ctype.GetInterface("IEnumerable") == typeof(IEnumerable) || ctype == typeof(IEnumerable))
                            brWrite = CreateWriterMethod<T>(typeof(IEnumerable));
                    }
                }

                MethodInfo get_Key = type.GetMethod("get_" + info.Name, new Type[0]);
                LocalBuilder dictionaryEntry = mthdIL.DeclareLocal(type);

                mthdIL.Emit(OpCodes.Unbox_Any, type);
                mthdIL.Emit(OpCodes.Stloc, dictionaryEntry);// save the DictionaryEntry
                mthdIL.Emit(OpCodes.Nop);
                mthdIL.Emit(OpCodes.Ldarg_0);//PU binary writer
                mthdIL.Emit(OpCodes.Ldloca_S, dictionaryEntry);//load the DictionaryEntry
                mthdIL.EmitCall(OpCodes.Call, get_Key, null);// call get_Key
                mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU call binary writer write
            }
        }

        private static void SerializeFields<T>(Type type, ILGenerator mthdIL, LocalBuilder tpmEvent)
        {
            //字段
            foreach (FieldInfo info in type.GetFields())
            {
                if (Utils.IsIgnoreAttribute(info))
                    continue;

                Type ctype = info.FieldType;
                MethodInfo brWrite = CreateWriterMethod<T>(ctype);
                mthdIL.Emit(OpCodes.Ldarg_0);//PU binary writer
                mthdIL.Emit(OpCodes.Ldloc, tpmEvent);//PU binary writer

                bool isExplicit = false;
                //不是期待的类型
                if (
                    IsExplicitList(ctype) == true ||
                    IsExplicitEnumerable(ctype) == true ||
                    IsExplicitDictionary(ctype) == true)
                {
                    isExplicit = true;
                }
                if (isExplicit == false)
                {
                    if (Utils.IsJsonBaseType(ctype) == false)
                    {
                        if (ctype.GetInterface("IList") == typeof(IList) || ctype == typeof(IList))
                            brWrite = CreateWriterMethod<T>(typeof(IList));
                        else if (ctype.GetInterface("IDictionary") == typeof(IDictionary) || ctype == typeof(IDictionary))
                            brWrite = CreateWriterMethod<T>(typeof(IDictionary));
                        else if (ctype.GetInterface("IEnumerable") == typeof(IEnumerable) || ctype == typeof(IEnumerable))
                            brWrite = CreateWriterMethod<T>(typeof(IEnumerable));
                    }
                }
                mthdIL.Emit(OpCodes.Ldfld, info);
                mthdIL.EmitCall(OpCodes.Call, brWrite, null);//PU
            }
        }

        private static void SerializePropertys<T>(Type type, ILGenerator mthdIL, LocalBuilder tpmEvent)
        {
            bool isize = typeof(T).GetInterface("ISize") == typeof(ISize) ? true : false;
            //属性
            foreach (PropertyInfo info in type.GetProperties())
            {
                if (Utils.IsIgnoreAttribute(info))
                    continue;

                Type ctype = info.PropertyType;
                if (isize && Utils.IsJsonBaseType(ctype) == true)
                    continue;

                MethodInfo brWrite = CreateWriterMethod<T>(ctype);
                MethodInfo mi = type.GetMethod("get_" + info.Name);
                mthdIL.Emit(OpCodes.Ldarg_0);//PU binary writer
                mthdIL.Emit(OpCodes.Ldloc, tpmEvent);//PU load the event object
                mthdIL.EmitCall(OpCodes.Callvirt, mi, null);//PU get the value of the proprty

                bool isExplicit = false;
                //不是期待的类型
                if (
                    IsExplicitList(ctype) == true ||
                    IsExplicitEnumerable(ctype) == true || 
                    IsExplicitDictionary(ctype) == true)
                {
                    isExplicit = true;
                }
                if (isExplicit == false)
                {
                    if (Utils.IsJsonBaseType(type) == false)
                    {
                        if (ctype.GetInterface("IList") == typeof(IList) || ctype == typeof(IList))
                            brWrite = CreateWriterMethod<T>(typeof(IList));
                        else if (ctype.GetInterface("IDictionary") == typeof(IDictionary) || ctype == typeof(IDictionary))
                            brWrite = CreateWriterMethod<T>(typeof(IDictionary));
                        else if (ctype.GetInterface("IEnumerable") == typeof(IEnumerable) || ctype == typeof(IEnumerable))
                            brWrite = CreateWriterMethod<T>(typeof(IEnumerable));
                    }
                }
                mthdIL.EmitCall(OpCodes.Call, brWrite, null);//PU
            }
        }



        private static void DeserializeFields<T>(Type type, ILGenerator deserializeIL, LocalBuilder tpmRetEvent, LocalBuilder tpmRetEvent2)
        {
            foreach (FieldInfo fi in type.GetFields())
            {
                if (fi.FieldType == typeof(string[]))
                    continue;
                if (IsIgnoreAttribute(fi.GetCustomAttributes(true)))
                    continue;

                LocalBuilder locTyp = deserializeIL.DeclareLocal(fi.FieldType);
                //MethodInfo brRead = typeof(TStream).GetMethod(GetJsonReaderMethod(fi.FieldType));
                MethodInfo brRead = typeof(T).GetMethod(CreateReaderMethod(fi.FieldType), flag, null, new Type[] { }, null);

                //FieldInfo fld = fldMap[fi.Name];
                FieldInfo fld = fi;


                #region OPCodes for DateTime DeSerialization

                if (fi.FieldType == typeof(DateTime))
                {
                    ConstructorInfo ctorDtTime = typeof(DateTime).GetConstructor(new Type[] { typeof(Int64) });

                    deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);//load new obj on ES
                    deserializeIL.Emit(OpCodes.Ldarg_1);//PU binary reader ,load BR on ES

                    deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);//PU
                    deserializeIL.Emit(OpCodes.Newobj, ctorDtTime);//PU
                    deserializeIL.Emit(OpCodes.Stfld, fld);//PU
                    deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
                    deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent2);

                }

                #endregion

                #region OPCodes for IDictionary DeSerialization

                else if (fi.FieldType == typeof(IDictionary))
                {
                    Label loopLabelBegin = deserializeIL.DefineLabel();
                    Label loopLabelEnd = deserializeIL.DefineLabel();

                    LocalBuilder count = deserializeIL.DeclareLocal(typeof(Int32));
                    LocalBuilder key = deserializeIL.DeclareLocal(typeof(string));
                    LocalBuilder value = deserializeIL.DeclareLocal(typeof(string));
                    LocalBuilder boolVal = deserializeIL.DeclareLocal(typeof(bool));

                    MethodInfo toString = typeof(object).GetMethod("ToString", new Type[0]);
                    MethodInfo dicAdd = typeof(IDictionary).GetMethod("Add", new Type[] { typeof(object), typeof(object) });
                    MethodInfo brReadInt = typeof(T).GetMethod(CreateReaderMethod(typeof(int)));

                    deserializeIL.Emit(OpCodes.Ldarg_1);//PU binary reader ,load BR on ES
                    deserializeIL.EmitCall(OpCodes.Callvirt, brReadInt, null);//PU
                    deserializeIL.Emit(OpCodes.Stloc, count);

                    deserializeIL.Emit(OpCodes.Br, loopLabelEnd);

                    deserializeIL.MarkLabel(loopLabelBegin); //begin loop 
                    deserializeIL.Emit(OpCodes.Nop);
                    deserializeIL.Emit(OpCodes.Ldarg_1);
                    deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);
                    deserializeIL.Emit(OpCodes.Stloc, key);
                    deserializeIL.Emit(OpCodes.Ldarg_1);
                    deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);
                    deserializeIL.Emit(OpCodes.Stloc, value);

                    deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
                    //deserializeIL.EmitCall(OpCodes.Callvirt, getProp, null);
                    deserializeIL.Emit(OpCodes.Ldfld, fld);
                    deserializeIL.Emit(OpCodes.Ldloc, key);
                    deserializeIL.Emit(OpCodes.Ldloc, value);
                    deserializeIL.EmitCall(OpCodes.Callvirt, dicAdd, null);//call add method
                    deserializeIL.Emit(OpCodes.Nop);

                    deserializeIL.Emit(OpCodes.Ldloc, count);
                    deserializeIL.Emit(OpCodes.Ldc_I4_1);
                    deserializeIL.Emit(OpCodes.Sub);
                    deserializeIL.Emit(OpCodes.Stloc, count);

                    deserializeIL.MarkLabel(loopLabelEnd); //end loop 
                    deserializeIL.Emit(OpCodes.Nop);
                    deserializeIL.Emit(OpCodes.Ldloc, count);
                    deserializeIL.Emit(OpCodes.Ldc_I4_0);
                    deserializeIL.Emit(OpCodes.Ceq);
                    deserializeIL.Emit(OpCodes.Ldc_I4_0);
                    deserializeIL.Emit(OpCodes.Ceq);
                    deserializeIL.Emit(OpCodes.Stloc_S, boolVal);
                    deserializeIL.Emit(OpCodes.Ldloc_S, boolVal);
                    deserializeIL.Emit(OpCodes.Brtrue_S, loopLabelBegin);

                    deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
                    deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent2);
                }
                #endregion

                #region OPCodes for other types DeSerialization

                else
                {
                    //deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);//load new obj on ES
                    //deserializeIL.Emit(OpCodes.Ldarg_1);//PU binary reader ,load BR on ES

                    //deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);//PU
                    //deserializeIL.Emit(OpCodes.Stfld, fld);//PU
                    //deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
                    //deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent2);


                    deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);//load new obj on ES
                    deserializeIL.Emit(OpCodes.Ldarg_0);//PU binary reader ,load BR on ES

                    deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);//PU
                    deserializeIL.Emit(OpCodes.Stfld, fld);//PU
                    deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
                    deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent2);
                }

                #endregion

            }
        }

        /// <summary>
        /// 序列化没有名称的类型，没有经过包装的类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="mthdIL"></param>
        private static void SerializeJsonBaseType<T>(Type type, ILGenerator mthdIL)
        {
            //MethodInfo brWrite = GetJsonWriterMethod<TStream>(type, "Write");
            MethodInfo brWrite = CreateWriterMethod<T>(type);
            mthdIL.Emit(OpCodes.Ldarg_0);
            mthdIL.Emit(OpCodes.Ldarg_1);
            if (type.IsValueType)
                mthdIL.Emit(OpCodes.Unbox_Any, type);
            mthdIL.EmitCall(OpCodes.Call, brWrite, null);
            mthdIL.Emit(OpCodes.Ret);
        }

        private static void DeserializeJsonBaseType<T>(Type type, ILGenerator deserializeIL)
        {
            //MethodInfo brRead = typeof(TStream).GetMethod(GetJsonReaderMethod(type), BF, null, new Type[] { }, null);
            //deserializeIL.Emit(OpCodes.Ldarg_0);//PU binary reader ,load BR on ES
            //deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);//PU
            //deserializeIL.Emit(OpCodes.Ret);

            MethodInfo brRead = typeof(T).GetMethod(CreateReaderMethod(type), flag, null, new Type[] { }, null);
            ConstructorInfo ctorDtTime = null;
            if (type == typeof(int))
            {
                //ctorDtTime = type.GetConstructor(new Type[0]);

                deserializeIL.Emit(OpCodes.Ldarg_0);//PU binary reader ,load BR on ES
                deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);//PU
                deserializeIL.Emit(OpCodes.Initobj);//PU
            }
            //ConstructorInfo ctorDtTime = typeof(DateTime).GetConstructor(new Type[] { typeof(Int64) });
            
            //deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);//load new obj on ES
            //deserializeIL.Emit(OpCodes.Ldarg_0);//PU binary reader ,load BR on ES

            //deserializeIL.EmitCall(OpCodes.Callvirt, brRead, null);//PU
            //deserializeIL.Emit(OpCodes.Newobj, ctorDtTime);//PU
            //deserializeIL.Emit(OpCodes.Stfld, fld);//PU
            //deserializeIL.Emit(OpCodes.Ldloc, tpmRetEvent);
            //deserializeIL.Emit(OpCodes.Stloc, tpmRetEvent2);
        }

        /// <summary>
        /// 可确定的List类型
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static bool IsExplicitList(Type t)
        {
            bool isExplicit = false;
            //可能是List及相关
            if (
                    t == typeof(List<byte>) || t == typeof(IList<byte>) || t == typeof(ArraySegment<byte>) ||
                    t == typeof(List<sbyte>) || t == typeof(IList<sbyte>) || t == typeof(ArraySegment<sbyte>) ||
                    t == typeof(List<short>) || t == typeof(IList<short>) || t == typeof(ArraySegment<short>) ||
                    t == typeof(List<ushort>) || t == typeof(IList<ushort>) || t == typeof(ArraySegment<ushort>) ||
                    t == typeof(List<int>) || t == typeof(IList<int>) || t == typeof(ArraySegment<int>) ||
                    t == typeof(List<uint>) || t == typeof(IList<uint>) || t == typeof(ArraySegment<uint>) ||
                    t == typeof(List<long>) || t == typeof(IList<long>) || t == typeof(ArraySegment<long>) ||
                    t == typeof(List<ulong>) || t == typeof(IList<ulong>) || t == typeof(ArraySegment<ulong>) ||
                    t == typeof(List<float>) || t == typeof(IList<float>) || t == typeof(ArraySegment<float>) ||
                    t == typeof(List<double>) || t == typeof(IList<double>) || t == typeof(ArraySegment<double>) ||
                    t == typeof(List<decimal>) || t == typeof(IList<decimal>) || t == typeof(ArraySegment<decimal>) ||
                    t == typeof(List<bool>) || t == typeof(IList<bool>) || t == typeof(ArraySegment<bool>) ||
                    t == typeof(List<DateTime>) || t == typeof(IList<DateTime>) || t == typeof(ArraySegment<DateTime>) ||
                    //t == typeof(List<Enum>) || t == typeof(IList<Enum>) || t == typeof(ArraySegment<Enum>) ||
                    t == typeof(List<Guid>) || t == typeof(IList<Guid>) || t == typeof(ArraySegment<Guid>) ||
                    t == typeof(List<TimeSpan>) || t == typeof(IList<TimeSpan>) || t == typeof(ArraySegment<TimeSpan>) ||
                    t == typeof(List<DateTimeOffset>) || t == typeof(IList<DateTimeOffset>) || t == typeof(ArraySegment<DateTimeOffset>) ||
                    t == typeof(List<Uri>) || t == typeof(IList<Uri>) || t == typeof(ArraySegment<Uri>) ||
                    t == typeof(List<string>) || t == typeof(IList<string>) || t == typeof(ArraySegment<string>) ||

                    t == typeof(string) || t == typeof(string[]) ||
                    t == typeof(byte[]) ||
                    t == typeof(sbyte[]) ||
                    t == typeof(short[]) ||
                    t == typeof(ushort[]) ||
                    t == typeof(int[]) ||
                    t == typeof(uint[]) ||
                    t == typeof(long[]) ||
                    t == typeof(ulong[]) ||
                    t == typeof(float[]) ||
                    t == typeof(double[]) ||
                    t == typeof(decimal[]) ||
                    t == typeof(bool[]) ||
                    t == typeof(DateTime[]) ||
                    t == typeof(DateTimeOffset[]) ||
                    t == typeof(Guid[]) ||
                    //t == typeof(Enum[]) ||
                    t == typeof(Uri[]) ||
                    t == typeof(TimeSpan[])
               )
            {
                isExplicit = true;
            }
            return isExplicit;

        }

        /// <summary>
        /// 可确定的List类型
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static bool IsExplicitDictionary(Type t)
        {
            bool isExplicit = false;
            //可能是IDictionary及相关
            if (
                 t == typeof(IDictionary<string, bool>) || t == typeof(Dictionary<string, bool>) ||
                 t == typeof(IDictionary<string, char>) || t == typeof(Dictionary<string, char>) ||
                 t == typeof(IDictionary<string, byte>) || t == typeof(Dictionary<string, byte>) ||
                 t == typeof(IDictionary<string, sbyte>) || t == typeof(Dictionary<string, sbyte>) ||
                 t == typeof(IDictionary<string, short>) || t == typeof(Dictionary<string, short>) ||
                 t == typeof(IDictionary<string, ushort>) || t == typeof(Dictionary<string, ushort>) ||
                 t == typeof(IDictionary<string, int>) || t == typeof(Dictionary<string, int>) ||
                 t == typeof(IDictionary<string, uint>) || t == typeof(Dictionary<string, uint>) ||
                 t == typeof(IDictionary<string, long>) || t == typeof(Dictionary<string, long>) ||
                 t == typeof(IDictionary<string, ulong>) || t == typeof(Dictionary<string, ulong>) ||
                 t == typeof(IDictionary<string, float>) || t == typeof(Dictionary<string, float>) ||
                 t == typeof(IDictionary<string, double>) || t == typeof(Dictionary<string, double>) ||
                 t == typeof(IDictionary<string, decimal>) || t == typeof(Dictionary<string, decimal>) ||
                 t == typeof(IDictionary<string, string>) || t == typeof(Dictionary<string, string>)
               )
            {
                isExplicit = true;
            }
            return isExplicit;
        }

        /// <summary>
        /// 可确定的枚举类型
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static bool IsExplicitEnumerable(Type t)
        {
            bool isExplicit = false;
            if (
                    t == typeof(string) ||
                    t == typeof(string[]) ||
                    t == typeof(byte[]) ||
                    t == typeof(sbyte[]) ||
                    t == typeof(short[]) ||
                    t == typeof(ushort[]) ||
                    t == typeof(int[]) ||
                    t == typeof(uint[]) ||
                    t == typeof(long[]) ||
                    t == typeof(ulong[]) ||
                    t == typeof(float[]) ||
                    t == typeof(double[]) ||
                    t == typeof(decimal[]) ||
                    t == typeof(bool[]) ||
                    t == typeof(DateTime[]) ||
                    t == typeof(DateTimeOffset[]) ||
                    t == typeof(Guid[]) ||
                    t == typeof(Enum[]) ||
                    t == typeof(Uri[]) ||
                    t == typeof(TimeSpan[])
                 )
            {
                isExplicit = true;
            }
            return isExplicit;
        }

        private static void WriteUnFlag<T>(ILGenerator mthdIL)
        {
            //该写入的值不会被写入
            //MethodInfo brWrite = typeof(TStream).GetMethod("WriteUnFlag", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[0], null);
            //mthdIL.Emit(OpCodes.Ldarg_0);
            ////mthdIL.Emit(OpCodes.Ldarg_1);
            //mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);
            //mthdIL.Emit(OpCodes.Ret);


            ////该写的值会被写入，但状态不会被记录
            //MethodInfo brWrite = typeof(TStream).GetMethod("WriteUnFlag", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[0], null);
            //mthdIL.Emit(OpCodes.Ldarg_0);
            ////mthdIL.Emit(OpCodes.Ldarg_1);
            //mthdIL.EmitCall(OpCodes.Call, brWrite, null);
            ////mthdIL.Emit(OpCodes.Ret);



            //该写的值会被写入，但状态不会被记录(最终版)
            MethodInfo brWrite = typeof(T).GetMethod("WriteUnFlag", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[0], null);
            mthdIL.Emit(OpCodes.Ldarg_0);
            mthdIL.EmitCall(OpCodes.Call, brWrite, null);
        }

        private static void CutTail<T>(ILGenerator mthdIL)
        {
            ////该写的值会被写入，但状态不会被记录(最终版)
            //MethodInfo brWrite = typeof(TStream).GetMethod("CutTail", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[0], null);
            //mthdIL.Emit(OpCodes.Ldarg_0);
            //mthdIL.EmitCall(OpCodes.Call, brWrite, null);
        }

        /// <summary>
        /// 默认原生支持的数据类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal bool IsBaseType(Type type)
        {
            if (type.GetInterface("IList") == typeof(IList) ||
                    type.GetInterface("IDictionary") == typeof(IDictionary) ||
                    type.GetInterface("ICollection") == typeof(ICollection))
                return true;
            if (type.IsPrimitive)
                return true;

            if (type == typeof(decimal))
                return true;

            if (type == typeof(ArraySegment<bool>))
                return true;
            if (type == typeof(ArraySegment<byte>))
                return true;
            if (type == typeof(ArraySegment<sbyte>))
                return true;
            if (type == typeof(ArraySegment<short>))
                return true;
            if (type == typeof(ArraySegment<ushort>))
                return true;
            if (type == typeof(ArraySegment<int>))
                return true;
            if (type == typeof(ArraySegment<uint>))
                return true;
            if (type == typeof(ArraySegment<long>))
                return true;
            if (type == typeof(ArraySegment<ulong>))
                return true;
            if (type == typeof(ArraySegment<float>))
                return true;
            if (type == typeof(ArraySegment<double>))
                return true;
            if (type == typeof(ArraySegment<decimal>))
                return true;
            if (type == typeof(ArraySegment<char>))
                return true;
            if (type == typeof(ArraySegment<DateTime>))
                return true;
            if (type == typeof(ArraySegment<DateTimeOffset>))
                return true;
            if (type == typeof(ArraySegment<TimeSpan>))
                return true;
            if (type == typeof(ArraySegment<Uri>))
                return true;
            if (type == typeof(ArraySegment<Guid>))
                return true;

            if (type == typeof(Uri))
                return true;
            if (type == typeof(Enum))
                return true;
            if (type == typeof(DateTime))
                return true;
            if (type == typeof(TimeSpan))
                return true;
            if (type == typeof(DateTimeOffset))
                return true;
            if (type == typeof(Guid))
                return true;
            if (type == typeof(DataTable))
                return true;
            if (type == typeof(DataSet))
                return true;
            return false;
        }

    }
}
