using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiSum
{
    public static class Extensions
    {
        public static string AppSettings(this string key, string defaultValue=""){
            string retVal = defaultValue;
            if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings[key]))
            {
                retVal = ConfigurationManager.AppSettings[key];
            }
            return retVal;
        }
        public static string StripTagsCharArray(string source)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }
    }
}
