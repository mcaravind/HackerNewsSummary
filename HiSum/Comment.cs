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
    }
}
