using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;
using System.Reflection;
using System.Text;
using JShibo.Serialization.Common;
using JShibo.Serialization;
using System.Diagnostics;

namespace JShibo.Serialization.Json
{
    internal class ShiboJsonStringSerializer : SerializerBase<JsonString, JsonUstring, JsonStringContext, JsonStringSize>
    {
        static ShiboJsonStringSerializer Instance;

        #region 构造函数

        static ShiboJsonStringSerializer()
        {
            Instance = new ShiboJsonStringSerializer();
            Instance.builder = new JsonILBuilder();
            Instance.RegisterAssemblyTypes();
        }

        #endregion

        #region 方法

        internal static void CreateContext(Type type, JsonStringContext info)
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
                    string name = Utils.GetAttributeName(field);
                    info.NamesList.Add(name);
                    info.MinSize += name.Length + 3;
                    info.ChecksList.Add(Utils.GetCheckAttribute(field));

                    if (Utils.IsDeep(field.FieldType))
                    {
                        info.SerializeList.Add(Instance.GenerateDataSerializeSurrogate(field.FieldType));
                        info.EstimateSizeList.Add(Instance.GenerateSizeSerializeSurrogate(field.FieldType));
                        info.DeserializeList.Add(Instance.GetDeserializeSurrogate(field.FieldType));
                        info.TypeCountsList.Add(Instance.GetDeserializeTypes(field.FieldType).Length);
                        info.TypesList.Add(field.FieldType);
                        info.NameCountsList.Add(Instance.GetSerializeNames(field.FieldType).Length);
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
                    string name = Utils.GetAttributeName(property);
                    info.NamesList.Add(name);
                    info.MinSize += name.Length + 3;
                    info.ChecksList.Add(Utils.GetCheckAttribute(property));

                    if (Utils.IsDeep(property.PropertyType))
                    {
                        info.SerializeList.Add(Instance.GenerateDataSerializeSurrogate(property.PropertyType));
                        info.EstimateSizeList.Add(Instance.GenerateSizeSerializeSurrogate(property.PropertyType));
                        info.DeserializeList.Add(Instance.GetDeserializeSurrogate(property.PropertyType));
                        info.TypeCountsList.Add(Instance.GetDeserializeTypes(property.PropertyType).Length);
                        info.TypesList.Add(property.PropertyType);
                        info.NameCountsList.Add(Instance.GetSerializeNames(property.PropertyType).Length);
                        CreateContext(property.PropertyType, info);
                    }
                    else
                        info.MinSize += Instance.builder.GetSize(property.PropertyType);
                }
            }
        }

        internal static JsonStringContext GetContext(Type type)
        {
            JsonStringContext info = null;
            if (types.TryGetValue(type, out info) == false)
            {
                info = new JsonStringContext();
                //Stopwatch w = Stopwatch.StartNew();
                //CreateContext(type, info);
                //Console.WriteLine(w.ElapsedMilliseconds);
                CreateContext(type, info);
                //Stopwatch w = Stopwatch.StartNew();
                //info.Serializer = GenerateDataSerializeSurrogate(type);
                //Console.WriteLine(w.ElapsedMilliseconds);
                info.Serializer = Instance.GenerateDataSerializeSurrogate(type);
                info.EstimateSize = Instance.GenerateSizeSerializeSurrogate(type);
                info.Deserializer = Instance.GetDeserializeSurrogate(type);
                info.ToArray();
                types.Add(type, info);
            }
            return info; 
        }

        internal static JsonStringContext GetSizeInfos(Type type)
        {
            JsonStringContext info = null;
            if (types.TryGetValue(type, out info) == false)
            {
                info = new JsonStringContext();
                CreateContext(type, info);
                info.EstimateSize = Instance.GenerateSizeSerializeSurrogate(type);
                //info.SizeSerializer = Instance.GenerateSizeSerializeSurrogate(type);
                //info.Deserialize = GetJsonDeserializeSurrogateFromType(type);
                types.Add(type, info);
                if (info != null)
                    info.ToArray();
            }
            return info;
        }


        internal static void LoopSerialize(JsonString stream, object graph)
        {
            JsonString loopStream = new JsonString(stream);
            loopStream.WriteStartObject();

            Type type = graph.GetType();
            JsonStringContext info = null;
            if (type == Instance.lastObjectLoopSerType)
                info = lastObjectLoopTypeInfo;
            else
            {
                Instance.lastObjectLoopSerType = type;
                info = GetContext(type);
                lastObjectLoopTypeInfo = info;
            }
            loopStream.SetInfo(info);

            info.Serializer(loopStream, graph);
            stream.position = loopStream.position;
            stream._buffer = loopStream._buffer;

            if (stream._buffer[stream.position - 1] == ',')
            {
                stream._buffer[stream.position - 1] = '}';
                stream._buffer[stream.position++] = ',';
            }
            else
                stream._buffer[stream.position++] = '}';
        }

        internal static void LoopSerializeList(JsonString stream, IList value)
        {
            JsonString loopStream = new JsonString(stream);
            loopStream.isRoot = false;
            loopStream.WriteStartArray();

            object v = value[0];
            Type type = v.GetType();

            JsonStringContext info = GetContext(type);
            loopStream.SetInfo(info);

            loopStream.WriteStartObject();
            info.Serializer(loopStream, v);
            loopStream.WriteEndObject();

            int count = value.Count;
            for (int i = 1; i < count; i++)
            {
                v = value[i];
                if (v != null)
                {
                    loopStream.current = 0;
                    loopStream.WriteStartObject();
                    info.Serializer(loopStream, value[i]);
                    loopStream.WriteEndObject();
                }
                else
                    loopStream.WriteNullWithoutName();
            }
            stream.position = loopStream.position;
            stream._buffer = loopStream._buffer;

            #region 测试，即便是使用指针传递，性能基本没变化
            //if (type.Name.IndexOf ( "valueitem") != -1)
            //{
            //    JsonStringTypeInfo info = GetJsonTypes(type);
            //    loopStream.names = info.Names;

            //    fixed (char* pd = &bf[loopStream.position])
            //    {
            //        char* tpd = pd;
            //        valueitem item = null;
            //        for (int i = 0; i < value.Count; i++)
            //        {
            //            item = (valueitem)value[i];
            //            *tpd++ = '{';
            //            loopStream.current = 0;
            //            tpd = loopStream.Write(tpd, item.specid);
            //            tpd = loopStream.Write(tpd, item.value);
            //            tpd--;
            //            *tpd++ = '}';
            //            *tpd++ = ',';
            //            loopStream.position += 2;
            //        }
            //    }
            //    stream.position = loopStream.position;
            //    stream._buffer = loopStream._buffer;
            //}
            //else
            //{
            //JsonStringTypeInfo info = GetJsonTypes(type);
            //int count = value.Count;

            //if (info.Names.Length > 0)
            //{
            //    loopStream.names = info.Names;
            //    loopStream.sers = info.SerializeStreams;
            //    loopStream.types = info.Types;
            //    loopStream.typeCounts = info.TypeCounts;
            //    loopStream.nameCounts = info.NameCounts;
            //}

            //loopStream.nameLens = info.NameLens;

            //loopStream._buffer[loopStream.position++] = '{';
            //info.Serialize(loopStream, v);
            //loopStream.position--;
            //loopStream._buffer[loopStream.position] = '}';
            //loopStream._buffer[loopStream.position + 1] = ',';
            //loopStream.position += 2;

            //for (int i = 1; i < count; i++)
            //{
            //    v = value[i];
            //    if (v != null)
            //    {
            //        loopStream.current = 0;
            //        loopStream.cnamepos = 0;
            //        loopStream._buffer[loopStream.position++] = '{';
            //        info.Serialize(loopStream, value[i]);
            //        loopStream._buffer[loopStream.position - 1] = '}';
            //        loopStream._buffer[loopStream.position] = ',';
            //        loopStream.position++;
            //    }
            //    else
            //    {
            //        loopStream.WriteNullWithoutName();
            //    }
            //}
            //stream.position = loopStream.position;
            //stream._buffer = loopStream._buffer;
            //}
            #endregion
            
            stream.WriteEndArray();
        }

        internal static void LoopSerializeEnumerable(JsonString stream, IEnumerable value)
        {
            IEnumerator it = value.GetEnumerator();
            if (it.MoveNext())
            {
                JsonString loopStream = new JsonString(stream);
                loopStream.isRoot = false;
                loopStream.WriteStartArray();

                object v = it.Current;
                Type type = v.GetType();

                JsonStringContext info = GetContext(type);
                loopStream.SetInfo(info);

                loopStream.WriteStartObject();
                info.Serializer(loopStream, v);
                loopStream.WriteEndObject();

                while (it.MoveNext())
                {
                    v = it.Current;
                    if (v != null)
                    {
                        loopStream.current = 0;
                        loopStream.WriteStartObject();
                        info.Serializer(loopStream, v);
                        loopStream.WriteEndObject();
                    }
                    else
                        loopStream.WriteNullWithoutName();
                }

                stream.position = loopStream.position;
                stream._buffer = loopStream._buffer;

                stream.WriteEndArray();
            }
            else
                stream.WriteZeroArrayWithoutName();
        }

        internal static void LoopSerializeObjectList(JsonString stream, IList value)
        {
            JsonString loopStream = new JsonString(stream);
            loopStream.isRoot = false;
            loopStream.WriteStartArray();

            object v = value[0];
            Type type = v.GetType();
            JsonStringContext info = GetContext(type);
            loopStream.SetInfo(info);

            loopStream.WriteStartObject();
            info.Serializer(loopStream, v);
            loopStream.WriteEndObject();

            int count = value.Count;
            for (int i = 1; i < count; i++)
            {
                v = value[i];
                if (v != null)
                {
                    if (v.GetType() == type)
                    {
                        loopStream.current = 0;
                        loopStream.WriteStartObject();
                        info.Serializer(loopStream, v);
                        loopStream.WriteEndObject();
                    }
                    else
                        LoopSerialize(loopStream, v);
                }
                else
                    loopStream.WriteNullWithoutName();
            }
            stream.position = loopStream.position;
            stream._buffer = loopStream._buffer;

            stream.WriteEndArray();
        }

        internal static JsonStringContext GetInfo(Type type)
        {
            JsonStringContext info = null;
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
            return info;
        }

        #endregion

        #region 公共的

        internal static string Serialize(object graph)
        {
            //Type type = graph.GetType();
            //JsonStringContext info = GetInfo(type);
            //char[] buffer = CharsBufferManager.GetBuffer(info.MinSize);
            //JsonString stream = new JsonString(buffer);
            //Serialize(stream, graph, info);
            //CharsBufferManager.SetBuffer(stream.GetBuffer());
            //return stream.ToString();
            return SerializeToBuffer(graph).ToString();
        }

        internal static void Serialize(JsonString stream, object graph)
        {
            Type type = graph.GetType();
            JsonStringContext info = GetInfo(type);
            Serialize(stream, graph, info);
        }

        internal static void Serialize(JsonString stream, object graph, JsonStringContext info)
        {
            if (info.IsBaseType == false)
            {
                stream._buffer[stream.position++] = '{';
                stream.SetInfo(info);
                info.Serializer(stream, graph);
                if (stream._buffer[stream.position - 1] == ',')
                    stream.position--;
                if (info.IsBaseType == false)
                    stream._buffer[stream.position++] = '}';
            }
            else
            {
                stream.isJsonBaseType = true;
                info.Serializer(stream, graph);
                stream.position--;
            }
        }

        internal static void Serialize(JsonString stream, object graph, Serialize<JsonString> ser)
        {
            stream._buffer[stream.position++] = '{';
            ser(stream, graph);
            if (stream._buffer[stream.position - 1] == ',')
                stream.position--;
            stream._buffer[stream.position++] = '}';
        }

        internal static void Serialize(JsonStringSize stream, object graph, Estimate<JsonStringSize> ser)
        {
            ser(stream, graph);


            //stream._buffer[stream.position++] = '{';
            //ser(stream, graph);
            //if (stream._buffer[stream.position - 1] == ',')
            //    stream.position--;
            //stream._buffer[stream.position++] = '}';
        }

        internal static JsonString SerializeToBuffer(object graph)
        {
            Type type = graph.GetType();
            JsonStringContext info = GetInfo(type);
            JsonStringSize size = new JsonStringSize();
            size.SetInfo(info);
            info.EstimateSize(size, graph);
            int totalSize = info.MinSize + size.Size;
            JsonString result = null;
            char[] buffer = null;
            if (totalSize > 400)
            {
                buffer = XPoolSave<char>.Rent(totalSize);
                result = new JsonString(buffer);
                Serialize(result, graph, info);
                XPoolSave<char>.Return(result.GetBuffer());
            }
            else
            {
                buffer = new char[totalSize];
                result = new JsonString(buffer);
                Serialize(result, graph, info);
            }
            return result;
        }


        internal static T Deserialize<T>(JsonUstring stream, JsonStringContext info)
        {
            if (info.IsBaseType == false)
                stream.position++;

            stream.desers = info.Deserializers;
            stream.types = info.Types;
            stream.typeCounts = info.TypeCounts;
            stream.nameCounts = info.NameCounts;
            stream.names = info.Names;
            stream.checkAttributes = info.checks;
            stream.currentLast = info.Names.Length;

            object value = info.Deserializer(stream);
            return (T)value;
        }

        internal static object Deserialize(JsonUstring stream, Deserialize<JsonUstring> info)
        {
            object value = info(stream);
            return value;
        }

        internal static T Deserialize<T>(JsonUstring stream)
        {
            Type type = typeof(T);
            JsonStringContext info = null;
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

            //return default(T);
            //throw new Exception("Not supported,it will implementation in next versions");
        }

        #endregion
    }
}
