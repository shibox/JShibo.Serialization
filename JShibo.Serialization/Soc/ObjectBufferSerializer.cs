using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
//using System.Linq;
using System.Reflection;
using System.Text;
using JShibo.Serialization.Common;
using JShibo.Serialization.Soc;

namespace JShibo.Serialization.Soc
{
    internal class ObjectBufferSerializer : SerializerBase<ObjectWriter, ObjectReader, ObjectBufferContext, ObjectWriterSize>
    {
        static ObjectBufferSerializer Instance;

        #region 构造函数

        static ObjectBufferSerializer()
        {
            Instance = new ObjectBufferSerializer();
            Instance.builder = new SocILBuilder();
            Instance.RegisterAssemblyTypes();
        }

        #endregion

        #region 方法

        internal static void CreateContext(Type type, ObjectBufferContext info)
        {
            if (Instance.builder.IsBaseType(type) == true)
            {
                info.IsBaseType = true;
                return;
            }

            FieldInfo[] fields = type.GetFields(info.Seting.Flags);
            foreach (FieldInfo field in fields)
            {
                if (Utils.IsIgnoreAttribute(field) == false)
                {
                    //info.NamesList.Add(Utils.GetAttributeName(field));
                    if (Utils.IsDeep(field.FieldType))
                    {
                        info.IsAllBaseType = false;
                        info.SerializeList.Add(Instance.GenerateDataSerializeSurrogate(field.FieldType));
                        info.EstimateSizeList.Add(Instance.GenerateSizeSerializeSurrogate(field.FieldType));
                        info.DeserializeList.Add(Instance.GetDeserializeSurrogate(field.FieldType));
                        info.TypeCountsList.Add(Instance.GetDeserializeTypes(field.FieldType).Length);
                        info.TypesList.Add(field.FieldType);
                        //info.NameCountsList.Add(GetSerializeNames(field.FieldType).Length);
                        CreateContext(field.FieldType, info);
                    }
                    else
                    {
                        int size = Instance.builder.GetSize(field.FieldType);
                        if(size == 0)
                            info.IsAllFixedSize = false;
                        info.MinSize += size;
                    }
                    if (field.FieldType == TypeConsts.Object)
                        info.IsHaveObjectType = true;
                }
            }
            PropertyInfo[] propertys = type.GetProperties(info.Seting.Flags);
            foreach (PropertyInfo property in propertys)
            {
                if (Utils.IsIgnoreAttribute(property) == false)
                {
                    //info.NamesList.Add(Utils.GetAttributeName(property));
                    if (Utils.IsDeep(property.PropertyType))
                    {
                        info.IsAllBaseType = false;
                        info.SerializeList.Add(Instance.GenerateDataSerializeSurrogate(property.PropertyType));
                        info.EstimateSizeList.Add(Instance.GenerateSizeSerializeSurrogate(property.PropertyType));
                        info.DeserializeList.Add(Instance.GetDeserializeSurrogate(property.PropertyType));
                        info.TypeCountsList.Add(Instance.GetDeserializeTypes(property.PropertyType).Length);
                        info.TypesList.Add(property.PropertyType);
                        //info.NameCountsList.Add(GetSerializeNames(property.PropertyType).Length);
                        CreateContext(property.PropertyType, info);
                    }
                    else
                    {
                        int size = Instance.builder.GetSize(property.PropertyType);
                        if (size == 0)
                            info.IsAllFixedSize = false;
                        info.MinSize += size;
                    }
                    if (property.PropertyType == TypeConsts.Object)
                        info.IsHaveObjectType = true;
                }
            }
        }

        internal static ObjectBufferContext GetContext(Type type)
        {
            ObjectBufferContext info = null;
            if (types.TryGetValue(type, out info) == false)
            {
                info = new ObjectBufferContext();
                CreateContext(type, info);
                info.Serializer = Instance.GenerateDataSerializeSurrogate(type);
                info.EstimateSize = Instance.GenerateSizeSerializeSurrogate(type);
                info.Deserializer = Instance.GetDeserializeSurrogate(type);
                types.Add(type, info);
                if (info != null)
                    info.ToArray();
            }
            return info; 
        }

        internal static void LoopSerialize(ObjectWriter stream, object graph)
        {
            //ObjectStream loopStream = new ObjectStream(stream);
            //loopStream.WriteStartObject();

            //Type type = graph.GetType();
            //JsonStringTypeInfo info = null;
            //if (type == curObjectLoopSerType)
            //    info = curObjectLoopTypeInfo;
            //else
            //{
            //    curObjectLoopSerType = type;
            //    info = GetJsonTypes(type);
            //    curObjectLoopTypeInfo = info;
            //}
            //loopStream.SetInfo(info);

            //info.Serialize(loopStream, graph);
            //stream.position = loopStream.position;
            //stream._buffer = loopStream._buffer;

            //if (stream._buffer[stream.position - 1] == ',')
            //{
            //    stream._buffer[stream.position - 1] = '}';
            //    stream._buffer[stream.position++] = ',';
            //}
            //else
            //    stream._buffer[stream.position++] = '}';
        }

        internal static void LoopSerializeList(ObjectWriter stream, IList value)
        {
            //JsonString loopStream = new JsonString(stream);
            //loopStream.WriteStartArray();

            //object v = value[0];
            //Type type = v.GetType();

            //JsonStringTypeInfo info = GetJsonTypes(type);
            //loopStream.SetInfo(info);

            //loopStream.WriteStartObject();
            //info.Serialize(loopStream, v);
            //loopStream.WriteEndObject();

            //int count = value.Count;
            //for (int i = 1; i < count; i++)
            //{
            //    v = value[i];
            //    if (v != null)
            //    {
            //        loopStream.current = 0;
            //        loopStream.WriteStartObject();
            //        info.Serialize(loopStream, value[i]);
            //        loopStream.WriteEndObject();
            //    }
            //    else
            //        loopStream.WriteNullWithoutName();
            //}
            //stream.position = loopStream.position;
            //stream._buffer = loopStream._buffer;

            //stream.WriteEndArray();
        }

        internal static void LoopSerializeEnumerable(ObjectWriter stream, IEnumerable value)
        {
            //IEnumerator it = value.GetEnumerator();
            //if (it.MoveNext())
            //{
            //    JsonString loopStream = new JsonString(stream);
            //    loopStream.WriteStartArray();

            //    object v = it.Current;
            //    Type type = v.GetType();

            //    JsonStringTypeInfo info = GetJsonTypes(type);
            //    loopStream.SetInfo(info);

            //    loopStream.WriteStartObject();
            //    info.Serialize(loopStream, v);
            //    loopStream.WriteEndObject();

            //    while (it.MoveNext())
            //    {
            //        v = it.Current;
            //        if (v != null)
            //        {
            //            loopStream.current = 0;
            //            loopStream.WriteStartObject();
            //            info.Serialize(loopStream, v);
            //            loopStream.WriteEndObject();
            //        }
            //        else
            //            loopStream.WriteNullWithoutName();
            //    }

            //    stream.position = loopStream.position;
            //    stream._buffer = loopStream._buffer;

            //    stream.WriteEndArray();
            //}
            //else
            //    stream.WriteZeroArrayWithoutName();
        }

        internal static void LoopSerializeObjectList(ObjectWriter stream, IList value)
        {
            //JsonString loopStream = new JsonString(stream);
            //loopStream.WriteStartArray();

            //object v = value[0];
            //Type type = v.GetType();
            //JsonStringTypeInfo info = GetJsonTypes(type);
            //loopStream.SetInfo(info);

            //loopStream.WriteStartObject();
            //info.Serialize(loopStream, v);
            //loopStream.WriteEndObject();

            //int count = value.Count;
            //for (int i = 1; i < count; i++)
            //{
            //    v = value[i];
            //    if (v != null)
            //    {
            //        if (v.GetType() == type)
            //        {
            //            loopStream.current = 0;
            //            loopStream.WriteStartObject();
            //            info.Serialize(loopStream, v);
            //            loopStream.WriteEndObject();
            //        }
            //        else
            //            LoopSerialize(loopStream, v);
            //    }
            //    else
            //        loopStream.WriteNullWithoutName();
            //}
            //stream.position = loopStream.position;
            //stream._buffer = loopStream._buffer;

            //stream.WriteEndArray();
        }

        internal static int GetSize(ObjectBufferContext info, object graph)
        {
            if (info.IsAllFixedSize)
                return info.MinSize + 1;
            ObjectWriterSize insize = new ObjectWriterSize();
            info.EstimateSize(insize, graph);
            return info.MinSize + insize.Size + 1;
        }

        internal static ObjectBufferContext GetInfo(Type type)
        {
            ObjectBufferContext info = null;
            if (type == Instance.lastSerType)
                info = Instance.lastSerTypeInfo;
            else
            {
                info = GetContext(type);
                Instance.lastSerType = type;
                Instance.lastSerTypeInfo = info;
            }
            return info;
        }

        #endregion

        #region 公共的

        static byte[] internalBuffer = new byte[40];

        internal static byte[] Serialize(object graph)
        {
            Type type = graph.GetType();
            ObjectBufferContext info = GetInfo(type);
            int size = GetSize(info, graph);
            byte[] buffer = XPoolSave<byte>.Rent(size);
            ObjectWriter stream = new ObjectWriter(buffer);
            //ObjectBuffer stream = new ObjectBuffer(size);
            //ObjectBuffer stream = null;
            //if (internalBuffer.Length >= size)
            //    stream = new ObjectBuffer(internalBuffer);
            //else
            //    stream = new ObjectBuffer(size);
            Serialize(stream, graph, info);
            return stream.ToArray();
        }

        internal static void Serialize(Stream stream,object graph)
        {
            ObjectBufferContext info = GetInfo(graph.GetType());
            int size = GetSize(info, graph);
            ObjectWriter buffer = new ObjectWriter(size);
            Serialize(buffer, graph, info);
            stream.Write(buffer.GetBuffer(), 0, buffer.position);
        }

        internal static void Serialize(ObjectWriter stream, object graph)
        {
            ObjectBufferContext info = GetInfo(graph.GetType());
            Serialize(stream, graph, info);
        }

        internal static void Serialize(ObjectWriter stream, object graph, ObjectBufferContext info)
        {
            stream.WriteType(graph.GetType());
            stream.SetInfo(info);
            info.Serializer(stream, graph);
            XPoolSave<byte>.Return(stream._buffer);
        }

        internal static object Deserialize(ObjectReader stream, ObjectBufferContext info)
        {
            //跳过和识别第一个字节
            //stream.position++;
            stream.desers = info.Deserializers;
            stream.types = info.Types;
            stream.typeCounts = info.TypeCounts;
            //stream.nameCounts = info.NameCounts;
            //stream.names = info.Names;
            //stream.nameLens = info.NameLens;

            object value = info.Deserializer(stream);
            return value;
        }

        internal static object Deserialize(ObjectReader stream, Deserialize<ObjectReader> info)
        {
            object value = info(stream);
            return value;
        }

        internal static T Deserialize<T>(ObjectReader stream)
        {
            return (T)Deserialize(stream, typeof(T));
        }

        internal static object Deserialize(ObjectReader stream,Type type)
        {
            ObjectBufferContext info = GetInfo(type);
            return Deserialize(stream, info);
        }

        internal static long Sizeof(object graph)
        {
            ObjectBufferContext info = GetInfo(graph.GetType());
            int size = GetSize(info, graph);
            return size;
        }

        #endregion

    }
}
