using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiSum
{
    public class FullStory
    {
        public int id { get; set; }
        public DateTime created_at { get; set; }
        public string author { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public string text { get; set; }
        public int point { get; set; }
        public int? parent_id { get; set; }
        public List<children> children { get; set; }

        public List<string> GetTopNWords(int N)
        {
            List<string> topNWords = new List<string>();
            foreach (children child in children)
            {
                List<string> commentTopNWords = child.GetTopNWords(N);
                topNWords.AddRange(commentTopNWords);
            }
            return topNWords;
        }
    }
}
