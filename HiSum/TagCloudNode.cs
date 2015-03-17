using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiSum
{
    public class TagCloudNode
    {
        public int id { get; set; }
        public string text { get; set; }
        public string title { get; set; }
        public int key { get; set; }
        public List<TagCloudNode> children { get; set; } 
    }
}
