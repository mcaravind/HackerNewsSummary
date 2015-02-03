using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiSum
{
    public class Comment
    {
        public string By { get; set; }
        public int Id { get; set; }
        public int Parent { get; set; }
        public string Text { get; set; }
        public string SubtreeText { get; set; }
        public long Time { get; set; }
        public List<Comment> Comments { get; set; }

        public List<string> GetTopNWords(int N)
        {
            string[] stopWords = new System.String[] { "he", "his", "which", "want", "do", "would", "more", "like", "you", "your", "very", "me", "get", "has", "i", "over", "could", "have", "what", "a", "an", "and", "are", "as", "at", "be", "but", "by", "for", "if", "in", "into", "is", "it", "no", "not", "of", "on", "or", "such", "that", "the", "their", "then", "there", "these", "they", "this", "to", "was", "will", "with" };
            string[] allWords;
            Dictionary<string, int> wordCount = new Dictionary<string, int>();
            List<string> topNWords = new List<string>();
            List<string> topNWordsForComment = new List<string>();
            string tagLess = Util.StripTagsCharArray(this.SubtreeText);
            wordCount = new Dictionary<string, int>();
            string[] separators = new string[] { " ", "." };
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
            topNWordsForComment = wordCount.OrderByDescending(x => x.Value).Where(x=>x.Value>1).Select(x => x.Key).Take(N).ToList();
            topNWordsForComment.Add(string.Empty);
            topNWords.AddRange(topNWordsForComment);
            return topNWords;
        } 
    }
}
