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
    }
}
