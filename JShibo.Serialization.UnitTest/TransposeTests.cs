using JShibo.Serialization.BenchMark.Entitiy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JShibo.Serialization.UnitTest
{
    [TestClass]
    public class TransposeTests
    {
        [TestMethod]
        public void TestInt8()
        {
            var data = new Int8Class[100];
            for (int i = 0; i < data.Length; i++)
                data[i] = ShiboSerializer.Initialize<Int8Class>();
            var ret = ShiboSerializer.ToColumns(data);
            var bytes = new byte[data.Length];
            for (int i = 0; i < data.Length; i++)
                bytes[i] = data[i].V0;
            var v1 = ServiceStack.Text.CsvSerializer.SerializeToCsv((byte[])ret[0].Value);
            var v2 = ServiceStack.Text.CsvSerializer.SerializeToCsv(bytes);
            Assert.AreEqual(v1, v2);
        }

        [TestMethod]
        public void TestInt32()
        {
            var data = new Int32Class[100];
            for (int i = 0; i < data.Length; i++)
                data[i] = ShiboSerializer.Initialize<Int32Class>();
            var ret = ShiboSerializer.ToColumns(data);
            var bytes = new int[data.Length];
            for (int i = 0; i < data.Length; i++)
                bytes[i] = data[i].V0;
            var v1 = Csv((int[])ret[0].Value);
            var v2 = Csv(bytes);
            Assert.AreEqual(v1, v2);
        }

        [TestMethod]
        public void TestInt64()
        {
            var data = new Int64Class[100];
            for (int i = 0; i < data.Length; i++)
                data[i] = ShiboSerializer.Initialize<Int64Class>();
            var ret = ShiboSerializer.ToColumns(data);
            var v0s = new long[data.Length];
            for (int i = 0; i < data.Length; i++)
                v0s[i] = data[i].V0;
            var v1 = Csv((long[])ret[0].Value);
            var v2 = Csv(v0s);
            Assert.AreEqual(v1, v2);
        }

        [TestMethod]
        public void TestAllBaseType()
        {
            var data = new TestAllBaseType[100];
            for (int i = 0; i < data.Length; i++)
                data[i] = ShiboSerializer.Initialize<TestAllBaseType>();
            var ret = ShiboSerializer.ToColumns(data);
            var boolValue = new bool[data.Length];
            var boolValueFalse = new bool[data.Length];
            var byteValue = new byte[data.Length];
            var sbyteValue = new sbyte[data.Length];
            var charValue = new char[data.Length];
            var shortValue = new short[data.Length];
            var ushortValue = new ushort[data.Length];
            var intValue = new int[data.Length];
            var uintValue = new uint[data.Length];
            var longValue = new long[data.Length];
            var ulongValue = new ulong[data.Length];
            var floatValue = new float[data.Length];
            var doubleValue = new double[data.Length];
            var decimalValue = new decimal[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                boolValue[i] = data[i].boolValue;
                boolValueFalse[i] = data[i].boolValueFalse;
                byteValue[i] = data[i].byteValue;
                sbyteValue[i] = data[i].sbyteValue;
                charValue[i] = data[i].charValue;
                shortValue[i] = data[i].shortValue;
                ushortValue[i] = data[i].ushortValue;
                intValue[i] = data[i].intValue;
                uintValue[i] = data[i].uintValue;
                longValue[i] = data[i].longValue;
                ulongValue[i] = data[i].ulongValue;
                floatValue[i] = data[i].floatValue;
                doubleValue[i] = data[i].doubleValue;
                decimalValue[i] = data[i].decimalValue;
            }
            Assert.AreEqual(Json(ret[0].Value), Json(boolValue));
            Assert.AreEqual(Json(ret[1].Value), Json(boolValueFalse));
            Assert.AreEqual(Json(ret[2].Value), Json(byteValue));
            Assert.AreEqual(Json(ret[3].Value), Json(sbyteValue));
            Assert.AreEqual(Json(ret[4].Value), Json(charValue));
            Assert.AreEqual(Json(ret[5].Value), Json(shortValue));
            Assert.AreEqual(Json(ret[6].Value), Json(ushortValue));
            Assert.AreEqual(Json(ret[7].Value), Json(intValue));
            Assert.AreEqual(Json(ret[8].Value), Json(uintValue));
            Assert.AreEqual(Json(ret[9].Value), Json(longValue));
            Assert.AreEqual(Json(ret[10].Value), Json(ulongValue));
            Assert.AreEqual(Json(ret[11].Value), Json(floatValue));
            Assert.AreEqual(Json(ret[12].Value), Json(doubleValue));
            Assert.AreEqual(Json(ret[13].Value), Json(decimalValue));
        }

        public static string Json(object o)
        {
            return JsonConvert.SerializeObject(o);
        }

        public static string Csv<T>(T[] o)
        {
            return ServiceStack.Text.CsvSerializer.SerializeToCsv(o);
        }


    }
}
