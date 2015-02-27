using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NLTKSharp;

namespace HiSum
{
    public class FullStory
    {
        public int id { get; set; }
        [JsonIgnore]
        public DateTime created_at { get; set; }
        public string author { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public string text { get; set; }
        [JsonIgnore]
        public int point { get; set; }
        [JsonIgnore]
        public int? parent_id { get; set; }
        public List<children> children { get; set; }

        public int TotalComments
        {
            get { 
                int count = GetStoryCommentCount();
                return count;
            }
        }

        public int GetChildCount(children childrenlist)
        {
            if (childrenlist.Children.Count == 0) return 1;
            int counter = 0;
            foreach (children child in childrenlist.Children)
            {
                counter += GetChildCount(child);
            }
            return counter;
        }

        public int GetStoryCommentCount()
        {
            int counter = 0;
            foreach (children child in this.children)
            {
                int childcount = GetChildCount(child);
                counter += childcount;
            }
            return counter;
        }

        public string GetCommentTree()
        {
            string commentTree = string.Empty;
            commentTree = JsonConvert.SerializeObject(this);
            return commentTree;
        }

        public Dictionary<string, int> GetTopNWordsDictionary(int N)
        {
            string[] stopWords = { "he", "his", "which", "want", "do", "would", "more", "like", "you", "your", "very", "me", "get", "has", "i", "over", "could", "have", "what", "a", "an", "and", "are", "as", "at", "be", "but", "by", "for", "if", "in", "into", "is", "it", "no", "not", "of", "on", "or", "such", "that", "the", "their", "then", "there", "these", "they", "this", "to", "was", "will", "with" };
            string[] ignoreWords = {"*"};
            string[] allWords;
            Dictionary<string, int> wordCount = new Dictionary<string, int>();
            StringBuilder sbFullText = new StringBuilder();
            foreach (children child in this.children)
            {
                sbFullText.Append(child.SubtreeText);
                sbFullText.Append(" ");
            }
            string tagLess = Util.StripTagsCharArray(sbFullText.ToString());
            string urlLess = Regex.Replace(tagLess, 
                @"((http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)", 
                string.Empty);
            wordCount = new Dictionary<string, int>();
            string[] separators = { " ", ".",",",";","-","(",")","[","]" };
            allWords = urlLess.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            
            Dictionary<string, string> stemParent = new Dictionary<string, string>();
            foreach (string word in allWords)
            {
                string stemmed = Stemmer.GetStem(word);
                if (stemParent.ContainsKey(stemmed))
                {
                    if (stemParent[stemmed].Length < word.Length)
                    {
                        stemParent[stemmed] = word;
                    }
                }
                else
                {
                    stemParent[stemmed] = word;
                }
                if (stopWords.Contains(stemmed.ToLower())) continue;
                if (!wordCount.ContainsKey(stemmed)&&!ignoreWords.Contains(stemmed))
                {
                    wordCount[stemmed] = 1;
                }
                else
                {
                    wordCount[stemmed] += 1;
                }
            }
            wordCount = wordCount.OrderByDescending(x => x.Value).Take(N).ToDictionary(kvp=>stemParent[kvp.Key],kvp=>kvp.Value);
            return wordCount;
        } 

        public List<string> GetTopNWords(int N)
        {
            string[] stopWords = { "he", "his", "which", "want", "do", "would", "more", "like", "you", "your", "very", "me", "get", "has", "i", "over", "could", "have", "what", "a", "an", "and", "are", "as", "at", "be", "but", "by", "for", "if", "in", "into", "is", "it", "no", "not", "of", "on", "or", "such", "that", "the", "their", "then", "there", "these", "they", "this", "to", "was", "will", "with" };
            string[] allWords;
            Dictionary<string, int> wordCount = new Dictionary<string, int>();
            List<string> topNWords = new List<string>();
            List<string> topNWordsForComment = new List<string>();
            StringBuilder sbFullText = new StringBuilder();
            foreach (children child in this.children)
            {
                sbFullText.Append(child.SubtreeText);
                sbFullText.Append(" ");
            }
            string tagLess = Util.StripTagsCharArray(sbFullText.ToString());
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
