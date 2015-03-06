﻿using System;
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
        public void TestGetTopSentences()
        {
            FullStory fs = FullStoryFactory.GetFullStory(9154960);
            //Dictionary<string, HashSet<int>> wordMapping = fs.WordIDMapping;
            List<string> top5Sentences = fs.GetTopSentences(5);
            Assert.AreEqual(top5Sentences.Count > 1, true);
        }

        [TestMethod]
        public void TestGetMapping()
        {
            FullStory fs = FullStoryFactory.GetFullStory(9148746);
            Dictionary<string, HashSet<int>> wordMapping = fs.WordIDMapping;
            Assert.AreEqual(wordMapping.Count>1,true);
        }

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
            Task<object> result = reader.GetTagCloudTree(9132815);
        }

        [TestMethod]
        public void GetCount()
        {
            Reader reader = new Reader();
            int count = reader.GetStoryCommentCount(9112885);
        }
    }
}
