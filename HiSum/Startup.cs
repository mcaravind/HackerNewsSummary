using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiSum
{
    public class Startup
    {
        //public async Task<object> Invoke(object input)
        //{
        //    Reader reader = new Reader();
        //    return reader.GetTop100();
        //}

        //public async Task<object> GetTop100(object input)
        //{
        //    Reader reader = new Reader();
        //    return reader.GetTop100();
        //}

        public async Task<object> GetTop100(object input)
        {
            return "testing123";
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
