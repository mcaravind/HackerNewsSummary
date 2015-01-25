using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiSum
{
    public class TreeNode
    {
        public string Text { get; set; }
        public List<TreeNode> ChildNodes { get; set; }
        public TreeNode Parent { get; set; }
        public int ID { get; set; }
        public string SubTreeText { get; set; }

        public List<string> TopNWords(int numWords)
        {
            List<string> topNWords = new List<string>();
            return topNWords;
        }  
    }
}
