using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using JShibo.Serialization.Common;
using JShibo.Serialization.Json;

namespace JShibo.Serialization
{
    internal class SerializerBase
    {
        internal Dictionary<Type, Type[]> deTypes;
        internal IBuilder builder = null;
        internal Type lastSerType = typeof(object);

        internal Type lastObjectLoopSerType = typeof(object);

        internal SerializerBase()
        { }
    }

    internal class SerializerBase<Data, UData, Info, Size> : SerializerBase<UData, Info>
    {
        #region 字段

        internal Dictionary<string, Type> typeNameMap;
        internal Dictionary<Type, Serialize<Data>> typeMap;
        internal Dictionary<Type, Serialize<Size>> typeSizeMap;

        internal Dictionary<Type, string[]> namesMap;
        internal Dictionary<Type, XType<Data, Info, Size>> xTypes;

        internal Info lastSerTypeInfo = default(Info);

        #endregion

        #region 构造函数

        internal SerializerBase()
        {
            if (typeNameMap == null)
                typeNameMap = new Dictionary<string, Type>();

            //if (deTypes == null)
            //    deTypes = new Dictionary<Type, Type[]>();

            if (namesMap == null)
                namesMap = new Dictionary<Type, string[]>();

            if (typeMap == null)
                typeMap = new Dictionary<Type, Serialize<Data>>();

            //if (types == null)
            //    types = new Dictionary<Type, Info>();

            if (typeSizeMap == null)
                typeSizeMap = new Dictionary<Type, Serialize<Size>>();

            //if (deTypeMap == null)
            //    deTypeMap = new Dictionary<Type, Deserialize<UData>>();

            if (xTypes == null)
                xTypes = new Dictionary<Type, XType<Data, Info, Size>>();
        }

        #endregion

        #region 方法

        internal void RegisterAssemblyTypes()
        {
            try
            {
                AssemblyName[] assembly = Assembly.GetEntryAssembly().GetReferencedAssemblies();
                foreach (AssemblyName ass in assembly)
                {
                    AppDomain.CurrentDomain.Load(ass);
                }

                Assembly[] asmbs = AppDomain.CurrentDomain.GetAssemblies();
                foreach (Assembly a in asmbs)
                {
                    if (Utils.IsTypeDecoratedByAttribute<TraceAssembly>(a.GetCustomAttributes(false)))
                    {
                        foreach (Type t in a.GetTypes())
                        {
                            if (Utils.IsTypeDecoratedByAttribute<SerializableAttribute>(t.GetCustomAttributes(true)))
                            {
                                //GenerateSurrogateForEvent(t);
                                GenerateDataSerializeSurrogate(t, true);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Serialize<Data> GenerateDataSerializeSurrogate(Type type, bool check)
        {
            bool isWrite = false;
            Serialize<Data> jsonser = null;
            if (check == true)
            {
                if (!type.IsAbstract && Utils.HasSerializableAttribute(type.GetCustomAttributes(true)))
                    isWrite = true;
            }
            if (isWrite == false && !type.IsAbstract)
            {
                jsonser = builder.GenerateSerializationType<Data>(type);
                isWrite = true;
            }

            if (!typeMap.ContainsKey(type) && isWrite == true)
                typeMap.Add(type, jsonser);

            return jsonser;
        }

        private Serialize<Size> GenerateSizeSerializeSurrogate(Type type, bool check)
        {
            bool isWrite = false;
            Serialize<Size> jsonser = null;
            if (check == true)
            {
                if (!type.IsAbstract && Utils.HasSerializableAttribute(type.GetCustomAttributes(true)))
                    isWrite = true;
            }
            if (isWrite == false && !type.IsAbstract)
            {
                jsonser = builder.GenerateSizeSerializationType<Size>(type);
                isWrite = true;
            }

            if (!typeSizeMap.ContainsKey(type) && isWrite == true)
                typeSizeMap.Add(type, jsonser);

            return jsonser;
        }

        //private static Deserialize<UData> GenerateDeserializeSurrogate(Type type, bool check)
        //{
        //    bool isWrite = false;
        //    Deserialize<UData> jsonser = null;
        //    if (check == true)
        //    {
        //        if (!type.IsAbstract && Utils.HasSerializableAttribute(type.GetCustomAttributes(true)))
        //            isWrite = true;
        //    }
        //    if (isWrite == false && !type.IsAbstract)
        //    {
        //        jsonser = builder.GenerateDeserializationType<UData>(type);
        //        isWrite = true;
        //    }

        //    if (!deTypeMap.ContainsKey(type) && isWrite == true)
        //        deTypeMap.Add(type, jsonser);

        //    return jsonser;
        //}

        internal Serialize<Data> GenerateDataSerializeSurrogate(Type type)
        {
            Serialize<Data> sr = null;
            if (typeMap.TryGetValue(type, out sr) == false)
            {
                sr = GenerateDataSerializeSurrogate(type, false);
            }
            return sr;
        }

        internal Serialize<Size> GenerateSizeSerializeSurrogate(Type type)
        {
            Serialize<Size> sr = null;
            if (typeSizeMap.TryGetValue(type, out sr) == false)
            {
                sr = GenerateSizeSerializeSurrogate(type, false);
            }
            return sr;
        }

        //internal static Deserialize<UData> GetDeserializeSurrogate(Type type)
        //{
        //    Deserialize<UData> sr = null;
        //    if (deTypeMap.TryGetValue(type, out sr) == false)
        //    {
        //        sr = GenerateDeserializeSurrogate(type, false);
        //    }
        //    return sr;
        //}

        protected Type[] GetDeserializeTypes(Type type)
        {
            Type[] sr = null;
            if (deTypes.TryGetValue(type, out sr) == false)
            {
                List<Type> types = new List<Type>();
                Utils.GetTypes(type, types);
                sr = types.ToArray();
                deTypes.Add(type, sr);
            }
            return sr;
        }

        internal string[] GetSerializeNames(Type type)
        {
            string[] sr = null;
            if (namesMap.TryGetValue(type, out sr) == false)
            {
                List<string> types = new List<string>();
                Utils.GetNames(type, types);
                sr = types.ToArray();
                namesMap.Add(type, sr);
            }
            return sr;
        }

        #endregion

        #region 字段

        //internal static Dictionary<string, Type> typeNameMap;
        //internal static Dictionary<Type, Type[]> deTypes;
        //internal static Dictionary<Type, Serialize<Data>> jsonTypeMap;
        //internal static Dictionary<Type, Serialize<Size>> jsonTypeSizeMap;
        //internal static Dictionary<Type, Deserialize<UData>> jsonDeTypeMap;
        //internal static Dictionary<Type, Info> jsonTypes;
        //internal static Dictionary<Type, string[]> namesMap;
        //internal static Dictionary<Type, XType<Data, Info, Size>> xtype;

        //internal static Info lastTypeInfo = default(Info);
        //internal static Type lastSerType = typeof(object);

        //internal static Type lastObjectLoopSerType = typeof(object);
        //internal static Info lastObjectLoopTypeInfo = default(Info);


        //#endregion

        //#region 构造函数

        //static SerializerBase()
        //{
        //    if (typeNameMap == null)
        //        typeNameMap = new Dictionary<string, Type>();

        //    if (deTypes == null)
        //        deTypes = new Dictionary<Type, Type[]>();

        //    if (namesMap == null)
        //        namesMap = new Dictionary<Type, string[]>();

        //    if (jsonTypeMap == null)
        //        jsonTypeMap = new Dictionary<Type, Serialize<Data>>();

        //    if (jsonTypes == null)
        //        jsonTypes = new Dictionary<Type, Info>();

        //    if (jsonTypeSizeMap == null)
        //        jsonTypeSizeMap = new Dictionary<Type, Serialize<Size>>();

        //    if (jsonDeTypeMap == null)
        //        jsonDeTypeMap = new Dictionary<Type, Deserialize<UData>>();

        //    if (xtype == null)
        //        xtype = new Dictionary<Type, XType<Data, Info, Size>>();

        //    RegisterAssemblyTypes();
        //}

        //#endregion

        //#region 方法

        //private static void RegisterAssemblyTypes()
        //{
        //    try
        //    {
        //        AssemblyName[] assembly = Assembly.GetEntryAssembly().GetReferencedAssemblies();
        //        foreach (AssemblyName ass in assembly)
        //        {
        //            AppDomain.CurrentDomain.Load(ass);
        //        }

        //        Assembly[] asmbs = AppDomain.CurrentDomain.GetAssemblies();
        //        foreach (Assembly a in asmbs)
        //        {
        //            if (Utils.IsTypeDecoratedByAttribute<TraceAssembly>(a.GetCustomAttributes(false)))
        //            {
        //                foreach (Type t in a.GetTypes())
        //                {
        //                    if (Utils.IsTypeDecoratedByAttribute<SerializableAttribute>(t.GetCustomAttributes(true)))
        //                    {
        //                        //GenerateSurrogateForEvent(t);
        //                        GenerateDataSerializeSurrogate(t,true);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //private static Serialize<Data> GenerateDataSerializeSurrogate(Type type, bool check)
        //{
        //    bool isWrite = false;
        //    Serialize<Data> jsonser = null;
        //    if (check == true)
        //    {
        //        if (!type.IsAbstract && Utils.HasSerializableAttribute(type.GetCustomAttributes(true)))
        //            isWrite = true;
        //    }
        //    if (isWrite == false && !type.IsAbstract)
        //    {
        //        jsonser = ILBuilder.GenerateSerializationType<Data>(type);
        //        isWrite = true;
        //    }

        //    if (!jsonTypeMap.ContainsKey(type) && isWrite == true)
        //        jsonTypeMap.Add(type, jsonser);

        //    return jsonser;
        //}

        //private static Serialize<Size> GenerateSizeSerializeSurrogate(Type type, bool check)
        //{
        //    bool isWrite = false;
        //    Serialize<Size> jsonser = null;
        //    if (check == true)
        //    {
        //        if (!type.IsAbstract && Utils.HasSerializableAttribute(type.GetCustomAttributes(true)))
        //            isWrite = true;
        //    }
        //    if (isWrite == false && !type.IsAbstract)
        //    {
        //        jsonser = ILBuilder.GenerateSerializationType<Size>(type);
        //        isWrite = true;
        //    }

        //    if (!jsonTypeSizeMap.ContainsKey(type) && isWrite == true)
        //        jsonTypeSizeMap.Add(type, jsonser);

        //    return jsonser;
        //}

        //private static Deserialize<UData> GenerateDeserializeSurrogateForEvent(Type type, bool check)
        //{
        //    bool isWrite = false;
        //    Deserialize<UData> jsonser = null;
        //    if (check == true)
        //    {
        //        if (!type.IsAbstract && Utils.HasSerializableAttribute(type.GetCustomAttributes(true)))
        //            isWrite = true;
        //    }
        //    if (isWrite == false && !type.IsAbstract)
        //    {
        //        jsonser = ILBuilder.GenerateDeserializationType<UData>(type);
        //        isWrite = true;
        //    }

        //    if (!jsonDeTypeMap.ContainsKey(type) && isWrite == true)
        //        jsonDeTypeMap.Add(type, jsonser);

        //    return jsonser;
        //}

        //internal static Serialize<Data> GenerateDataSerializeSurrogate(Type type)
        //{
        //    Serialize<Data> sr = null;
        //    if (jsonTypeMap.TryGetValue(type, out sr) == false)
        //    {
        //        sr = GenerateDataSerializeSurrogate(type, false);
        //    }
        //    return sr;
        //}

        //internal static Serialize<Size> GenerateSizeSerializeSurrogate(Type type)
        //{
        //    Serialize<Size> sr = null;
        //    if (jsonTypeSizeMap.TryGetValue(type, out sr) == false)
        //    {
        //        sr = GenerateSizeSerializeSurrogate(type, false);
        //    }
        //    return sr;
        //}

        //internal static Deserialize<UData> GetJsonDeserializeSurrogateFromType(Type type)
        //{
        //    Deserialize<UData> sr = null;
        //    if (jsonDeTypeMap.TryGetValue(type, out sr) == false)
        //    {
        //        sr = GenerateDeserializeSurrogateForEvent(type, false);
        //    }
        //    return sr;
        //}

        //protected static Type[] GetDeserializeTypes(Type type)
        //{
        //    Type[] sr = null;
        //    if (deTypes.TryGetValue(type, out sr) == false)
        //    {
        //        List<Type> types = new List<Type>();
        //        Utils.GetTypes(type, types);
        //        sr = types.ToArray();
        //        deTypes.Add(type, sr);
        //    }
        //    return sr;
        //}

        //internal static string[] GetSerializeNames(Type type)
        //{
        //    string[] sr = null;
        //    if (namesMap.TryGetValue(type, out sr) == false)
        //    {
        //        List<string> types = new List<string>();
        //        Utils.GetNames(type, types);
        //        sr = types.ToArray();
        //        namesMap.Add(type, sr);
        //    }
        //    return sr;
        //}

        #endregion

    }

    internal class SerializerBase<UData, Info> : SerializerBase
    {
        internal static Dictionary<Type, Info> types;
        internal static Dictionary<Type, Deserialize<UData>> deTypeMap;
        internal static Info lastRandomTypeInfo = default(Info);
        internal static Type lastRandomType = typeof(object);
        internal static Info lastObjectLoopTypeInfo = default(Info);

        internal SerializerBase()
        {
            if (deTypes == null)
                deTypes = new Dictionary<Type, Type[]>();

            if (types == null)
                types = new Dictionary<Type, Info>();

            if (deTypeMap == null)
                deTypeMap = new Dictionary<Type, Deserialize<UData>>();
        }

        private Deserialize<UData> GenerateDeserializeSurrogate(Type type, bool check)
        {
            bool isWrite = false;
            Deserialize<UData> jsonser = null;
            if (check == true)
            {
                if (!type.IsAbstract && Utils.HasSerializableAttribute(type.GetCustomAttributes(true)))
                    isWrite = true;
            }
            if (isWrite == false && !type.IsAbstract)
            {
                jsonser = builder.GenerateDeserializationType<UData>(type);
                isWrite = true;
            }

            if (!deTypeMap.ContainsKey(type) && isWrite == true)
                deTypeMap.Add(type, jsonser);

            return jsonser;
        }

        internal Deserialize<UData> GetDeserializeSurrogate(Type type)
        {
            Deserialize<UData> sr = null;
            if (deTypeMap.TryGetValue(type, out sr) == false)
            {
                sr = GenerateDeserializeSurrogate(type, false);
            }
            return sr;
        }


    }

    #region static

    //internal class SerializerBase
    //{
    //    internal static Dictionary<Type, Type[]> deTypes;
    //    internal static IBuilder builder = null;
    //    internal static Type lastSerType = typeof(object);

    //    internal static Type lastObjectLoopSerType = typeof(object);

    //}

    //internal class SerializerBase<Data, UData, Info, Size> : SerializerBase<UData,Info>
    //{
    //    #region 字段

    //    internal static Dictionary<string, Type> typeNameMap;
    //    internal static Dictionary<Type, Serialize<Data>> typeMap;
    //    internal static Dictionary<Type, Serialize<Size>> typeSizeMap;

    //    internal static Dictionary<Type, string[]> namesMap;
    //    internal static Dictionary<Type, XType<Data, Info, Size>> xTypes;

    //    internal static Info lastSerTypeInfo = default(Info);

    //    #endregion

    //    #region 构造函数

    //    static SerializerBase()
    //    {
    //        if (typeNameMap == null)
    //            typeNameMap = new Dictionary<string, Type>();

    //        //if (deTypes == null)
    //        //    deTypes = new Dictionary<Type, Type[]>();

    //        if (namesMap == null)
    //            namesMap = new Dictionary<Type, string[]>();

    //        if (typeMap == null)
    //            typeMap = new Dictionary<Type, Serialize<Data>>();

    //        //if (types == null)
    //        //    types = new Dictionary<Type, Info>();

    //        if (typeSizeMap == null)
    //            typeSizeMap = new Dictionary<Type, Serialize<Size>>();

    //        //if (deTypeMap == null)
    //        //    deTypeMap = new Dictionary<Type, Deserialize<UData>>();

    //        if (xTypes == null)
    //            xTypes = new Dictionary<Type, XType<Data, Info, Size>>();
    //    }

    //    #endregion

    //    #region 方法

    //    internal static void RegisterAssemblyTypes()
    //    {
    //        try
    //        {
    //            AssemblyName[] assembly = Assembly.GetEntryAssembly().GetReferencedAssemblies();
    //            foreach (AssemblyName ass in assembly)
    //            {
    //                AppDomain.CurrentDomain.Load(ass);
    //            }

    //            Assembly[] asmbs = AppDomain.CurrentDomain.GetAssemblies();
    //            foreach (Assembly a in asmbs)
    //            {
    //                if (Utils.IsTypeDecoratedByAttribute<TraceAssembly>(a.GetCustomAttributes(false)))
    //                {
    //                    foreach (Type t in a.GetTypes())
    //                    {
    //                        if (Utils.IsTypeDecoratedByAttribute<SerializableAttribute>(t.GetCustomAttributes(true)))
    //                        {
    //                            //GenerateSurrogateForEvent(t);
    //                            GenerateDataSerializeSurrogate(t, true);
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    private static Serialize<Data> GenerateDataSerializeSurrogate(Type type, bool check)
    //    {
    //        bool isWrite = false;
    //        Serialize<Data> jsonser = null;
    //        if (check == true)
    //        {
    //            if (!type.IsAbstract && Utils.HasSerializableAttribute(type.GetCustomAttributes(true)))
    //                isWrite = true;
    //        }
    //        if (isWrite == false && !type.IsAbstract)
    //        {
    //            jsonser = builder.GenerateSerializationType<Data>(type);
    //            isWrite = true;
    //        }

    //        if (!typeMap.ContainsKey(type) && isWrite == true)
    //            typeMap.Add(type, jsonser);

    //        return jsonser;
    //    }

    //    private static Serialize<Size> GenerateSizeSerializeSurrogate(Type type, bool check)
    //    {
    //        bool isWrite = false;
    //        Serialize<Size> jsonser = null;
    //        if (check == true)
    //        {
    //            if (!type.IsAbstract && Utils.HasSerializableAttribute(type.GetCustomAttributes(true)))
    //                isWrite = true;
    //        }
    //        if (isWrite == false && !type.IsAbstract)
    //        {
    //            jsonser = builder.GenerateSizeSerializationType<Size>(type);
    //            isWrite = true;
    //        }

    //        if (!typeSizeMap.ContainsKey(type) && isWrite == true)
    //            typeSizeMap.Add(type, jsonser);

    //        return jsonser;
    //    }

    //    //private static Deserialize<UData> GenerateDeserializeSurrogate(Type type, bool check)
    //    //{
    //    //    bool isWrite = false;
    //    //    Deserialize<UData> jsonser = null;
    //    //    if (check == true)
    //    //    {
    //    //        if (!type.IsAbstract && Utils.HasSerializableAttribute(type.GetCustomAttributes(true)))
    //    //            isWrite = true;
    //    //    }
    //    //    if (isWrite == false && !type.IsAbstract)
    //    //    {
    //    //        jsonser = builder.GenerateDeserializationType<UData>(type);
    //    //        isWrite = true;
    //    //    }

    //    //    if (!deTypeMap.ContainsKey(type) && isWrite == true)
    //    //        deTypeMap.Add(type, jsonser);

    //    //    return jsonser;
    //    //}

    //    internal static Serialize<Data> GenerateDataSerializeSurrogate(Type type)
    //    {
    //        Serialize<Data> sr = null;
    //        if (typeMap.TryGetValue(type, out sr) == false)
    //        {
    //            sr = GenerateDataSerializeSurrogate(type, false);
    //        }
    //        return sr;
    //    }

    //    internal static Serialize<Size> GenerateSizeSerializeSurrogate(Type type)
    //    {
    //        Serialize<Size> sr = null;
    //        if (typeSizeMap.TryGetValue(type, out sr) == false)
    //        {
    //            sr = GenerateSizeSerializeSurrogate(type, false);
    //        }
    //        return sr;
    //    }

    //    //internal static Deserialize<UData> GetDeserializeSurrogate(Type type)
    //    //{
    //    //    Deserialize<UData> sr = null;
    //    //    if (deTypeMap.TryGetValue(type, out sr) == false)
    //    //    {
    //    //        sr = GenerateDeserializeSurrogate(type, false);
    //    //    }
    //    //    return sr;
    //    //}

    //    protected static Type[] GetDeserializeTypes(Type type)
    //    {
    //        Type[] sr = null;
    //        if (deTypes.TryGetValue(type, out sr) == false)
    //        {
    //            List<Type> types = new List<Type>();
    //            Utils.GetTypes(type, types);
    //            sr = types.ToArray();
    //            deTypes.Add(type, sr);
    //        }
    //        return sr;
    //    }

    //    internal static string[] GetSerializeNames(Type type)
    //    {
    //        string[] sr = null;
    //        if (namesMap.TryGetValue(type, out sr) == false)
    //        {
    //            List<string> types = new List<string>();
    //            Utils.GetNames(type, types);
    //            sr = types.ToArray();
    //            namesMap.Add(type, sr);
    //        }
    //        return sr;
    //    }

    //    #endregion

    //    #region 字段

    //    //internal static Dictionary<string, Type> typeNameMap;
    //    //internal static Dictionary<Type, Type[]> deTypes;
    //    //internal static Dictionary<Type, Serialize<Data>> jsonTypeMap;
    //    //internal static Dictionary<Type, Serialize<Size>> jsonTypeSizeMap;
    //    //internal static Dictionary<Type, Deserialize<UData>> jsonDeTypeMap;
    //    //internal static Dictionary<Type, Info> jsonTypes;
    //    //internal static Dictionary<Type, string[]> namesMap;
    //    //internal static Dictionary<Type, XType<Data, Info, Size>> xtype;

    //    //internal static Info lastTypeInfo = default(Info);
    //    //internal static Type lastSerType = typeof(object);

    //    //internal static Type lastObjectLoopSerType = typeof(object);
    //    //internal static Info lastObjectLoopTypeInfo = default(Info);


    //    //#endregion

    //    //#region 构造函数

    //    //static SerializerBase()
    //    //{
    //    //    if (typeNameMap == null)
    //    //        typeNameMap = new Dictionary<string, Type>();

    //    //    if (deTypes == null)
    //    //        deTypes = new Dictionary<Type, Type[]>();

    //    //    if (namesMap == null)
    //    //        namesMap = new Dictionary<Type, string[]>();

    //    //    if (jsonTypeMap == null)
    //    //        jsonTypeMap = new Dictionary<Type, Serialize<Data>>();

    //    //    if (jsonTypes == null)
    //    //        jsonTypes = new Dictionary<Type, Info>();

    //    //    if (jsonTypeSizeMap == null)
    //    //        jsonTypeSizeMap = new Dictionary<Type, Serialize<Size>>();

    //    //    if (jsonDeTypeMap == null)
    //    //        jsonDeTypeMap = new Dictionary<Type, Deserialize<UData>>();

    //    //    if (xtype == null)
    //    //        xtype = new Dictionary<Type, XType<Data, Info, Size>>();

    //    //    RegisterAssemblyTypes();
    //    //}

    //    //#endregion

    //    //#region 方法

    //    //private static void RegisterAssemblyTypes()
    //    //{
    //    //    try
    //    //    {
    //    //        AssemblyName[] assembly = Assembly.GetEntryAssembly().GetReferencedAssemblies();
    //    //        foreach (AssemblyName ass in assembly)
    //    //        {
    //    //            AppDomain.CurrentDomain.Load(ass);
    //    //        }

    //    //        Assembly[] asmbs = AppDomain.CurrentDomain.GetAssemblies();
    //    //        foreach (Assembly a in asmbs)
    //    //        {
    //    //            if (Utils.IsTypeDecoratedByAttribute<TraceAssembly>(a.GetCustomAttributes(false)))
    //    //            {
    //    //                foreach (Type t in a.GetTypes())
    //    //                {
    //    //                    if (Utils.IsTypeDecoratedByAttribute<SerializableAttribute>(t.GetCustomAttributes(true)))
    //    //                    {
    //    //                        //GenerateSurrogateForEvent(t);
    //    //                        GenerateDataSerializeSurrogate(t,true);
    //    //                    }
    //    //                }
    //    //            }
    //    //        }
    //    //    }
    //    //    catch (Exception ex)
    //    //    {
    //    //        throw ex;
    //    //    }
    //    //}

    //    //private static Serialize<Data> GenerateDataSerializeSurrogate(Type type, bool check)
    //    //{
    //    //    bool isWrite = false;
    //    //    Serialize<Data> jsonser = null;
    //    //    if (check == true)
    //    //    {
    //    //        if (!type.IsAbstract && Utils.HasSerializableAttribute(type.GetCustomAttributes(true)))
    //    //            isWrite = true;
    //    //    }
    //    //    if (isWrite == false && !type.IsAbstract)
    //    //    {
    //    //        jsonser = ILBuilder.GenerateSerializationType<Data>(type);
    //    //        isWrite = true;
    //    //    }

    //    //    if (!jsonTypeMap.ContainsKey(type) && isWrite == true)
    //    //        jsonTypeMap.Add(type, jsonser);

    //    //    return jsonser;
    //    //}

    //    //private static Serialize<Size> GenerateSizeSerializeSurrogate(Type type, bool check)
    //    //{
    //    //    bool isWrite = false;
    //    //    Serialize<Size> jsonser = null;
    //    //    if (check == true)
    //    //    {
    //    //        if (!type.IsAbstract && Utils.HasSerializableAttribute(type.GetCustomAttributes(true)))
    //    //            isWrite = true;
    //    //    }
    //    //    if (isWrite == false && !type.IsAbstract)
    //    //    {
    //    //        jsonser = ILBuilder.GenerateSerializationType<Size>(type);
    //    //        isWrite = true;
    //    //    }

    //    //    if (!jsonTypeSizeMap.ContainsKey(type) && isWrite == true)
    //    //        jsonTypeSizeMap.Add(type, jsonser);

    //    //    return jsonser;
    //    //}

    //    //private static Deserialize<UData> GenerateDeserializeSurrogateForEvent(Type type, bool check)
    //    //{
    //    //    bool isWrite = false;
    //    //    Deserialize<UData> jsonser = null;
    //    //    if (check == true)
    //    //    {
    //    //        if (!type.IsAbstract && Utils.HasSerializableAttribute(type.GetCustomAttributes(true)))
    //    //            isWrite = true;
    //    //    }
    //    //    if (isWrite == false && !type.IsAbstract)
    //    //    {
    //    //        jsonser = ILBuilder.GenerateDeserializationType<UData>(type);
    //    //        isWrite = true;
    //    //    }

    //    //    if (!jsonDeTypeMap.ContainsKey(type) && isWrite == true)
    //    //        jsonDeTypeMap.Add(type, jsonser);

    //    //    return jsonser;
    //    //}

    //    //internal static Serialize<Data> GenerateDataSerializeSurrogate(Type type)
    //    //{
    //    //    Serialize<Data> sr = null;
    //    //    if (jsonTypeMap.TryGetValue(type, out sr) == false)
    //    //    {
    //    //        sr = GenerateDataSerializeSurrogate(type, false);
    //    //    }
    //    //    return sr;
    //    //}

    //    //internal static Serialize<Size> GenerateSizeSerializeSurrogate(Type type)
    //    //{
    //    //    Serialize<Size> sr = null;
    //    //    if (jsonTypeSizeMap.TryGetValue(type, out sr) == false)
    //    //    {
    //    //        sr = GenerateSizeSerializeSurrogate(type, false);
    //    //    }
    //    //    return sr;
    //    //}

    //    //internal static Deserialize<UData> GetJsonDeserializeSurrogateFromType(Type type)
    //    //{
    //    //    Deserialize<UData> sr = null;
    //    //    if (jsonDeTypeMap.TryGetValue(type, out sr) == false)
    //    //    {
    //    //        sr = GenerateDeserializeSurrogateForEvent(type, false);
    //    //    }
    //    //    return sr;
    //    //}

    //    //protected static Type[] GetDeserializeTypes(Type type)
    //    //{
    //    //    Type[] sr = null;
    //    //    if (deTypes.TryGetValue(type, out sr) == false)
    //    //    {
    //    //        List<Type> types = new List<Type>();
    //    //        Utils.GetTypes(type, types);
    //    //        sr = types.ToArray();
    //    //        deTypes.Add(type, sr);
    //    //    }
    //    //    return sr;
    //    //}

    //    //internal static string[] GetSerializeNames(Type type)
    //    //{
    //    //    string[] sr = null;
    //    //    if (namesMap.TryGetValue(type, out sr) == false)
    //    //    {
    //    //        List<string> types = new List<string>();
    //    //        Utils.GetNames(type, types);
    //    //        sr = types.ToArray();
    //    //        namesMap.Add(type, sr);
    //    //    }
    //    //    return sr;
    //    //}

    //    #endregion

    //}

    //internal class SerializerBase<UData, Info> : SerializerBase
    //{
    //    internal static Dictionary<Type, Info> types;
    //    internal static Dictionary<Type, Deserialize<UData>> deTypeMap;
    //    internal static Info lastRandomTypeInfo = default(Info);
    //    internal static Type lastRandomType = typeof(object);
    //    internal static Info lastObjectLoopTypeInfo = default(Info);

    //    static SerializerBase()
    //    {
    //        if (deTypes == null)
    //            deTypes = new Dictionary<Type, Type[]>();

    //        if (types == null)
    //            types = new Dictionary<Type, Info>();

    //        if (deTypeMap == null)
    //            deTypeMap = new Dictionary<Type, Deserialize<UData>>();
    //    }

    //    private static Deserialize<UData> GenerateDeserializeSurrogate(Type type, bool check)
    //    {
    //        bool isWrite = false;
    //        Deserialize<UData> jsonser = null;
    //        if (check == true)
    //        {
    //            if (!type.IsAbstract && Utils.HasSerializableAttribute(type.GetCustomAttributes(true)))
    //                isWrite = true;
    //        }
    //        if (isWrite == false && !type.IsAbstract)
    //        {
    //            jsonser = builder.GenerateDeserializationType<UData>(type);
    //            isWrite = true;
    //        }

    //        if (!deTypeMap.ContainsKey(type) && isWrite == true)
    //            deTypeMap.Add(type, jsonser);

    //        return jsonser;
    //    }

    //    internal static Deserialize<UData> GetDeserializeSurrogate(Type type)
    //    {
    //        Deserialize<UData> sr = null;
    //        if (deTypeMap.TryGetValue(type, out sr) == false)
    //        {
    //            sr = GenerateDeserializeSurrogate(type, false);
    //        }
    //        return sr;
    //    }


    //}

    #endregion

    
}
