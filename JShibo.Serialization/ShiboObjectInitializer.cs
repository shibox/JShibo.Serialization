using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using JShibo.Serialization.Common;
using JShibo.Serialization.Soc;

namespace JShibo.Serialization
{
    internal class ShiboObjectInitializer : DeserializerBase<OValue, ObjectInitializeContext<OValue>>
    {
        static ShiboObjectInitializer Instance;

        #region 构造函数

        static ShiboObjectInitializer()
        {
            Instance = new ShiboObjectInitializer();
            Instance.builder = new SocILBuilder();
        }

        #endregion

        #region 方法

        internal static void CreateContext(Type type, ObjectInitializeContext<OValue> info)
        {
            if (Instance.builder.IsBaseType(type) == true)
                return;

            var fields = type.GetFields(info.Seting.Flags);
            foreach (FieldInfo field in fields)
            {
                if (Utils.IsIgnoreAttribute(field) == false)
                {
                    if (Utils.IsDeep(field.FieldType))
                    {
                        info.DesList.Add(Instance.GetDeserializeSurrogate(field.FieldType));
                        CreateContext(field.FieldType, info);
                    }
                }
            }
            var propertys = type.GetProperties(info.Seting.Flags);
            foreach (PropertyInfo property in propertys)
            {
                if (Utils.IsIgnoreAttribute(property) == false)
                {
                    if (Utils.IsDeep(property.PropertyType))
                    {
                        info.DesList.Add(Instance.GetDeserializeSurrogate(property.PropertyType));
                        CreateContext(property.PropertyType, info);
                    }
                }
            }
        }

        internal static ObjectInitializeContext<OValue> GetContext(Type type)
        {
            if (types.TryGetValue(type, out var info) == false)
            {
                info = new ObjectInitializeContext<OValue>();
                CreateContext(type, info);
                info.Deserializer = Instance.GetDeserializeSurrogate(type);
                types.Add(type, info);
                if (info != null)
                    info.ToArray();
            }
            return info;
        }

        #endregion

        #region 公共方法

        internal static object Initialize(OValue hander, ObjectInitializeContext<OValue> info)
        {
            hander.desers = info.Deserializes;
            object value = info.Deserializer(hander);
            return value;
        }

        internal static object Initialize(OValue hander, Deserialize<OValue> info)
        {
            object value = info(hander);
            return value;
        }

        public static object Initialize(Type type, int seed)
        {
            return Initialize(new OValue(seed), GetContext(type));
            //ObjectInitializeContext<OValue> info;
            //if (type == lastRandomType)
            //{
            //    info = lastRandomTypeInfo;
            //}
            //else
            //{
            //    info = GetContext(type);
            //    lastRandomType = type;
            //    lastRandomTypeInfo = info;
            //}
            //return Initialize(new OValue(seed), info);
        }

        public static object Initialize(Type type)
        {
            return Initialize(type, Guid.NewGuid().GetHashCode());
        }

        public static T Initialize<T>()
        {
            return Initialize<T>(Guid.NewGuid().GetHashCode());
        }

        public static T Initialize<T>(int seed)
        {
            Type type = typeof(T);
            object value = Initialize(type, seed);
            return (T)value;
        }

        #endregion

    }
}
