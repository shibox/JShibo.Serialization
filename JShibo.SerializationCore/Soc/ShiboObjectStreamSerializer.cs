using JShibo.Serialization.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JShibo.Serialization.Soc
{
    internal class ShiboObjectStreamSerializer : SerializerBase<ObjectStream, ObjectUstream, ObjectStreamContext, ObjectStreamSize>
    {
        static ShiboObjectStreamSerializer Instance;

        #region 构造函数

        static ShiboObjectStreamSerializer()
        {
            Instance = new ShiboObjectStreamSerializer();
            Instance.builder = new SocILBuilder();
            Instance.RegisterAssemblyTypes();
        }

        #endregion

        #region 方法

        internal static void CreateContext(Type type, ObjectStreamContext info)
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

                        info.SerializeList.Add(Instance.GenerateDataSerializeSurrogate(field.FieldType));
                        info.TypeCountsList.Add(Instance.GetDeserializeTypes(field.FieldType).Length);
                        info.TypesList.Add(field.FieldType);
                        //info.NameCountsList.Add(GetSerializeNames(field.FieldType).Length);
                        CreateContext(field.FieldType, info);
                    }
                    else
                        info.MinSize += Instance.builder.GetSize(field.FieldType);
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
                        info.SerializeList.Add(Instance.GenerateDataSerializeSurrogate(property.PropertyType));
                        info.TypeCountsList.Add(Instance.GetDeserializeTypes(property.PropertyType).Length);
                        info.TypesList.Add(property.PropertyType);
                        //info.NameCountsList.Add(GetSerializeNames(property.PropertyType).Length);
                        CreateContext(property.PropertyType, info);
                    }
                    else
                        info.MinSize += Instance.builder.GetSize(property.PropertyType);
                }
            }
        }

        internal static ObjectStreamContext GetContext(Type type)
        {
            ObjectStreamContext info = null;
            if (types.TryGetValue(type, out info) == false)
            {
                info = new ObjectStreamContext();
                CreateContext(type, info);
                info.Serializer = Instance.GenerateDataSerializeSurrogate(type);
                info.SizeSerializer = Instance.GenerateSizeSerializeSurrogate(type);
                info.Deserializer = Instance.GetDeserializeSurrogate(type);
                types.Add(type, info);
                if (info != null)
                    info.ToArray();
            }
            return info; 
        }

        internal static void LoopSerialize(ObjectStream stream, object graph)
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

        internal static void LoopSerializeList(ObjectStream stream, IList value)
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

        internal static void LoopSerializeEnumerable(ObjectStream stream, IEnumerable value)
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

        internal static void LoopSerializeObjectList(ObjectStream stream, IList value)
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

        private static int GetSize(ObjectStreamContext info, object graph)
        {
            ObjectStreamSize insize = new ObjectStreamSize();
            info.SizeSerializer(insize, graph);
            return info.MinSize + insize.Size;
        }

        #endregion

        #region 公共的

        internal static void Serialize(Stream stream,object graph)
        {
            Type type = graph.GetType();
            ObjectStreamContext info = null;
            if (type == Instance.lastSerType)
                info = Instance.lastSerTypeInfo;
            else
            {
                info = GetContext(type);
                Instance.lastSerType = type;
                Instance.lastSerTypeInfo = info;
            }

            int size = GetSize(info, graph);
            ObjectStream buffer = new ObjectStream(stream, size);
            Serialize(buffer, graph, info);
            //stream.Write(buffer.GetBuffer(), 0, buffer.position);
        }

        internal static void Serialize(ObjectStream stream, object graph)
        {
            Type type = graph.GetType();
            ObjectStreamContext info = null;
            if (type == Instance.lastSerType)
                info = Instance.lastSerTypeInfo;
            else
            {
                info = GetContext(type);
                Instance.lastSerType = type;
                Instance.lastSerTypeInfo = info;
            }
            Serialize(stream, graph, info);
        }

        internal static void Serialize(ObjectStream stream, object graph, ObjectStreamContext info)
        {
            stream.SetInfo(info);
            info.Serializer(stream, graph);
        }



        internal static T Deserialize<T>(ObjectUstream stream, ObjectStreamContext info)
        {
            //if (info.IsJsonBaseType == false)
            //    stream.position++;

            stream.desers = info.Deserializers;
            stream.types = info.Types;
            stream.typeCounts = info.TypeCounts;
            //stream.nameCounts = info.NameCounts;
            //stream.names = info.Names;
            //stream.nameLens = info.NameLens;

            object value = info.Deserializer(stream);
            return (T)value;
        }

        internal static T Deserialize<T>(ObjectUstream stream)
        {
            Type type = typeof(T);
            ObjectStreamContext info = null;
            if (type == Instance.lastSerType)
            {
                info = Instance.lastSerTypeInfo;
            }
            else
            {
                info = GetContext(type);
                Instance.lastSerType = type;
                Instance.lastSerTypeInfo = info;
            }
            return Deserialize<T>(stream, info);
        }

        #endregion
    }
}
