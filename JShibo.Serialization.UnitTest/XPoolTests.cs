using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace JShibo.Serialization.UnitTest
{
    [TestClass]
    public class XPoolTests
    {
        [TestMethod]
        public void RentReturnTest()
        {
            var bytes = XPool<byte>.Shared.Rent(128);
            Console.WriteLine(bytes);
            XPool<byte>.Shared.Return(bytes);
            //ArrayPool<byte>.Shared.Rent();
        }

        [TestMethod]
        public void ReturnTest()
        {
            for (int i = 0; i < 100; i++)
            {
                var bytes = new byte[128];
                Random.Shared.NextBytes(bytes);
                //ArrayPool<byte>.Shared.Return(bytes);
                XPool<byte>.Shared.Return(bytes);
            }
            
        }

        [TestMethod]
        public void SelectBucketIndexTest()
        {
            for (int i = 16; i < 100_000_000; i++)
            {
                var minimumLength = i;
                var v1 = 28 - Lzcnt.LeadingZeroCount((uint)minimumLength);
                if (minimumLength == (8 << (int)v1))
                    v1--;
                var v2 = (uint)BitOperations.Log2((uint)minimumLength - 1 | 15) - 3;
                var v3 = 28 - Lzcnt.LeadingZeroCount(((uint)i - 1) | 15);
                Assert.AreEqual(v1, v2);
                Assert.AreEqual(v1, v3);
            }
        }

        [TestMethod]
        public void SelectBucketIndexTest2()
        {
            for (int i = 0; i < 100_000_000; i++)
            {
                var minimumLength = i;
                var expected = (uint)BitOperations.Log2((uint)minimumLength - 1 | 15) - 3;
                var actual = 28 - Lzcnt.LeadingZeroCount(((uint)i - 1) | 15);
                Assert.AreEqual(expected, actual);
                
            }
        }


    }
}
