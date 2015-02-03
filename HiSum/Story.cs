using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiSum
{
    public class Story
    {
        public string By { get; set; }
        public string Title { get; set; }
        public int Score { get; set; }
        public int Id { get; set; }
        public long Time { get; set; }
        public string Url { get; set; }
        public List<Comment> Comments { get; set; }

        public List<string> GetTopNWords(int N)
        {
            List<string> topNWords = new List<string>();
            foreach (Comment comment in Comments)
            {
                List<string> commentTopNWords = comment.GetTopNWords(N);
                topNWords.AddRange(commentTopNWords);
            }
            return topNWords;
        }
    }
}

