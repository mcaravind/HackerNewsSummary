using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiSum
{
    public class Startup
    {
        public async Task<object> Invoke(object input)
        {
            int v = (int)input;
            return Helper.AddSeven(v);
        }
    }
    static class Helper
    {
        public static int AddSeven(int v)
        {
            return v + 7;
        }
    }
}
