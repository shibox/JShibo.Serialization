using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JShibo.Serialization;
using JShibo.Serialization.BenchMark.Entitiy;

namespace JShibo.Serialization.UnitTest
{
    //[TestFixture]
    [TestClass]
    public class ValidityTest
    {
        //[Test]
        [TestMethod]
        public void TestInt32()
        {
            //int a = 1;
            //int b = 2;
            //int sum = a + b;

            Int32Class a = ShiboSerializer.Initialize<Int32Class>();
            byte[] buffer = ShiboSerializer.BinarySerialize(a);
            Int32Class b = null;

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(a, b); 
        }
    }
}
