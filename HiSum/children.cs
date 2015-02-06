using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HiSum
{
    public class children
    {
        public int id { get; set; }
        public DateTime created_at { get; set; }
        public string author { get; set; }
        public string text { get; set; }
        public int? points { get; set; }
        public int parent_id { get; set; }
        [JsonProperty(PropertyName = "children")]
        public List<children> Children { get; set; }

        public string SubtreeText
        {
            get
            {
                return GetSubtree(this);
            }
        }

        private string GetSubtree(children node)
        {
            string subtreeText = node.text;
            foreach (children child in node.Children)
            {
                subtreeText += " " + GetSubtree(child);
            }
            return subtreeText;
        }

        public List<string> GetTopNWords(int N)
        {
            string[] stopWords = { "he", "his", "which", "want", "do", "would", "more", "like", "you", "your", "very", "me", "get", "has", "i", "over", "could", "have", "what", "a", "an", "and", "are", "as", "at", "be", "but", "by", "for", "if", "in", "into", "is", "it", "no", "not", "of", "on", "or", "such", "that", "the", "their", "then", "there", "these", "they", "this", "to", "was", "will", "with" };
            string[] allWords;
            Dictionary<string, int> wordCount = new Dictionary<string, int>();
            List<string> topNWords = new List<string>();
            List<string> topNWordsForComment = new List<string>();
            string tagLess = Util.StripTagsCharArray(this.SubtreeText);
            wordCount = new Dictionary<string, int>();
            string[] separators = { " ", "." };
            allWords = tagLess.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            foreach (string word in allWords)
            {
                if (stopWords.Contains(word.ToLower())) continue;
                if (!wordCount.ContainsKey(word))
                {
                    wordCount[word] = 1;
                }
                else
                {
                    wordCount[word] += 1;
                }
            }
            topNWordsForComment = wordCount.OrderByDescending(x => x.Value).Select(x => x.Key + "[" + x.Value + "]").Take(N).ToList();
            topNWords.AddRange(topNWordsForComment);
            return topNWords;
        } 
    }
}
