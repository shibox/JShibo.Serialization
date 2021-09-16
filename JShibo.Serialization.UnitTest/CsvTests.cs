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
            var csv = ShiboSerializer.ToCsv(data);
            var csv2 = ServiceStack.Text.CsvSerializer.SerializeToCsv(data);
            Assert.AreEqual(csv, csv2);
        }

        [TestMethod]
        public void TestInt8()
        {
            var data = new Int8Class[100];
            for (int i = 0; i < data.Length; i++)
                data[i] = ShiboSerializer.Initialize<Int8Class>();
            var csv = ShiboSerializer.ToCsv(data);
            var csv2 = ServiceStack.Text.CsvSerializer.SerializeToCsv(data);
            Assert.AreEqual(csv, csv2);
        }

    }
}
