using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiSum
{
    public class StoryItem
    {
        public string by { get; set; }
        public int id { get; set; }
        public List<int> kids { get; set; } 
        public int score { get; set; }
        public long time { get; set; }
        public string title { get; set; }
        public string url { get; set; }
    }
}
