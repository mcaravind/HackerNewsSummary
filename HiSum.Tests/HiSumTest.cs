using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HiSum;

namespace HiSum.Tests
{
    [TestClass]
    public class HiSumTest
    {
        [TestMethod]
        public void TestGetStories()
        {
            Reader reader = new Reader();
            Task<object> result = reader.GetFrontPage(null);
        }

        [TestMethod]
        public void TestGetTagCloudTree()
        {
            Reader reader = new Reader();
            Task<object> result = reader.GetTagCloudTree(9117028);
        }
    }
}
