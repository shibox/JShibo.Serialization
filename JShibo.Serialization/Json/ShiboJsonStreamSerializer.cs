//using System;
//using System.Collections;
//using System.Collections.Generic;
////using System.Linq;
//using System.Reflection;
//using System.Text;
//using JShibo.Serialization.Common;

//namespace JShibo.Serialization.Json
//{
//    internal class ShiboJsonStreamSerializer : SerializerBase<JsonStream,JsonUstream, JsonStreamContext, JsonStreamSize>
//    {
//        #region 构造函数

//        static ShiboJsonStreamSerializer()
//        {
//            builder = new JsonILBuilder();
//            RegisterAssemblyTypes();
//        }

//        #endregion

//        #region 方法

//        //private static void RegisterAssemblyTypes()
//        //{
//        //    try
//        //    {
//        //        AssemblyName[] aa = Assembly.GetEntryAssembly().GetReferencedAssemblies();
//        //        foreach (AssemblyName a in aa)
//        //        {
//        //            AppDomain.CurrentDomain.Load(a);
//        //        }

//        //        Assembly[] asmbs = AppDomain.CurrentDomain.GetAssemblies();
//        //        foreach (Assembly a in asmbs)
//        //        {
//        //            if (Utils.IsTypeDecoratedByAttribute<TraceAssembly>(a.GetCustomAttributes(false)))
//        //            {
//        //                foreach (Type t in a.GetTypes())
//        //                {
//        //                    if (Utils.IsTypeDecoratedByAttribute<SerializableAttribute>(t.GetCustomAttributes(true)))
//        //                    {
//        //                        //GenerateSurrogateForEvent(t);
//        //                        GenerateJsonStreamSerializeSurrogate(t,true);
//        //                    }
//        //                }
//        //            }
//        //        }
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        throw ex;
//        //    }
//        //}

//        //private static Serialize<JsonStream> GenerateJsonStreamSerializeSurrogate(Type type, bool check)
//        //{
//        //    bool isWrite = false;
//        //    Serialize<JsonStream> jsonser = null;
//        //    if (check == true)
//        //    {
//        //        if (!type.IsAbstract && Utils.HasSerializableAttribute(type.GetCustomAttributes(true)))
//        //            isWrite = true;
//        //    }
//        //    if (isWrite == false && !type.IsAbstract)
//        //    {
//        //        jsonser = ILBuilder.GenerateJsonSerializationType<JsonStream>(type);
//        //        isWrite = true;

//        //        //if(ILBuilder.IsBaseType(type)==true)

//        //        //if (type.IsClass && type.IsGenericType)
//        //        //{
//        //        //    Type tmpType = type.GetGenericArguments()[0];
//        //        //    if (jsonTypeMap.TryGetValue(tmpType, out jsonser) == true)
//        //        //    {
//        //        //        isWrite = true;
//        //        //    }
//        //        //    //jsonType = ILBuilder.GenerateJsonSerializationSurrogateType<IJsonStreamSerialize, JsonStream>(type);
//        //        //}
//        //        //else
//        //        //    //jsonType = ILBuilder.GenerateJsonSerializationSurrogateType<Serialize<JsonStream>, JsonStream>(type);
//        //        //    jsonser = ILBuilder.GenerateJsonSerializationType<JsonStream>(type);
//        //    }


//        //    //else
//        //    //{
//        //    //    if (!type.IsAbstract)
//        //    //    {
//        //    //        if (type.IsClass && type.IsGenericType)
//        //    //        {
//        //    //            Type tmpType = type.GetGenericArguments()[0];
//        //    //            if (jsonTypeMap.TryGetValue(tmpType, out jsonser) == true)
//        //    //            {
//        //    //                isWrite = true;
//        //    //            }
//        //    //            //jsonType = ILBuilder.GenerateJsonSerializationSurrogateType<IJsonStreamSerialize, JsonStream>(type);
//        //    //        }
//        //    //        else
//        //    //            //jsonType = ILBuilder.GenerateJsonSerializationSurrogateType<Serialize<JsonStream>, JsonStream>(type);
//        //    //            jsonser = ILBuilder.GenerateJsonSerializationType<JsonStream>(type);
//        //    //    }
//        //    //}

//        //    if (!jsonTypeMap.ContainsKey(type) && isWrite == true)
//        //        jsonTypeMap.Add(type, jsonser);

//        //    return jsonser;
//        //}

//        //private static Deserialize<JsonStream> GenerateJsonDeserializeSurrogateForEvent(Type type, bool check)
//        //{
//        //    bool isWrite = false;
//        //    Deserialize<JsonStream> jsonser = null;
//        //    if (check == true)
//        //    {
//        //        if (!type.IsAbstract && Utils.HasSerializableAttribute(type.GetCustomAttributes(true)))
//        //            isWrite = true;
//        //    }
//        //    if (isWrite == false && !type.IsAbstract)
//        //    {
//        //        jsonser = ILBuilder.GenerateJsonDeserializationType<JsonStream>(type);
//        //        isWrite = true;
//        //    }

//        //    if (!jsonDeTypeMap.ContainsKey(type) && isWrite == true)
//        //        jsonDeTypeMap.Add(type, jsonser);

//        //    return jsonser;
//        //}

//        ////internal static IJsonStreamSerialize GetJsonSurrogateFromType(Type type)
//        ////{
//        ////    IJsonStreamSerialize sr = null;
//        ////    if (jsonTypeMap.TryGetValue(type, out sr) == false)
//        ////    {
//        ////        sr = GenerateJsonSurrogateForEvent(type,false);
//        ////    }
//        ////    return sr;
//        ////}

//        //internal static Serialize<JsonStream> GenerateJsonStreamSerializeSurrogate(Type type)
//        //{
//        //    Serialize<JsonStream> sr = null;
//        //    if (jsonTypeMap.TryGetValue(type, out sr) == false)
//        //    {
//        //        sr = GenerateJsonStreamSerializeSurrogate(type, false);
//        //    }
//        //    return sr;
//        //}

//        //internal static Deserialize<JsonStream> GetJsonDeserializeSurrogateFromType(Type type)
//        //{
//        //    Deserialize<JsonStream> sr = null;
//        //    if (jsonDeTypeMap.TryGetValue(type, out sr) == false)
//        //    {
//        //        sr = GenerateJsonDeserializeSurrogateForEvent(type, false);
//        //    }
//        //    return sr;
//        //}

//        //private static Type[] GetDeserializeTypes(Type type)
//        //{
//        //    Type[] sr = null;
//        //    if (deTypes.TryGetValue(type, out sr) == false)
//        //    {
//        //        List<Type> types = new List<Type>();
//        //        Utils.GetTypes(type, types);
//        //        sr = types.ToArray();
//        //        deTypes.Add(type, sr);
//        //    }
//        //    return sr;
//        //}

//        //internal static string[] GetSerializeNames(Type type)
//        //{
//        //    string[] sr = null;
//        //    if (namesMap.TryGetValue(type, out sr) == false)
//        //    {
//        //        List<string> types = new List<string>();
//        //        Utils.GetNames(type, types);
//        //        sr = types.ToArray();
//        //        namesMap.Add(type, sr);
//        //    }
//        //    return sr;
//        //}

//        internal static void GetJsonTypes(Type type, JsonStreamContext info)
//        {
//            if (Utils.IsJsonBaseType(type))
//            {
//                info.IsJsonBaseType = true;
//                return;
//            }

//            FieldInfo[] fields = type.GetFields();
//            foreach (FieldInfo field in fields)
//            {
//                if (Utils.IsIgnoreAttribute(field) == false)
//                {
//                    info.NamesList.Add(Utils.GetAttributeName(field));
//                    if (Utils.IsDeep(field.FieldType))
//                    {
//                        info.SerializeStreamsList.Add(GenerateDataSerializeSurrogate(field.FieldType));
//                        info.TypeCountsList.Add(GetDeserializeTypes(field.FieldType).Length);
//                        info.TypesList.Add(field.FieldType);
//                        info.NameCountsList.Add(GetSerializeNames(field.FieldType).Length);
//                        GetJsonTypes(field.FieldType, info);
//                    }
//                }
//            }
//            PropertyInfo[] propertys = type.GetProperties();
//            foreach (PropertyInfo property in propertys)
//            {
//                if (Utils.IsIgnoreAttribute(property) == false)
//                {
//                    info.NamesList.Add(Utils.GetAttributeName(property));
//                    if (Utils.IsDeep(property.PropertyType))
//                    {
//                        info.SerializeStreamsList.Add(GenerateDataSerializeSurrogate(property.PropertyType));
//                        info.TypeCountsList.Add(GetDeserializeTypes(property.PropertyType).Length);
//                        info.TypesList.Add(property.PropertyType);
//                        info.NameCountsList.Add(GetSerializeNames(property.PropertyType).Length);
//                        GetJsonTypes(property.PropertyType, info);
//                    }
//                }
//            }
//        }

//        internal static JsonStreamContext GetJsonTypes(Type type)
//        {
//            JsonStreamContext info = null;
//            if (types.TryGetValue(type, out info) == false)
//            {
//                info = new JsonStreamContext();
//                GetJsonTypes(type, info);
//                info.Serializer = GenerateDataSerializeSurrogate(type);
//                info.Deserializer = GetDeserializeSurrogate(type);
//                types.Add(type, info);
//                if (info != null)
//                    info.Donames();
//            }
//            return info; 
//        }

//        internal static void LoopSerialize(JsonStream stream, object graph)
//        {
//            #region old
//            //stream._buffer[stream.position++] = (byte)'{';
//            //Type type = graph.GetType();
//            ////IJsonStreamSerialize ser = GetJsonSurrogateFromType(type);
//            //byte[] bf = stream.GetBuffer();
//            //JsonStream loopStream = new JsonStream(ref bf);
//            //IJsonStreamSerialize ser = null;
//            //if (type == curJsonSerType)
//            //{
//            //    ser = curJsonSer;
//            //    loopStream.names = curSerName;
//            //}
//            //else
//            //{
//            //    ser = GetJsonSurrogateFromType(type);
//            //    curJsonSer = ser;
//            //    curJsonSerType = type;
//            //    curSerName = GetSerializeNames(type);
//            //    loopStream.names = curSerName;
//            //}
//            ////loopStream.SetPosition(ref stream.position);
//            //loopStream.position = stream.position;
//            ////loopStream.names = GetSerializeNames(type);
//            //ser.Serialize(loopStream, graph);
//            //stream.position = loopStream.position;
//            //stream._buffer = loopStream._buffer;

//            //if (stream._buffer[stream.position - 1] == (byte)',')
//            //{
//            //    stream._buffer[stream.position - 1] = (byte)'}';
//            //    stream._buffer[stream.position++] = (byte)',';
//            //}
//            //else
//            //    stream._buffer[stream.position++] = (byte)'}';
//            #endregion

//            #region old2

//            //stream._buffer[stream.position++] = (byte)'{';
//            //Type type = graph.GetType();
//            //byte[] bf = stream.GetBuffer();
//            //JsonStream loopStream = new JsonStream(ref bf);
//            //IJsonStreamSerialize ser = null;
//            //JsonTypeInfo info = null;
//            //if (type == curJsonSerType)
//            //{
//            //    ser = curJsonSer;
//            //    info = curTypeInfo;
//            //}
//            //else
//            //{
//            //    ser = GetJsonSurrogateFromType(type);
//            //    curJsonSer = ser;
//            //    curJsonSerType = type;
//            //    info = GetJsonTypes(type);
//            //    curTypeInfo = info;
//            //}

//            //loopStream.sers = info.Streams.ToArray();
//            //loopStream.types = info.Types.ToArray();
//            //loopStream.typeCounts = info.TypeCounts.ToArray();
//            //loopStream.nameCounts = info.NameCounts.ToArray();
//            //loopStream.names = info.Names.ToArray();

//            //loopStream.position = stream.position;
//            //ser.Serialize(loopStream, graph);
//            //stream.position = loopStream.position;
//            //stream._buffer = loopStream._buffer;

//            //if (stream._buffer[stream.position - 1] == (byte)',')
//            //{
//            //    stream._buffer[stream.position - 1] = (byte)'}';
//            //    stream._buffer[stream.position++] = (byte)',';
//            //}
//            //else
//            //    stream._buffer[stream.position++] = (byte)'}';

//            #endregion

//            #region old
//            //stream._buffer[stream.position++] = (byte)'{';
//            //Type type = graph.GetType();
//            //IJsonStreamSerialize ser = null;
//            //if (type == curJsonSerType)
//            //{
//            //    ser = curJsonSer;
//            //    stream.names = curSerName;
//            //}
//            //else
//            //{
//            //    ser = GetJsonSurrogateFromType(type);
//            //    curJsonSer = ser;
//            //    curJsonSerType = type;
//            //    curSerName = GetSerializeNames(type);
//            //    stream.names = curSerName;
//            //}
//            //ser.Serialize(stream, graph);
//            //if (stream._buffer[stream.position - 1] == (byte)',')
//            //    stream.position--;
//            //stream._buffer[stream.position++] = (byte)'}';
//            #endregion

//            stream._buffer[stream.position++] = (byte)'{';
//            Type type = graph.GetType();
//            byte[] bf = stream._buffer;
//            JsonStream loopStream = new JsonStream(bf);
//            JsonStreamContext info = null;
//            if (type == lastObjectLoopSerType)
//                info = lastObjectLoopTypeInfo;
//            else
//            {
//                lastObjectLoopSerType = type;
//                info = GetJsonTypes(type);
//                lastObjectLoopTypeInfo = info;
//            }

//            if (info.Names.Length > 0)
//            {
//                loopStream.names = info.Names;
//                loopStream.sers = info.SerializeStreams;
//                loopStream.types = info.Types;
//                loopStream.typeCounts = info.TypeCounts;
//                loopStream.nameCounts = info.NameCounts;
//            }

//            //loopStream.namesBytes = info.info.namesBytes;
//            loopStream.nameLens = info.NameLens;

//            loopStream.position = stream.position;
//            info.Serializer(loopStream, graph);
//            stream.position = loopStream.position;
//            stream._buffer = loopStream._buffer;

//            if (stream._buffer[stream.position - 1] == (byte)',')
//            {
//                stream._buffer[stream.position - 1] = (byte)'}';
//                stream._buffer[stream.position++] = (byte)',';
//            }
//            else
//                stream._buffer[stream.position++] = (byte)'}';
//        }

//        internal static void LoopSerializeObject(JsonStream stream, object graph)
//        {
//            stream._buffer[stream.position++] = (byte)'{';
//            Type type = graph.GetType();
//            byte[] bf = stream._buffer; //stream.GetBuffer();
//            JsonStream loopStream = new JsonStream(bf);
//            JsonStreamContext info = null;
//            if (type == lastObjectLoopSerType)
//                info = lastObjectLoopTypeInfo;
//            else
//            {
//                lastObjectLoopSerType = type;
//                info = GetJsonTypes(type);
//                lastObjectLoopTypeInfo = info;
//            }

//            if (info.Names.Length > 0)
//            {
//                loopStream.names = info.Names;
//                loopStream.sers = info.SerializeStreams;
//                loopStream.types = info.Types;
//                loopStream.typeCounts = info.TypeCounts;
//                loopStream.nameCounts = info.NameCounts;
//            }

//            //loopStream.namesBytes = info.info.namesBytes;
//            loopStream.nameLens = info.NameLens;

//            loopStream.position = stream.position;
//            info.Serializer(loopStream, graph);
//            stream.position = loopStream.position;
//            stream._buffer = loopStream._buffer;
//            //stream.current += loopStream.current;

//            if (stream._buffer[stream.position - 1] == (byte)',')
//            {
//                stream._buffer[stream.position - 1] = (byte)'}';
//                stream._buffer[stream.position++] = (byte)',';
//            }
//            else
//                stream._buffer[stream.position++] = (byte)'}';
//        }

//        //internal static void LoopSerialize(JsonStream stream, object graph, IJsonStreamSerialize ser,string[] names)
//        //{
//        //    stream._buffer[stream.position++] = (byte)'{';
//        //    byte[] bf = stream.GetBuffer();
//        //    JsonStream loopStream = new JsonStream(ref bf);
//        //    //IJsonStreamSerialize ser = null;
//        //    //if (type == curJsonSerType)
//        //    //{
//        //    //    ser = curJsonSer;
//        //    //    loopStream.names = curSerName;
//        //    //}
//        //    //else
//        //    //{
//        //    //    ser = GetJsonSurrogateFromType(type);
//        //    //    curJsonSer = ser;
//        //    //    curJsonSerType = type;
//        //    //    curSerName = GetSerializeNames(type);
//        //    //    loopStream.names = curSerName;
//        //    //}
//        //    //loopStream.SetPosition(ref stream.position);
//        //    loopStream.position = stream.position;
//        //    loopStream.names = names;
//        //    //loopStream.names = GetSerializeNames(type);
//        //    ser.Serialize(loopStream, graph);
//        //    stream.position = loopStream.position;
//        //    stream._buffer = loopStream._buffer;

//        //    if (stream._buffer[stream.position - 1] == (byte)',')
//        //    {
//        //        stream._buffer[stream.position - 1] = (byte)'}';
//        //        stream._buffer[stream.position++] = (byte)',';
//        //    }
//        //    else
//        //        stream._buffer[stream.position++] = (byte)'}';
//        //}


//        internal static void LoopSerialize(JsonStream stream, object graph, Serialize<JsonStream> ser, string[] names)
//        {
//            stream._buffer[stream.position++] = (byte)'{';
//            byte[] bf = stream._buffer; //stream.GetBuffer();
//            JsonStream loopStream = new JsonStream(ref bf);
//            loopStream.position = stream.position;
//            loopStream.names = names;
//            ser(loopStream, graph);
//            stream.position = loopStream.position;
//            stream._buffer = loopStream._buffer;

//            if (stream._buffer[stream.position - 1] == (byte)',')
//            {
//                stream._buffer[stream.position - 1] = (byte)'}';
//                stream._buffer[stream.position++] = (byte)',';
//            }
//            else
//                stream._buffer[stream.position++] = (byte)'}';
//        }

//        internal static void LoopSerialize(JsonStream stream, object graph, JsonStreamContext info)
//        {
//            stream._buffer[stream.position++] = (byte)'{';
//            byte[] bf = stream._buffer; //stream.GetBuffer();
//            JsonStream loopStream = new JsonStream(ref bf);
//            loopStream.position = stream.position;

//            loopStream.names = info.Names;
//            loopStream.sers = info.SerializeStreams;
//            loopStream.types = info.Types;
//            loopStream.typeCounts = info.TypeCounts;
//            loopStream.nameCounts = info.NameCounts;
//            loopStream.names = info.Names;

//            //loopStream.namesBytes = info.info.namesBytes;
//            loopStream.nameLens = info.NameLens;

//            info.Serializer(loopStream, graph);
//            stream.position = loopStream.position;
//            stream._buffer = loopStream._buffer;

//            if (stream._buffer[stream.position - 1] == (byte)',')
//            {
//                stream._buffer[stream.position - 1] = (byte)'}';
//                stream._buffer[stream.position++] = (byte)',';
//            }
//            else
//                stream._buffer[stream.position++] = (byte)'}';
//        }

//        internal static void LoopSerializeList(JsonStream stream, IList value)
//        {
//            //stream._buffer[stream.position++] = (byte)'{';
//            //stream.unFlag = false;
//            //stream.current = 0;
//            //stream.cnamepos = 0;
//            //ser(stream, graph);

//            //stream._buffer[stream.position - 1] = (byte)'}';
//            //stream._buffer[stream.position++] = (byte)',';


//            stream._buffer[stream.position] = (byte)'[';
//            stream.position++;

//            stream._buffer[stream.position++] = (byte)'{';
//            byte[] bf = stream._buffer; //stream.GetBuffer();
//            JsonStream loopStream = new JsonStream(ref bf);
//            loopStream.position = stream.position;

//            object v = value[0];
//            Type type = v.GetType();
//            JsonStreamContext info = ShiboJsonStreamSerializer.GetJsonTypes(type);
//            int count = value.Count;

//            if (info.Names.Length > 0)
//            {
//                loopStream.names = info.Names;
//                loopStream.sers = info.SerializeStreams;
//                loopStream.types = info.Types;
//                loopStream.typeCounts = info.TypeCounts;
//                loopStream.nameCounts = info.NameCounts;
//            }

//            //loopStream.namesBytes = info.info.namesBytes;
//            loopStream.nameLens = info.NameLens;

//            info.Serializer(loopStream, v);
//            loopStream.position--;
//            loopStream._buffer[loopStream.position] = (byte)'}';
//            loopStream._buffer[loopStream.position + 1] = (byte)',';
//            loopStream.position += 2;

//            for (int i = 1; i < count; i++)
//            {
//                v = value[i];
//                if (v != null)
//                {
//                    loopStream.current = 0;
//                    loopStream.cnamepos = 0;
//                    loopStream._buffer[loopStream.position++] = (byte)'{';
//                    info.Serializer(loopStream, value[i]);
//                    loopStream._buffer[loopStream.position - 1] = (byte)'}';
//                    loopStream._buffer[loopStream.position] = (byte)',';
//                    loopStream.position++;
//                }
//                else
//                {
//                    loopStream.WriteNullWithoutName();
//                }
//            }
//            stream.position = loopStream.position;
//            stream._buffer = loopStream._buffer;


//            //直接覆盖掉最后一个“,”
//            stream._buffer[stream.position - 1] = (byte)']';
//            stream._buffer[stream.position] = (byte)',';
//            stream.position++;
//        }

//        internal static void LoopSerializeObjectList(JsonStream stream, IList value)
//        {
//            stream._buffer[stream.position] = (byte)'[';
//            stream.position++;

//            stream._buffer[stream.position++] = (byte)'{';
//            byte[] bf = stream._buffer; //stream.GetBuffer();
//            JsonStream loopStream = new JsonStream(bf);
//            loopStream.position = stream.position;

//            object v = value[0];
//            Type type = v.GetType();
//            JsonStreamContext info = GetJsonTypes(type);
//            int count = value.Count;

//            if (info.Names.Length > 0)
//            {
//                loopStream.names = info.Names;
//                loopStream.sers = info.SerializeStreams;
//                loopStream.types = info.Types;
//                loopStream.typeCounts = info.TypeCounts;
//                loopStream.nameCounts = info.NameCounts;
//            }

//            //loopStream.namesBytes = info.info.namesBytes;
//            loopStream.nameLens = info.NameLens;

//            info.Serializer(loopStream, v);
//            loopStream.position--;
//            loopStream._buffer[loopStream.position] = (byte)'}';
//            loopStream._buffer[loopStream.position + 1] = (byte)',';
//            loopStream.position += 2;

//            for (int i = 1; i < count; i++)
//            {
//                v = value[i];
//                if (v != null)
//                {
//                    if (v.GetType() == type)
//                    {
//                        loopStream.current = 0;
//                        loopStream.cnamepos = 0;
//                        loopStream._buffer[loopStream.position++] = (byte)'{';
//                        info.Serializer(loopStream, v);
//                        loopStream._buffer[loopStream.position - 1] = (byte)'}';
//                        loopStream._buffer[loopStream.position] = (byte)',';
//                        loopStream.position++;
//                    }
//                    else
//                    {
//                        LoopSerialize(loopStream, v);
//                    }
//                }
//                else
//                {
//                    loopStream.WriteNullWithoutName();
//                }
//            }
//            stream.position = loopStream.position;
//            stream._buffer = loopStream._buffer;


//            //直接覆盖掉最后一个“,”
//            stream._buffer[stream.position - 1] = (byte)']';
//            stream._buffer[stream.position] = (byte)',';
//            stream.position++;
//        }

//        #endregion

//        #region 公共的

//        internal static string Serialize(object graph)
//        {
//            Type type = graph.GetType();
//            JsonStreamContext info = null;
//            if (type == lastSerType)
//            {
//                info = lastTypeInfo;
//            }
//            else
//            {
//                info = GetJsonTypes(type);
//                lastSerType = type;
//                lastTypeInfo = info;
//            }
//            JsonStream stream = new JsonStream(info.MinSize);
//            Serialize(stream, graph, info);
//            return stream.ToString();
//        }

//        internal static void Serialize(JsonStream stream, object graph)
//        {
//            #region old
//            //stream._buffer[stream.position++] = (byte)'{';
//            //Type type = graph.GetType();
//            //IJsonStreamSerialize ser = null;
//            //if (type == curJsonSerType)
//            //{
//            //    ser = curJsonSer;
//            //    stream.names = curSerName;
//            //}
//            //else
//            //{
//            //    ser = GetJsonSurrogateFromType(type);
//            //    curJsonSer = ser;
//            //    curJsonSerType = type;
//            //    if (stream.names == null)
//            //    {
//            //        curSerName = GetSerializeNames(type);
//            //        stream.names = curSerName;
//            //    }
//            //}
//            //ser.Serialize(stream, graph);
//            //if (stream._buffer[stream.position - 1] == (byte)',')
//            //{
//            //    stream.position--;
//            //    stream._buffer[stream.position++] = (byte)'}';
//            //}
//            //else
//            //    stream._buffer[stream.position++] = (byte)'}';
//            #endregion

//            #region old2

//            //stream._buffer[stream.position++] = (byte)'{';
//            //Type type = graph.GetType();
//            //JsonTypeInfo info = null;
//            //if (type == curJsonSerType)
//            //{
//            //    info = curTypeInfo;
//            //}
//            //else
//            //{
//            //    info = GetJsonTypes(type);
//            //    curJsonSerType = type;
//            //    curTypeInfo = info;
//            //}

//            //stream.sers = info.Streams.ToArray();
//            //stream.types = info.Types.ToArray();
//            //stream.typeCounts = info.TypeCounts.ToArray();
//            //stream.nameCounts = info.NameCounts.ToArray();
//            //stream.names = info.Names.ToArray();
//            //info.Ser.Serialize(stream, graph);
//            //if (stream._buffer[stream.position - 1] == (byte)',')
//            //{
//            //    stream.position--;
//            //    stream._buffer[stream.position++] = (byte)'}';
//            //}
//            //else
//            //    stream._buffer[stream.position++] = (byte)'}';

//            #endregion

//            //stream._buffer[stream.position++] = (byte)'{';
//            //Type type = graph.GetType();
//            //JsonTypeInfo info = null;
//            //if (type == curJsonSerType)
//            //{
//            //    info = curTypeInfo;
//            //}
//            //else
//            //{
//            //    info = GetJsonTypes(type);
//            //    curJsonSerType = type;
//            //    curTypeInfo = info;
//            //    info.Donames();
//            //}

//            //stream.sers = info.Streams.ToArray();
//            //stream.types = info.Types.ToArray();
//            //stream.typeCounts = info.TypeCounts.ToArray();
//            //stream.nameCounts = info.NameCounts.ToArray();
//            //stream.names = info.Names.ToArray();

//            //stream.namesBytes = info.namesBytes;
//            ////stream.NamesBytes = info.namesBytes;
//            //stream.nameLens = info.nameLens.ToArray();

//            //info.Ser(stream, graph);
//            //if (stream._buffer[stream.position - 1] == (byte)',')
//            //{
//            //    stream.position--;
//            //    stream._buffer[stream.position++] = (byte)'}';
//            //}
//            //else
//            //    stream._buffer[stream.position++] = (byte)'}';



//            Type type = graph.GetType();
//            JsonStreamContext info = null;
//            if (type == lastSerType)
//            {
//                info = lastTypeInfo;
//            }
//            else
//            {
//                info = GetJsonTypes(type);
//                lastSerType = type;
//                lastTypeInfo = info;
//            }
//            Serialize(stream, graph, info);
//        }

//        internal static void Serialize(JsonStream stream, object graph, JsonStreamContext info)
//        {
//            if (info.IsJsonBaseType == false)
//                stream._buffer[stream.position++] = (byte)'{';

//            stream.sers = info.SerializeStreams;
//            stream.types = info.Types;
//            stream.typeCounts = info.TypeCounts;
//            stream.nameCounts = info.NameCounts;
//            stream.names = info.Names;

//            //stream.namesBytes = info.info.namesBytes;
//            stream.nameLens = info.NameLens;

//            info.Serializer(stream, graph);
//            if (stream._buffer[stream.position - 1] == ',')
//                stream.position--;
//            if (info.IsJsonBaseType == false)
//                stream._buffer[stream.position++] = (byte)'}';
//        }

//        internal static void Serialize(JsonStream stream, object graph, Serialize<JsonStream> ser)
//        {
//            stream._buffer[stream.position++] = (byte)'{';
//            ser(stream, graph);
//            if (stream._buffer[stream.position - 1] == (byte)',')
//                stream.position--;
//            stream._buffer[stream.position++] = (byte)'}';
//        }



//        internal static T Deserialize<T>(JsonUstream stream, JsonStreamContext info) where T : class
//        {
//            if (info.IsJsonBaseType == false)
//                stream.position++;

//            stream.desers = info.DeserializeStreams;
//            stream.types = info.Types;
//            stream.typeCounts = info.TypeCounts;
//            stream.nameCounts = info.NameCounts;
//            stream.names = info.Names;

//            //stream.namesBytes = info.info.namesBytes;
//            stream.nameLens = info.NameLens;

//            object value = info.Deserializer(stream);
//            return value as T;
//        }

//        internal static T Deserialize<T>(JsonUstream stream) where T : class
//        {
//            Type type = typeof(T);
//            JsonStreamContext info = null;
//            if (type == lastSerType)
//            {
//                info = lastTypeInfo;
//            }
//            else
//            {
//                info = GetJsonTypes(type);
//                lastSerType = type;
//                lastTypeInfo = info;
//            }
//            return Deserialize<T>(stream, info);

//            return default(T);
//            //throw new Exception("Not supported,it will implementation in next versions");
//        }

//        #endregion
//    }

//}
