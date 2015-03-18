using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HiSum
{
    public static class FullStoryFactory
    {
        public static FullStory GetFullStory(int storyID)
        {
            FullStory fs = new FullStory();
            try
            {
                string storyURL = Globals.AlgoliaUrl + storyID;
                string response = Util.FetchJson(storyURL);
                string strToReplace = @"""points"":null";
                string strReplaceWith = @"""points"":0";
                response = response.Replace(strToReplace, strReplaceWith);
                fs = JsonConvert.DeserializeObject<FullStory>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return fs;
        }
    }
}
