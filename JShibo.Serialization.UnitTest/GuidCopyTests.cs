using JShibo.Serialization.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JShibo.Serialization.UnitTest
{
    [TestClass]
    public class GuidCopyTests
    {

        [TestMethod]
        public unsafe void CopyTest()
        {
            var expected = Guid.NewGuid().ToString();
            var buffer = new char[100];
            fixed (char* pd = expected, ptr = buffer)
            {
                Utils.FastCopyGuid(ptr, pd);
            }
            var actual = new string(buffer, 0, 36);
            Assert.AreEqual(expected, actual);
        }

    }
}
