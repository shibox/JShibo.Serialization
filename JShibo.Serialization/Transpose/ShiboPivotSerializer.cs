using JShibo.Serialization.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JShibo.Serialization.Transpose
{
    internal class ShiboPivotSerializer : ConvertBase<PivotEncode, PivotDecode, ConvertContext>
    {

        static ShiboPivotSerializer Instance;

        #region 构造函数

        static ShiboPivotSerializer()
        {
            Instance = new ShiboPivotSerializer
            {
                builder = new ConvertILBuilder()
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
        internal static void CreateContext(Type type, ConvertContext info)
        {
            var fields = type.GetFields(info.Seting.Flags);
            foreach (var field in fields)
            {
                if (Utils.IsIgnoreAttribute(field) == false)
                {
                    string name = Utils.GetAttributeName(field);
                    info.NamesList.Add(name);
                }
            }
            var propertys = type.GetProperties(info.Seting.Flags);
            foreach (var property in propertys)
            {
                if (Utils.IsIgnoreAttribute(property) == false)
                {
                    string name = Utils.GetAttributeName(property);
                    info.NamesList.Add(name);
                }
            }
        }

        internal static ConvertContext GetContext(Type type)
        {
            if (types.TryGetValue(type, out var info) == false)
            {
                info = new ConvertContext();
                CreateContext(type, info);
                info.Serializer = Instance.GenerateDataSerializeSurrogate(type);
                //info.SizeSerializer = Instance.GenerateSizeSerializeSurrogate(type);
                //info.Deserializer = Instance.GetDeserializeSurrogate(type);
                types.Add(type, info);
                if (info != null)
                    info.ToArray();
            }
            return info;
        }

        #endregion

        #region 公共的

        internal static ConvertContext GetLastContext(Type type)
        {
            ConvertContext info = null;
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

        internal static DataColumn[] Serialize(object graph,bool useCache = true)
        {
            if (graph.GetType().IsArray)
            {
                var list = graph as Array;
                var type = list.GetType();
                if (list.Length > 0)
                    type = list.GetValue(0).GetType();
                ConvertContext info = GetLastContext(type);
                //using PivotEncode encoder = null;
                if (list != null)
                {
                    using PivotEncode encoder = new PivotEncode(type, list.Length, useCache);
                    for (int i = 0; i < list.Length; i++)
                    {
                        info.Serializer(encoder, list.GetValue(i));
                        encoder.Reset();
                    }
                    return encoder.GetResult();
                }
                return null;
            }
            Type[] gtypes = graph.GetType().GenericTypeArguments;
            if (gtypes.Length == 1)
            {
                Type type = gtypes[0];
                ConvertContext info = GetLastContext(type);
                PivotEncode stream = null;
                if (graph is IList list)
                {
                    stream = new PivotEncode(type, list.Count, useCache);
                    for (int i = 0; i < list.Count; i++)
                    {
                        info.Serializer(stream, list[i]);
                        stream.Reset();
                    }
                }
                return stream.GetResult();
            }
            return null;
        }

        /// <summary>
        /// 从数据库等数据源的转换
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static DataColumn Serialize(IDataReader source)
        {
            var types = new Type[source.FieldCount];
            for (int i = 0; i < types.Length; i++)
                types[i] = source.GetFieldType(i);
            SerializeWrite<PivotEncodeIDataReader> info = Instance.builder.GenerateSerializationWriteType<PivotEncodeIDataReader>(types);
            var handle = new PivotEncodeIDataReader(types, 10000);
            while (source.Read())
            {
                source.GetValues(handle.values);
                info(handle);
                handle.Reset();
            }
            return handle.GetResult();
        }

        #endregion
    }
}
