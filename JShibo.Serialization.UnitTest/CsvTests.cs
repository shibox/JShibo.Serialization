using JShibo.Serialization.BenchMark.Entitiy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JShibo.Serialization.UnitTest
{
    [TestClass]
    public class CsvTests
    {
        [TestMethod]
        public void TestInt32()
        {
            var data = new Int32Class[10];
            for (int i = 0; i < data.Length; i++)
                data[i] = ShiboSerializer.Initialize<Int32Class>();
            var expected = ServiceStack.Text.CsvSerializer.SerializeToCsv(data);
            var actual = ShiboSerializer.ToCsv(data);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestInt32Bytes()
        {
            var data = new Int32Class[10];
            for (int i = 0; i < data.Length; i++)
                data[i] = ShiboSerializer.Initialize<Int32Class>();
            var expected = ServiceStack.Text.CsvSerializer.SerializeToCsv(data);
            var actual = Encoding.UTF8.GetString(ShiboSerializer.ToCsvUtf8(data));
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestInt8()
        {
            var data = new Int8Class[100];
            for (int i = 0; i < data.Length; i++)
                data[i] = ShiboSerializer.Initialize<Int8Class>();
            var expected = ServiceStack.Text.CsvSerializer.SerializeToCsv(data);
            var actual = ShiboSerializer.ToCsv(data);
            Assert.AreEqual(expected, actual);
        }

    }
}
