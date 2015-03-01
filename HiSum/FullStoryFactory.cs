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
            string storyURL = Globals.AlgoliaUrl + storyID;
            string response = Util.FetchJson(storyURL);
            FullStory fullStory = JsonConvert.DeserializeObject<FullStory>(response);
            return fullStory;
        }
    }
}
