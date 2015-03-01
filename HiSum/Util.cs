using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HiSum
{
    public static class Util
    {
        public static string StripTagsCharArray(string source)
        {
            if (source == null)
                return string.Empty;
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

        public static string FetchJson(string url)
        {
            string returnVal = string.Empty;
            WebRequest request = WebRequest.Create(url);
            using (WebResponse response = request.GetResponse())
            {
                Stream dataStream = response.GetResponseStream();
                using (StreamReader reader = new StreamReader(dataStream))
                {
                    returnVal = reader.ReadToEnd();
                }
            }
            return returnVal;
        }
    }
}
