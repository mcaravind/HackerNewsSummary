using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace HiSum
{
    public class Reader
    {
        string _apiURL { get; set; }
        string _top100 { get; set; }
        string _story { get; set; }
        string _comment { get; set; }
        string _algoliaURL { get; set; }
        public Reader()
        {
            _apiURL = "apiURL".AppSettings(defaultValue: "https://hacker-news.firebaseio.com/v0/");
            _top100 = "top100".AppSettings(defaultValue: "topstories.json");
            _story = "item".AppSettings(defaultValue: "item");
            _comment = "comment".AppSettings(defaultValue: "item");
            _algoliaURL = "algoliaURL".AppSettings(defaultValue: "http://hn.algolia.com/api/v1/items/");
        }

        public List<int> GetTop100(int number = 100)
        {
            List<string> top100URLs = new List<string>();
            string top100URL = _apiURL + _top100;
            string response = FetchJson(top100URL);
            top100URLs = response.Replace("[", string.Empty).Replace("]", string.Empty).Split(',').ToList();
            List<int> topN = top100URLs.ConvertAll(x => Convert.ToInt32(x)).Take(number).ToList();
            return topN;
        }

        public async Task<object> GetTop100Stories(object input)
        {
            Dictionary<int,string> top100 = new Dictionary<int, string>();
            List<int> top100Ids = GetTop100();
            List<StoryObj> storyObjList = new List<StoryObj>();
            foreach (int id in top100Ids)
            {
                FullStory fs = GetStoryFull(id);
                StoryObj so = new StoryObj(){StoryId = id,StoryTitle = fs.title,Author = fs.author,StoryText = fs.text};
                storyObjList.Add(so);
            }
            return storyObjList;
        }

        public async Task<object> GetFrontPage(object input)
        {
            Dictionary<int, string> top100 = new Dictionary<int, string>();
            List<int> top100Ids = GetTop100(30);
            List<StoryObj> storyObjList = new List<StoryObj>();
            foreach (int id in top100Ids)
            {
                FullStory fs = GetStoryFull(id);
                int commentCount = fs.TotalComments;
                //top100[id] = fs.title;
                StoryObj so = new StoryObj() { StoryId = id, StoryTitle = fs.title, Author = fs.author, StoryText = fs.text,Url=fs.url??string.Empty, StoryComments = commentCount };
                storyObjList.Add(so);
            }
            return storyObjList;
        }

        public async Task<object> GetCommentTree(int storyid)
        {
            FullStory fs = GetStoryFull(storyid);
            string json = GetCommentTree(fs);
            return json;
        }

        public async Task<object> GetTagCloudTree(int storyid)
        {
            FullStory fs = GetStoryFull(storyid);
            string json = GetTagCloudTree(fs);
            return json;
        }

        string GetTagCloudTree(FullStory fs)
        {
            Dictionary<string,int> topNWordsRoot = fs.GetTopNWordsDictionary(10);
            TagCloudNode tgnRoot = new TagCloudNode();
            tgnRoot.id = fs.id;
            tgnRoot.text = GetTagCloudFromDictionary(topNWordsRoot);
            tgnRoot.children = new List<TagCloudNode>();
            foreach (children child in fs.children)
            {
                TagCloudNode tgnChild = GetTagCloudTree(child);
                tgnRoot.children.Add(tgnChild);
            }
            return JsonConvert.SerializeObject(tgnRoot) ;
        }

        TagCloudNode GetTagCloudTree(children children)
        {
            Dictionary<string, int> topNWordsRoot = children.GetTopNWordsDictionary(10);
            TagCloudNode tgnRoot = new TagCloudNode();
            tgnRoot.id = children.id;
            tgnRoot.text = GetTagCloudFromDictionary(topNWordsRoot);
            tgnRoot.children = new List<TagCloudNode>();
            foreach (children child in children.Children)
            {
                TagCloudNode tgnChild = GetTagCloudTree(child);
                tgnRoot.children.Add(tgnChild);
            }
            return tgnRoot;
        }

        string GetTagCloudFromDictionary(Dictionary<string,int> dict)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in dict)
            {
                double fontSize = ((Math.Min(item.Value,10)+5)*100)/10;
                //sb.Append("<font size='" + fontSize + "'>");
                //sb.Append(item.Key);
                //sb.Append("</font>");
                //sb.Append("&nbsp;");
                sb.Append("<span style='font-size:");
                sb.Append(fontSize);
                sb.Append("%'>");
                sb.Append(item.Key);
                sb.Append("</span>&nbsp;");
            }
            return sb.ToString();
        }

        public async Task<object> GetStoryTopNWords(int storyid)
        {
            FullStory fs = GetStoryFull(storyid);
            Dictionary<string, int> topNWords = fs.GetTopNWordsDictionary(10);
            List<WordObj> wordObjs = topNWords.ToList().Select(x => new WordObj() {Word = x.Key, Count = x.Value}).ToList();
            return wordObjs;
        }

        public FullStory GetStoryFull(int storyID)
        {
            string storyURL = _algoliaURL + storyID;
            string response = FetchJson(storyURL);
            FullStory fullStory = JsonConvert.DeserializeObject<FullStory>(response);
            return fullStory;
        }

        string GetCommentTree(FullStory fs)
        {
            return fs.GetCommentTree();
        }

        public Story GetStory(int storyID)
        {
            string storyURL = _apiURL + _story + "/" + storyID + ".json";
            string response = FetchJson(storyURL);
            StoryItem storyItem = JsonConvert.DeserializeObject<StoryItem>(response);
            Story story = new Story()
            {
                By = storyItem.by,
                Id = storyItem.id,
                Score = storyItem.score,
                Time = storyItem.time,
                Title = storyItem.title,
                Url = storyItem.url
            };
            story.Comments = new List<Comment>();
            foreach (int kid in storyItem.kids)
            {
                Comment comment = GetComment(kid);
                story.Comments.Add(comment);
            }
            return story;
        }

        private Comment GetComment(int id)
        {
            Comment comment = new Comment();
            string commentURL = _apiURL + _comment + "/" + id + ".json";
            string responseComment = FetchJson(commentURL);
            CommentItem commentItem = JsonConvert.DeserializeObject<CommentItem>(responseComment);
            comment.By = commentItem.by;
            comment.Text = WebUtility.HtmlDecode(commentItem.text);
            comment.SubtreeText = WebUtility.HtmlDecode(commentItem.text);
            comment.Parent = commentItem.parent;
            comment.Time = commentItem.time;
            comment.Comments = new List<Comment>();
            if (commentItem.kids != null)
            {
                foreach (int kid in commentItem.kids)
                {
                    Comment child = GetComment(kid);
                    comment.Comments.Add(child);
                    comment.SubtreeText += " " + WebUtility.HtmlDecode(child.Text);
                }
            }
            return comment;
        }

        public string FetchJson(string url)
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

        class StoryObj
        {
            public int StoryId { get; set; }
            public string StoryTitle { get; set; }
            public string Author { get; set; }
            public string StoryText { get; set; }
            public string Url { get; set; }
            public int StoryComments { get; set; }
        }

        class WordObj
        {
            public string Word { get; set; }
            public int Count { get; set; }
        }
    }
}
