using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HiSum;
using NLTKSharp;

namespace HiSum.Tests
{
    [TestClass]
    public class HiSumTest
    {
        [TestMethod]
        public void GetArchive()
        {
            Reader reader = new Reader();
            var archives = reader.GetArchivedStories(1);
        }

        [TestMethod]
        public void TestGetAnchorWords()
        {
            FullStory fs = FullStoryFactory.GetFullStory(9272275);
            //Dictionary<string, HashSet<int>> wordMapping = fs.WordIDMapping;
            //List<string> top5Sentences = fs.GetAnchorWords(10);//fs.GetTopSentences(5);
            //children node = fs.GetNodeById(9271331);
            List<string> topAnchors = fs.GetAnchorWords(10);
            //List<string> topNamedObjects = fs.GetNamedObjects(30);
            //List<string> top5Anchors = fs.GetAnchorWords(node, 5);
            //Assert.AreEqual(topNamedObjects.Count > 1, true);
        }


        [TestMethod]
        public void TestGetTopSentences()
        {
            FullStory fs = FullStoryFactory.GetFullStory(9231386);
            Dictionary<string, HashSet<int>> wordMapping = fs.WordIDMapping;
            List<SentenceObj> top5Sentences = fs.GetTopSentences(5);
            Assert.AreEqual(top5Sentences.Count > 1, true);
        }

        [TestMethod]
        public void TestGetTopStory()
        {
            Reader r = new Reader();
            object result =  r.GetFullStory(9231386);
            Assert.AreNotEqual(result, null);
        }

        [TestMethod]
        public void TestGetMapping()
        {
            FullStory fs = FullStoryFactory.GetFullStory(9148746);
            Dictionary<string, HashSet<int>> wordMapping = fs.WordIDMapping;
            Assert.AreEqual(wordMapping.Count>1,true);
        }

        [TestMethod]
        public void TestGetFrontPage()
        {
            Reader r = new Reader();
            var result = r.GetFrontPage(10);
            Console.WriteLine(result.ToString());
        }

        [TestMethod]
        public void TestGetTagCloudTree()
        {
            Reader reader = new Reader();
            Task<object> result = reader.GetFullStory(9283249);
            Assert.AreNotEqual(result,null);
        }

        [TestMethod]
        public void TestGetSentenceTree()
        {
            Reader reader = new Reader();
            Task<object> result = reader.GetCommentTreeForId("8870648:8870479");
        }

        [TestMethod]
        public void GetCount()
        {
            Reader reader = new Reader();
            int count = reader.GetStoryCommentCount(9112885);
        }

        [TestMethod]
        public void GetFrequency()
        {
            int freq = CommonWords.GetFrequency("curious");
            Console.WriteLine(freq);
        }

        [TestMethod]
        public List<string> NamedObjectMethod(string text)
        {
            string[] separators = { " ", ".", ",", ";", "-", "(", ")", "[", "]", "*", "#", "$", "%", "\"", "?", "!", ":", "\n", "\r" };
            List<string> namedEntities = text
                .Split(separators,StringSplitOptions.RemoveEmptyEntries)
                .ToList()
                .Where(x => CommonWords.GetFrequency(x) < 1)
                .Where(a => char.IsUpper(a[0]))
                .Where(b => b.Length > 1)
                .Where(
                    z =>
                        !(z.EndsWith("n't") || z.EndsWith("'m") || (z.EndsWith("'ll")) || (z.EndsWith("'d")) ||
                          z.EndsWith("'ve") || z.EndsWith("'re") || z.EndsWith("'s")))
                .ToList();
            return namedEntities;
        }
    }
}
