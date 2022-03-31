using JShibo.Serialization.Common;
using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace JShibo.Serialization.Csv
{

    internal class Utf8CsvSerializer : SerializerBase<Utf8CsvWriter, Utf8CsvReader, CsvBytesContext, CsvBytesSize>
    {

        static Utf8CsvSerializer Instance;

        #region 构造函数

        static Utf8CsvSerializer()
        {
            Instance = new Utf8CsvSerializer
            {
                builder = new CsvILBuilder()
            };
            Instance.RegisterAssemblyTypes();
        }

        #endregion

        #region 方法

        /// <summary>
        /// 必须是数组或集合类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="info"></param>
        internal static void CreateContext(Type type, CsvBytesContext info)
        {
            var fields = type.GetFields(info.Seting.Flags);
            foreach (var field in fields)
            {
                if (Utils.IsIgnoreAttribute(field) == false)
                {
                    var name = Utils.GetAttributeName(field);
                    info.NamesList.Add(name);

                    if (Utils.IsDeep(field.FieldType))
                        info.IsAllFixedSize = false;
                    else
                        info.MinSize += Instance.builder.GetSize(field.FieldType);
                }
            }
            var propertys = type.GetProperties(info.Seting.Flags);
            foreach (var property in propertys)
            {
                if (Utils.IsIgnoreAttribute(property) == false)
                {
                    string name = Utils.GetAttributeName(property);
                    info.NamesList.Add(name);
                    if (Utils.IsDeep(property.PropertyType))
                        info.IsAllFixedSize = false;
                    else
                        info.MinSize += Instance.builder.GetSize(property.PropertyType);
                }
            }
        }

        internal static CsvBytesContext GetContext(Type type)
        {
            if (types.TryGetValue(type, out var info) == false)
            {
                info = new CsvBytesContext();
                CreateContext(type, info);
                info.Serializer = Instance.GenerateDataSerializeSurrogate(type);
                info.EstimateSize = Instance.GenerateSizeSerializeSurrogate(type);
                //info.Deserializer = Instance.GetDeserializeSurrogate(type);
                types.Add(type, info);
                if (info != null)
                {
                    info.ToArray();
                    info.FormatNames();
                }
            }
            return info;
        }

        //internal static CsvBytesContext GetBytesContext(Type type)
        //{
        //    if (types.TryGetValue(type, out var info) == false)
        //    {
        //        info = new CsvStringContext();
        //        CreateContext(type, info);
        //        info.Serializer = Instance.GenerateDataSerializeSurrogate(type);
        //        info.EstimateSize = Instance.GenerateSizeSerializeSurrogate(type);
        //        //info.Deserializer = Instance.GetDeserializeSurrogate(type);
        //        types.Add(type, info);
        //        if (info != null)
        //        {
        //            info.ToArray();
        //            info.FormatNames();
        //        }
        //    }
        //    return info;
        //}

        internal static CsvBytesContext GetSizeInfos(Type type)
        {
            if (types.TryGetValue(type, out var info) == false)
            {
                info = new CsvBytesContext();
                CreateContext(type, info);
                info.EstimateSize = Instance.GenerateSizeSerializeSurrogate(type);
                //info.SizeSerialize = GenerateSizeSerializeSurrogate(type);
                //info.Deserialize = GetJsonDeserializeSurrogateFromType(type);
                types.Add(type, info);
                if (info != null)
                {
                    info.ToArray();
                    info.FormatNames();
                }
            }
            return info;
        }

        #endregion

        #region 公共的

        internal static CsvBytesContext GetLastContext(Type type)
        {
            CsvBytesContext info;
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

        internal unsafe static Span<byte> Serialize(object graph)
        {
            if (graph.GetType().IsArray)
            {
                GCHandle handle = default;
                Array ar = (Array)graph;
                if (ar.Length == 0)
                    return null;
                bool isFirst = true;
                CsvBytesContext info = null;
                Utf8CsvWriter writer = null;
                foreach (object item in ar)
                {
                    if (isFirst)
                    {
                        info = GetLastContext(item.GetType());
                        //if(info.IsAllBaseType)
                        int size = info.MinSize * ar.Length + info.GetHeaderSize();
                        //char[] buffer = CharsBufferManager.GetBuffer(size);
                        byte[] buffer = ArrayPool<byte>.Shared.Rent(size);
                        handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                        writer = new Utf8CsvWriter(buffer);
                        //fixed (byte* pointer = buffer)
                        //{
                        //    stream.bp = pointer;
                        //}
                        //stream.WriteHeader(info.Names);
                        writer.WriteHeader(info.NamesCommaBytes);
                        isFirst = false;
                    }
                    info.Serializer(writer, item);
                    writer.WriteNewLine();
                }
                //CharsBufferManager.SetBuffer(stream.GetBuffer());
                ArrayPool<byte>.Shared.Return(writer.GetBuffer());
                //var ret = stream.ToString();
                handle.Free();
                return new Span<byte>(writer.GetBuffer(), 0, writer.Position);
            }
            else
            {
                Type[] gtypes = graph.GetType().GenericTypeArguments;
                if (gtypes.Length == 1)
                {
                    Type type = gtypes[0];
                    CsvBytesContext info = GetLastContext(type);
                    Utf8CsvWriter stream = null;
                    IList list = graph as IList;
                    if (list != null)
                    {
                        int size = info.MinSize * list.Count + info.GetHeaderSize();
                        byte[] buffer = XPoolSave<byte>.Rent(size);
                        stream = new Utf8CsvWriter(buffer);
                        stream.WriteHeader(info.NamesCommaBytes);
                        for (int i = 0; i < list.Count; i++)
                        {
                            info.Serializer(stream, list[i]);
                            stream.WriteNewLine();
                        }
                        XPoolSave<byte>.Return(stream.GetBuffer());
                    }
                    return new Span<byte>(stream.GetBuffer(), 0, stream.Position);
                }
                //IEnumerable enumerable =  graph as IEnumerable;
                //foreach (object item in enumerable)
                //{
                //    //stream.Write(item);
                //    //Serialize(stream, graph, info);
                //}
            }

            return null;
        }

        //internal static void Serialize(CsvString stream, object graph)
        //{
        //    Type type = graph.GetType();
        //    CsvStringContext info = GetLastContext(type);
        //    Serialize(stream, graph, info);
        //}

        internal static void Serialize(Utf8CsvWriter stream, object graph, CsvBytesContext info)
        {
            if (info.IsBaseType == false)
            {
                stream.SetInfo(info);
                stream.WriteHeader(info.NamesCommaBytes);
                info.Serializer(stream, graph);
                //if (stream._buffer[stream.position - 1] == ',')
                //    stream.position--;
                //if (info.IsBaseType == false)
                //{
                //    stream._buffer[stream.position++] = '\r';
                //    stream._buffer[stream.position++] = '\n';
                //}
            }
            else
            {
                stream.isJsonBaseType = true;
                info.Serializer(stream, graph);
                //stream.position -= 2;
            }
        }

        internal static Utf8CsvWriter SerializeToBuffer(object graph)
        {
            Type type = graph.GetType();
            CsvBytesContext info = GetLastContext(type);
            var size = new CsvBytesSize();
            info.EstimateSize(size, graph);
            int totalSize = info.MinSize + size.Size;
            Utf8CsvWriter result = null;
            byte[] buffer = null;
            if (totalSize > 400)
            {
                buffer = XPoolSave<byte>.Rent(totalSize);
                result = new Utf8CsvWriter(buffer);
                Serialize(result, graph, info);
                XPoolSave<byte>.Return(result.GetBuffer());
            }
            else
            {
                buffer = new byte[totalSize];
                result = new Utf8CsvWriter(buffer);
                Serialize(result, graph, info);
            }
            return result;
        }

        
        internal static T Deserialize<T>(Utf8CsvReader stream, CsvBytesContext info)
        {
            //if (info.IsJsonBaseType == false)
            //    stream.position++;

            //stream.desers = info.DeserializeStreams;
            //stream.types = info.Types;
            //stream.typeCounts = info.TypeCounts;
            //stream.nameCounts = info.NameCounts;
            //stream.names = info.Names;

            //object value = info.Deserialize(stream);
            //return (T)value;

            return default(T);
        }

        internal static T Deserialize<T>(Utf8CsvReader stream)
        {
            Type type = typeof(T);
            CsvBytesContext info = null;
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
