﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics;
using System.Text;
using System.Threading.Tasks;
using XShibo.Text;
using XShibo.Text.EditDistance;

namespace JShibo.Serialization.UnitTest
{
    [TestClass]
    public unsafe class XDistanceTests
    {

        [TestMethod]
        public void Stops_At_Value1_Lenght_Test()
        {
            Test("test", "test ignored", 0);

            var v = 0x8000000000000000L;


        }

        [TestMethod]
        public void Shorter_Value2_Is_Distance_Test()
        {
            Test("test", "tes", 1);
        }

        [TestMethod]
        public void Deletion_Adds_Two_Distance_Test()
        {
            Test("test", "tst ignored", 2); // deletion
        }

        [TestMethod]
        public void Subsitation_Adds_One_Distance_Test()
        {
            Test("test", "tast ignored", 1); // subsitation
        }

        [TestMethod]
        public void Addation_Adds_Two_Distance_Test()
        {
            Test("teest", "test ignored", 2); // addation
        }

        [TestMethod]
        public void Transposition_Adds_Two_Distance_Test()
        {
            Test("test", "tset ignored", 2); // transposition
        }

        [TestMethod]
        public void Different_Case_Adds_One_Distance_Test()
        {
            Test("test", "Test", 1); // case 
        }

        [TestMethod]
        public void EmtpyString_Returns_Length_Test()
        {
            Test("test", string.Empty, 4);
            Test(string.Empty, "test", 0);
        }

        [TestMethod]
        public void EmtpyStrings_Returns_Zero_Test()
        {
            Test(string.Empty, string.Empty, 0);
        }

        private void Test(string value1, string value2, int expected)
        {
            int actual = XDistance.AutoCompleteDistance(value1, value2);
            Assert.AreEqual(expected, actual);
        }



    }
}
