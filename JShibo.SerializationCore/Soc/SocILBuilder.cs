using JShibo.Serialization.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace JShibo.Serialization.Soc
{
    internal class SocILBuilder : IBuilder
    {


        internal override Serialize<T> GenerateSerializationType<T>(Type type)
        {
            return base.GenerateSerializationType<T>(type);

            #region Serialize Method Builder

            DynamicMethod dynamicGet = new DynamicMethod("Serialization_" + type.Name, typeof(void), new Type[] { typeof(T), typeof(object) }, typeof(object), true);//new DynamicMethod("DynamicGet", typeof(object), new Type[] { typeof(object) }, type, true);
            ILGenerator mthdIL = dynamicGet.GetILGenerator();

            if (type.IsClass)
            {
                LocalBuilder tpmEvent = mthdIL.DeclareLocal(type);
                mthdIL.Emit(OpCodes.Nop);
                //mthdIL.Emit(OpCodes.Ldarg_0);//PU
                mthdIL.Emit(OpCodes.Ldarg_1);//PU
                mthdIL.Emit(OpCodes.Castclass, type);//PU
                mthdIL.Emit(OpCodes.Stloc, tpmEvent);//PP

                foreach (FieldInfo info in type.GetFields())
                {
                    Type ctype = info.FieldType;
                    MethodInfo brWrite = CreateWriterMethod<T>(ctype);
                    mthdIL.Emit(OpCodes.Nop);
                    mthdIL.Emit(OpCodes.Ldarg_0);//PU binary writer
                    mthdIL.Emit(OpCodes.Ldarg_1);//PU binary writer
                    //mthdIL.Emit(OpCodes.Ldloc, tpmEvent);//PU binary writer

                    mthdIL.Emit(OpCodes.Ldfld, info);
                    mthdIL.EmitCall(OpCodes.Call, brWrite, null);//PU
                                                                 //mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU
                    mthdIL.Emit(OpCodes.Nop);
                    //mthdIL.Emit(OpCodes.Ret);
                }
            }
            mthdIL.Emit(OpCodes.Ret);

            //if (type.IsClass)
            //{
            //    LocalBuilder tpmEvent = mthdIL.DeclareLocal(type);
            //    mthdIL.Emit(OpCodes.Nop);
            //    mthdIL.Emit(OpCodes.Ldarg_1);//PU
            //    mthdIL.Emit(OpCodes.Castclass, type);//PU
            //    mthdIL.Emit(OpCodes.Stloc, tpmEvent);//PP

            //    foreach (FieldInfo info in type.GetFields())
            //    {
            //        if (Utils.IsIgnoreAttribute(info))
            //            continue;

            //        Type ctype = info.FieldType;
            //        MethodInfo brWrite = CreateWriterMethod<T>(ctype);
            //        mthdIL.Emit(OpCodes.Ldarg_0);//PU binary writer
            //        mthdIL.Emit(OpCodes.Ldloc, tpmEvent);//PU binary writer

            //        mthdIL.Emit(OpCodes.Ldfld, info);
            //        mthdIL.EmitCall(OpCodes.Call, brWrite, null);//PU
            //                                                     //mthdIL.EmitCall(OpCodes.Callvirt, brWrite, null);//PU
            //        mthdIL.Emit(OpCodes.Nop);
            //        mthdIL.Emit(OpCodes.Ret);
            //    }
            //}

            #region old

            //DynamicMethod dynamicGet = new DynamicMethod("Serialization_" + type.Name, typeof(void), new Type[] { typeof(T), typeof(object) }, typeof(object), true);//new DynamicMethod("DynamicGet", typeof(object), new Type[] { typeof(object) }, type, true);
            //ILGenerator mthdIL = dynamicGet.GetILGenerator();

            //if (IsBaseType(type) == true)
            //{
            //    SerializeBaseType<T>(type, mthdIL);
            //    CutTail<T>(mthdIL);
            //}
            //else if (type.IsClass)
            //{
            //    LocalBuilder tpmEvent = mthdIL.DeclareLocal(type);
            //    mthdIL.Emit(OpCodes.Nop);
            //    mthdIL.Emit(OpCodes.Ldarg_1);//PU
            //    mthdIL.Emit(OpCodes.Castclass, type);//PU
            //    mthdIL.Emit(OpCodes.Stloc, tpmEvent);//PP


            //    SerializeFields<T>(type, mthdIL, tpmEvent);
            //    SerializePropertys<T>(type, mthdIL, tpmEvent);
            //}
            ////目前还无法处理值类型
            //else if (type.BaseType == typeof(ValueType))
            //{
            //    LocalBuilder tpmEvent = mthdIL.DeclareLocal(type);
            //    mthdIL.Emit(OpCodes.Nop);
            //    mthdIL.Emit(OpCodes.Ldarg_1);//PU
            //    mthdIL.Emit(OpCodes.Castclass, type);//PU
            //    mthdIL.Emit(OpCodes.Stloc, tpmEvent);//PP

            //    SerializeValueFields<T>(type, mthdIL, tpmEvent);
            //    SerializeValuePropertys<T>(type, mthdIL, tpmEvent);
            //}
            //else
            //{
            //    SerializeBaseType<T>(type, mthdIL);
            //    CutTail<T>(mthdIL);
            //}

            #endregion



            #endregion

            return (Serialize<T>)dynamicGet.CreateDelegate(typeof(Serialize<T>));
        }

        internal override Deserialize<T> GenerateDeserializationType<T>(Type type)
        {
            return base.GenerateDeserializationType<T>(type);
        }

        internal override void SerializeSizeFields<T>(Type type, ILGenerator mthdIL, LocalBuilder tpmEvent)
        {
            base.SerializeSizeFields<T>(type, mthdIL, tpmEvent);
        }

        internal override void SerializeSizePropertys<T>(Type type, ILGenerator mthdIL, LocalBuilder tpmEvent)
        {
            base.SerializeSizePropertys<T>(type, mthdIL, tpmEvent);
        }

        internal override bool IsFixedSizeType(Type type)
        {
            if (type.IsPrimitive)
                return true;
            if (type == TypeConsts.Guid)
                return true;
            if (type == TypeConsts.DateTime)
                return true;
            if (type == TypeConsts.DateTimeOffset)
                return true;
            if (type == TypeConsts.TimeSpan)
                return true;
            return false;
        }

        internal override int GetSize(Type type)
        {
            if (type == TypeConsts.Boolean)
                return 1;
            if (type == TypeConsts.Char)
                return 2;
            if (type == TypeConsts.SByte)
                return 1;
            if (type == TypeConsts.Byte)
                return 1;
            if (type == TypeConsts.Int16)
                return 2;
            if (type == TypeConsts.UInt16)
                return 2;
            if (type == TypeConsts.Int32)
                return 4;
            if (type == TypeConsts.UInt32)
                return 4;
            if (type == TypeConsts.Int64)
                return 8;
            if (type == TypeConsts.UInt64)
                return 8;
            if (type == TypeConsts.Single)
                return 4;
            if (type == TypeConsts.Double)
                return 8;
            if (type == TypeConsts.Decimal)
                return 16;

            if (type == TypeConsts.DateTime)
                return 8;
            if (type == TypeConsts.DateTimeOffset)
                return 8;
            if (type == TypeConsts.TimeSpan)
                return 8;
            if (type == TypeConsts.Guid)
                return 16;
            if (type == TypeConsts.String)
                return 0;
            else 
                throw new NotSupportedException();
        }

        /// <summary>
        /// 默认原生支持的数据类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal override bool IsBaseType(Type type)
        {
            if (type.GetInterface("IList") == typeof(IList) ||
                    type.GetInterface("IDictionary") == typeof(IDictionary) ||
                    type.GetInterface("ICollection") == typeof(ICollection))
                return true;
            if (type.IsPrimitive)
                return true;

            if (type == typeof(decimal))
                return true;

            if (type.IsArray)
            {
                if (type == typeof(bool[][]))
                    return true;
                if (type == typeof(byte[][]))
                    return true;
            }

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

        internal override void CutTail<T>(ILGenerator mthdIL)
        {
            
        }

        #region old

        //internal override string CreateReaderMethod(Type type)
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

        //    else if (type == typeof(decimal))
        //        return "ReadDecimal";

        //    else if (type == typeof(float))
        //        return "ReadSingle";

        //    else if (type == typeof(double))
        //        return "ReadDouble";

        //    //else if (type == typeof(IDictionary))//currenly supports a string dictionary only
        //    //    return "ReadString";
        //    //else if (type == typeof(IList))
        //    //    return "ReadString";
        //    else if (type == typeof(TimeSpan))
        //        return "ReadTimeSpan";
        //    else if (type == typeof(DateTimeOffset))
        //        return "ReadDateTimeOffset";
        //    else if (type == typeof(Enum))
        //        return "ReadEnum";
        //    else if (type == typeof(Guid))
        //        return "ReadGuid";
        //    //else if (type == typeof(DataTable))
        //    //    return "ReadString";
        //    //else if (type == typeof(DataSet))
        //    //    return "ReadString";
        //    //else if (type == typeof(Hashtable))
        //    //    return "ReadString";


        //        //读取数组数据
        //    else if (type == typeof(bool[]) || type == typeof(List<bool>) || type == typeof(IList<bool>) || type == typeof(IEnumerable<bool>))
        //        return "ReadBooleans";
        //    else if (type == typeof(byte[]) || type == typeof(List<byte>) || type == typeof(IList<byte>) || type == typeof(IEnumerable<byte>))
        //        return "ReadBytes";
        //    else if (type == typeof(sbyte[]) || type == typeof(List<sbyte>) || type == typeof(IList<sbyte>) || type == typeof(IEnumerable<sbyte>))
        //        return "ReadSBytes";
        //    else if (type == typeof(short[]) || type == typeof(List<short>) || type == typeof(IList<short>) || type == typeof(IEnumerable<short>))
        //        return "ReadShorts";
        //    else if (type == typeof(ushort[]) || type == typeof(List<ushort>) || type == typeof(IList<ushort>) || type == typeof(IEnumerable<ushort>))
        //        return "ReadUShorts";
        //    else if (type == typeof(char[]) || type == typeof(List<char>) || type == typeof(IList<char>) || type == typeof(IEnumerable<char>))
        //        return "ReadChars";
        //    else if (type == typeof(int[]) || type == typeof(List<int>) || type == typeof(IList<int>) || type == typeof(IEnumerable<int>))
        //        return "ReadInts";
        //    else if (type == typeof(uint[]) || type == typeof(List<uint>) || type == typeof(IList<uint>) || type == typeof(IEnumerable<uint>))
        //        return "ReadUInts";
        //    else if (type == typeof(float[]) || type == typeof(List<float>) || type == typeof(IList<float>) || type == typeof(IEnumerable<float>))
        //        return "ReadFloats";
        //    else if (type == typeof(double[]) || type == typeof(List<double>) || type == typeof(IList<double>) || type == typeof(IEnumerable<double>))
        //        return "ReadDoubles";
        //    else if (type == typeof(decimal[]) || type == typeof(List<decimal>) || type == typeof(IList<decimal>) || type == typeof(IEnumerable<decimal>))
        //        return "ReadDecimals";
        //    else if (type == typeof(DateTime[]) || type == typeof(List<DateTime>) || type == typeof(IList<DateTime>) || type == typeof(IEnumerable<DateTime>))
        //        return "ReadDateTimes";
        //    else if (type == typeof(TimeSpan[]) || type == typeof(List<TimeSpan>) || type == typeof(IList<TimeSpan>) || type == typeof(IEnumerable<TimeSpan>))
        //        return "ReadTimeSpans";
        //    else if (type == typeof(DateTimeOffset[]) || type == typeof(List<DateTimeOffset>) || type == typeof(IList<DateTimeOffset>) || type == typeof(IEnumerable<DateTimeOffset>))
        //        return "ReadDateTimeOffsets";
        //    else if (type == typeof(Enum[]) || type == typeof(List<Enum>) || type == typeof(IList<Enum>) || type == typeof(IEnumerable<Enum>))
        //        return "ReadEnums";
        //    else if (type == typeof(Guid[]) || type == typeof(List<Guid>) || type == typeof(IList<Guid>) || type == typeof(IEnumerable<Guid>))
        //        return "ReadGuids";

        //    else if (type is object)
        //        return "ReadObject";
        //    else
        //        throw new Exception("类型无法解析到指定的方法");

        //}

        #endregion
    }
}
