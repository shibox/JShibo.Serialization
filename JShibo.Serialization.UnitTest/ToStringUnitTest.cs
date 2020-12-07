using JShibo.Serialization.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JShibo.Serialization.UnitTest
{
    [TestClass]
    public class ToStringUnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            char[] data = new char[20];
            int n = FastToString.ToStringFast(data, 0, (uint)123456);
            string s = new string(data, 0, n);
            Assert.AreEqual(s, "123456");

        }
    }
}
