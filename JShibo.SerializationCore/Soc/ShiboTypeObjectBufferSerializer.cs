using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using JShibo.Serialization.Common;
//using System.Threading.Tasks;

namespace JShibo.Serialization.Soc
{
    internal class ShiboTypeObjectBufferSerializer : SerializerBase<ObjectBuffer, ObjectUbuffer, ObjectBufferContext, ObjectBufferSize>
    {
        static ShiboTypeObjectBufferSerializer Instance;

        static ShiboTypeObjectBufferSerializer()
        {
            Instance = new ShiboTypeObjectBufferSerializer();
            Instance.builder = new SocILBuilder();
            Instance.RegisterAssemblyTypes();
        }

        internal static void GetJsonTypes(Type type, ObjectBufferContext info)
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
                        GetJsonTypes(field.FieldType, info);
                    }
                    else
                        info.MinSize += Instance.builder.GetSize(field.FieldType) + 1;
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
                        GetJsonTypes(property.PropertyType, info);
                    }
                    else
                        info.MinSize += Instance.builder.GetSize(property.PropertyType) + 1;
                }
            }
        }

        internal static ObjectBufferContext GetJsonTypes(Type type)
        {
            ObjectBufferContext info = null;
            if (types.TryGetValue(type, out info) == false)
            {
                info = new ObjectBufferContext();
                GetJsonTypes(type, info);
                info.Serializer = Instance.GenerateDataSerializeSurrogate(type);
                //info.Deserialize = GetJsonDeserializeSurrogateFromType(type);
                types.Add(type, info);
                if (info != null)
                    info.ToArray();
            }
            return info;
        }
    }
}
