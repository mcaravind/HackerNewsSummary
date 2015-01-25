using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiSum
{
    public class CommentItem
    {
        public string by { get; set; }
        public int id { get; set; }
        public List<int> kids { get; set; }
        public int parent { get; set; }
        public string text { get; set; }
        public long time { get; set; }
        public string type { get; set; }
    }
}
