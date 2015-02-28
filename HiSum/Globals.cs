using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiSum
{
    public static class Globals
    {
        public static string ApiUrl
        {
            get
            {
                return "apiURL".AppSettings(defaultValue: "https://hacker-news.firebaseio.com/v0/");
            } 
        }

        public static string Top100 
        {
            get
            {
                return "top100".AppSettings(defaultValue: "topstories.json");
            }
        }

        public static string Story
        {
            get
            {
                return "item".AppSettings(defaultValue: "item");
            }
        }

        public static string Comment
        {
            get
            {
                return "comment".AppSettings(defaultValue: "item");
            }
        }

        public static string AlgoliaUrl
        {
            get
            {
                return "algoliaURL".AppSettings(defaultValue: "http://hn.algolia.com/api/v1/items/");
            }
        }
    }
}
