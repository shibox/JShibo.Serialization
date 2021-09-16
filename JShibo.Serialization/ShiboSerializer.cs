using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using JShibo.Serialization.Soc;
using JShibo.Serialization.Json;
using JShibo.Serialization.Csv;
using JShibo.Serialization.Transpose;
using System.Data;

namespace JShibo.Serialization
{
    /// <summary>
    /// 序列化器
    /// Json.Net功能列表
    /// http://james.newtonking.com/json/help/index.html?topic=html/JsonNetVsDotNetSerializers.htm
    /// </summary>
    public static class ShiboSerializer
    {
        #region 字段

        private const string Null = "null";

        #endregion

        #region Json Serialize

        #region string

        public static void Serialize(JsonString stream, object value)
        {
            SerializeObject(stream, value, Formatting.None, SerializerSettings.Default);
        }

        public static void Serialize(JsonString stream, object value, JsonStringContext info)
        {
            ShiboJsonStringSerializer.Serialize(stream, value, info);
        }

        public static void Serialize(JsonString stream, object value, SerializerSettings settings)
        {
            SerializeObject(stream, value, Formatting.None, settings);
        }

        public static void Serialize(JsonString stream, object value, JsonStringContext info, SerializerSettings settings)
        {
            stream.sets = settings;
            ShiboJsonStringSerializer.Serialize(stream, value, info);
        }

        public static void Serialize(TextWriter writer, object value)
        {
            JsonString buffer = ShiboJsonStringSerializer.SerializeToBuffer(value);
            writer.Write(buffer.GetBuffer(), 0, buffer.position);
        }

        public static string Serialize(object value)
        {
            return SerializeObject(value, Formatting.None, SerializerSettings.Default);
        }

        public static string SerializeObject(object value)
        {
            return SerializeObject(value, Formatting.None, SerializerSettings.Default);
        }

        public static string SerializeObject(object value, Formatting formatting)
        {
            return SerializeObject(value, formatting, SerializerSettings.Default);
        }

        public static string SerializeObject(object value, SerializerSettings settings)
        {
            return SerializeObject(value, Formatting.None, settings);
        }

        public static string SerializeObject(object value, Formatting formatting, SerializerSettings settings)
        {
            if (value == null)
                return Null;
            if (formatting == Formatting.Indented)
                settings.Pretty = true;
            return ShiboJsonStringSerializer.Serialize(value);
        }

        public static void SerializeObject(JsonString stream, object value, Formatting formatting, SerializerSettings settings)
        {
            if (value == null)
            {
                stream.WriteNullWithoutName();
                return;
            }
            stream.sets = settings;
            ShiboJsonStringSerializer.Serialize(stream, value);
        }

        public static string SerializeObjectDebug(object value)
        {
            //string s1 = SerializeObject(value);
            //string s2 = JsonConvert.SerializeObject(value);
            //if (s1 == s2)
            //    return s1;
            //else
            //    throw new Exception("两种序列化方式结果不一样");
            return JsonConvert.SerializeObject(value);
        }

        public static string SerializeObjectDebug(object value, Formatting formatting)
        {
            //string s1 = SerializeObject(value);
            //string s2 = JsonConvert.SerializeObject(value,Formatting.Indented);
            //if (s1 == s2)
            //    return s1;
            //else
            //    throw new Exception("两种序列化方式结果不一样");
            if (formatting == Formatting.Indented)
                return JsonConvert.SerializeObject(value, Newtonsoft.Json.Formatting.Indented);
            else
                return SerializeObjectDebug(value);
        }

        public static JsonString SerializeToBuffer(object value)
        {
            return ShiboJsonStringSerializer.SerializeToBuffer(value);
        }

        //public static ArraySegment<char> SerializeToBuffer(object value)
        //{
        //    JsonString result = ShiboJsonStringSerializer.SerializeToBuffer(value);
        //    return new ArraySegment<char>(result.GetBuffer(), 0, result.Position);
        //}

        //public static string ToJson<T>(this T value)
        //{
        //    return Serialize(value);
        //}

        #endregion

        #region stream

        //public static void Serialize(JsonStream stream, object value)
        //{
        //    SerializeObject(stream, value, Formatting.None, SerializerSettings.Default);
        //}

        //public static void Serialize(JsonStream stream, object value, SerializerSettings settings)
        //{
        //    SerializeObject(stream, value, Formatting.None, settings);
        //}

        //public static void Serialize(JsonStream stream, object value, JsonStreamTypeInfo info)
        //{
        //    ShiboJsonStreamSerializer.Serialize(stream, value, info);
        //}

        //public static void Serialize(JsonStream stream, object value, JsonStreamTypeInfo info, SerializerSettings settings)
        //{
        //    stream.sets = settings;
        //    ShiboJsonStreamSerializer.Serialize(stream, value, info);
        //}

        //public static void SerializeObject(object value, Stream stream)
        //{
        //    if (value == null)
        //        return;
        //    JsonStream s = new JsonStream();
        //    Serialize(s, value);
        //    s.WriteTo(stream);
        //}

        //public static void SerializeObject(JsonStream stream, object value, Formatting formatting, SerializerSettings settings)
        //{
        //    if (value == null)
        //    {
        //        stream.WriteNullWithoutName();
        //        return;
        //    }
        //    stream.sets = settings;
        //    ShiboJsonStreamSerializer.Serialize(stream, value);
        //}


        #endregion

        #endregion

        #region Json Deserialize

        //public static T Deserialize<T>(JsonStream stream) where T : class
        //{
        //    //return ShiboJsonStreamSerializer.Deserialize<T>(stream);
        //    throw new Exception("Not supported,it will implementation in next versions");
        //}

        //public static T Deserialize<T>(JsonString stream) //where T : class
        //{
        //    return ShiboJsonStringSerializer.Deserialize<T>(stream);
        //    //throw new Exception("Not supported,it will implementation in next versions");
        //}

        public static T Deserialize<T>(string value) //where T : class
        {
            //JsonParser parser = new JsonParser();
            //return parser.Parse<T>(value);

            JsonUstring stream = new JsonUstring(value);
            return ShiboJsonStringSerializer.Deserialize<T>(stream);
            //throw new Exception("Not supported,it will implementation in next versions");
        }

        public static T DeserializeObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        public static object DeserializeObject(string value)
        {
            return JsonConvert.DeserializeObject(value);
        }

        

        //public static T Deserialize<T>(string value, SerializerSettings settings) where T : class
        //{
        //    throw new Exception("Not supported,it will implementation in next versions");
        //}

        //public static T FromJson<T>(this string json) where T : class
        //{
        //    //return Deserialize<T>(new JsonStream());
        //    throw new Exception("Not supported,it will implementation in next versions");
        //}

        #endregion

        #region Bin Serialize

        /// <summary>
        /// 序列化成二进制数据
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        public static byte[] BinarySerialize(object graph)
        {
            return ShiboObjectBufferSerializer.Serialize(graph);
        }

        /// <summary>
        /// 使用缓冲区序列化
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="stream"></param>
        public static void BinarySerialize(ObjectBuffer stream, object graph)
        {
            ShiboObjectBufferSerializer.Serialize(stream, graph);
        }

        public static void BinarySerialize(ObjectStream stream, object graph)
        {
            ShiboObjectStreamSerializer.Serialize(stream, graph);
        }

        public static void BinarySerialize(Stream stream, object graph)
        {
            ShiboObjectBufferSerializer.Serialize(stream,graph);
        }

        #endregion

        #region Bin Deserialize

        public static T BinaryDeserialize<T>(byte[] buffer)
        {
            ObjectUbuffer stream = new ObjectUbuffer(buffer);
            return ShiboObjectBufferSerializer.Deserialize<T>(stream);
        }

        public static object BinaryDeserialize(byte[] buffer,Type type)
        {
            ObjectUbuffer stream = new ObjectUbuffer(buffer);
            return ShiboObjectBufferSerializer.Deserialize(stream, type);
        }

        #endregion

        #region Csv

        public static string ToCsv(object value)
        {
            return ShiboCsvStringSerializer.Serialize(value);
        }

        #endregion

        #region Transpose

        public static Transpose.DataColumn[] ToColumns(object value)
        {
            return ShiboPivotSerializer.Serialize(value);
        }

        public static void ToObjects(object value)
        {

        }

        public static Transpose.DataColumn Serialize(IDataReader reader)
        {
            //DataTable table = new DataTable();
            //for (int i = 0; i < reader.FieldCount; i++)
            //{
            //    table.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
            //}
            return ShiboPivotSerializer.Serialize(reader);
        }

        #endregion

            #region info

            //public static JsonStreamContext GetJsonStreamTypeInfos<T>()
            //{
            //    return ShiboJsonStreamSerializer.GetJsonTypes(typeof(T));
            //}

        public static JsonStringContext GetJsonStringTypeInfos<T>()
        {
            return ShiboJsonStringSerializer.GetContext(typeof(T));
        }

        //public static JsonStreamContext GetJsonStreamTypeInfos(Type type)
        //{
        //    return ShiboJsonStreamSerializer.GetJsonTypes(type);
        //}

        public static JsonStringContext GetJsonStringTypeInfos(Type type)
        {
            return ShiboJsonStringSerializer.GetContext(type);
        }

        #endregion

        #region 初始化

        /// <summary>
        /// 对数据进行初始化
        /// </summary>
        /// <typeparam name="T">初始化的类型</typeparam>
        /// <returns></returns>
        public static T Initialize<T>()
        {
            return ShiboObjectInitializer.Initialize<T>();
        }

        /// <summary>
        /// 对数据进行初始化
        /// </summary>
        /// <typeparam name="T">初始化的类型</typeparam>
        /// <param name="seed">初始化的种子</param>
        /// <returns></returns>
        public static T Initialize<T>(int seed)
        {
            return ShiboObjectInitializer.Initialize<T>(seed);
        }

        /// <summary>
        /// 对数据进行初始化
        /// </summary>
        /// <param name="type">初始化的类型</param>
        /// <returns></returns>
        public static object Initialize(Type type)
        {
            return ShiboObjectInitializer.Initialize(type);
        }

        public static object Initialize(Type type,int seed)
        {
            return ShiboObjectInitializer.Initialize(type,seed);
        }
        
        #endregion

        #region Sizeof

        /// <summary>
        /// 计算对象的内存占用情况
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long Sizeof(object value)
        {
            return ShiboObjectBufferSerializer.Sizeof(value);
        }

        #endregion
    }
}
