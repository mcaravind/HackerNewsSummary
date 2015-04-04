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
        public async Task<object> ArchiveStory(int id)
        {
            try
            {
                FullStory fs = FullStoryFactory.GetFullStory(id);
                fs.ArchivedOn = DateTime.Now;
                Directory.CreateDirectory("archive");
                string fileName = Path.Combine("archive", id + ".json");
                File.WriteAllText(fileName, JsonConvert.SerializeObject(fs));
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return string.Empty;
        }

        public async Task<object> DeleteStory(int id)
        {
            try
            {
                string fileName = Path.Combine("archive", id + ".json");
                File.Delete(fileName);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return string.Empty;
        }

        public async Task<object> CheckForUpdates(dynamic input)
        {
            List<int> top30Ids = GetTop100(30);
            List<int> currentList = new List<int>();
            object[] idList = (object[]) input.idList;
            foreach (object id in idList)
            {
                currentList.Add(Convert.ToInt32((string)id));
            }
            List<int> newIds = top30Ids.Except(currentList).ToList();
            List<StoryObj> newStories = new List<StoryObj>();
            foreach (int id in newIds)
            {
                try
                {
                    FullStory fs = FullStoryFactory.GetFullStory(id);
                    int commentCount = fs.TotalComments;
                    string commentUrl = "https://news.ycombinator.com/item?id=" + id;
                    StoryObj so = new StoryObj() { StoryId = id, StoryTitle = fs.title, Author = fs.author, StoryText = fs.text, Url = fs.url ?? commentUrl, CommentUrl = commentUrl, StoryComments = commentCount };
                    newStories.Add(so);
                }
                catch (Exception ex)
                {
                    //Sometimes algolia api throws an error, just move
                    //on to the next item
                    Console.WriteLine(ex.ToString());
                }
            }
            return newStories;
        }

        public async Task<object> GetArchivedStories(int id)
        {
            try
            {
                var directory = new DirectoryInfo("archive");
                var files = directory.GetFiles().OrderByDescending(x => x.LastWriteTime).Where(y => y.Extension == ".json").ToList();
                List<StoryObj> storyObjList = new List<StoryObj>();
                foreach (var file in files)
                {
                    string fileText = File.ReadAllText(file.FullName);
                    FullStory fs = JsonConvert.DeserializeObject<FullStory>(fileText);
                    string commentUrl = "https://news.ycombinator.com/item?id=" + fs.id;
                    StoryObj so = new StoryObj() { StoryId = fs.id, StoryTitle = fs.title, Author = fs.author, StoryText = fs.text, Url = fs.url ?? commentUrl, CommentUrl = commentUrl, StoryComments = fs.TotalComments,ArchivedOn = fs.ArchivedOn};
                    storyObjList.Add(so);
                }
                return storyObjList.OrderByDescending(x=>x.ArchivedOn).ToList();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public async Task<Object> FollowUser(string userName)
        {
            try
            {
                Directory.CreateDirectory("data");
                string fileName = Path.Combine("data", "following.txt");
                File.AppendAllText(fileName, userName+Environment.NewLine);
            }
            catch (Exception ex)
            {
                //Sometimes algolia api throws an error, just move
                //on to the next item
                Console.WriteLine(ex.ToString());
                return ex.ToString();
            }
            return string.Empty;
        }

        public async Task<Object> UnfollowUser(string userName)
        {
            try
            {
                Directory.CreateDirectory("data");
                string fileName = Path.Combine("data", "following.txt");
                var lines = File.ReadAllLines(fileName).Where(arg => arg!=userName);
                File.WriteAllLines(fileName,lines);
            }
            catch (Exception ex)
            {
                //Sometimes algolia api throws an error, just move
                //on to the next item
                Console.WriteLine(ex.ToString());
                return ex.ToString();
            }
            return string.Empty;
        }

        public async Task<Object> GetStory(int id)
        {
            List<StoryObj> storyObjList = new List<StoryObj>();
            try
            {
                FullStory fs = FullStoryFactory.GetFullStory(id);
                int commentCount = fs.TotalComments;
                string commentUrl = "https://news.ycombinator.com/item?id=" + id;
                StoryObj so = new StoryObj() { StoryId = id, StoryTitle = fs.title, Author = fs.author, StoryText = fs.text, Url = fs.url ?? commentUrl,CommentUrl = commentUrl,StoryComments = commentCount };
                storyObjList.Add(so);
            }
            catch (Exception ex)
            {
                //Sometimes algolia api throws an error, just move
                //on to the next item
                Console.WriteLine(ex.ToString());
            }
            return storyObjList;
        }

        public async Task<object> GetStoryTopNWords(int storyid)
        {
            FullStory fs = FullStoryFactory.GetFullStory(storyid);
            Dictionary<string, int> topNWords = fs.GetTopNWordsDictionary(10);
            List<WordObj> wordObjs = topNWords.ToList().Select(x => new WordObj() { Word = x.Key, Count = x.Value }).ToList();
            return wordObjs;
        }

        public async Task<object> GetTop100Stories(object input)
        {
            List<int> top100Ids = GetTop100();
            List<StoryObj> storyObjList = new List<StoryObj>();
            foreach (int id in top100Ids)
            {
                FullStory fs = FullStoryFactory.GetFullStory(id);
                string commentUrl = "https://news.ycombinator.com/item?id=" + id;
                StoryObj so = new StoryObj() { StoryId = id, StoryTitle = fs.title, Author = fs.author, StoryText = fs.text, Url = fs.url ?? commentUrl, CommentUrl = commentUrl };
                storyObjList.Add(so);
            }
            return storyObjList;
        }

        public async Task<object> GetAllFollowing(int id)
        {
            Directory.CreateDirectory("data");
            string fileName = Path.Combine("data", "following.txt");
            string[] following = File.ReadAllLines(fileName);
            return following;
        }


        public async Task<object> GetFrontPage(object input)
        {
            List<int> top100Ids = GetTop100(30);
            List<StoryObj> storyObjList = new List<StoryObj>();
            Parallel.ForEach(top100Ids, currentId =>
            {
                StoryObj so = GetFullStoryObj(currentId);
                if (so != null)
                {
                    storyObjList.Add(so);
                }
            });
            return storyObjList;
        }

        private StoryObj GetFullStoryObj(int id)
        {
            StoryObj so = new StoryObj();
            try
            {
                FullStory fs = FullStoryFactory.GetFullStory(id);
                int commentCount = fs.TotalComments;
                string commentUrl = "https://news.ycombinator.com/item?id=" + id;
                var usercommentObjList = new List<UserCommentObj>();
                var dictComments = fs.GetUserComments();
                foreach (var kvp in dictComments)
                {
                    UserCommentObj userCommentObj = new UserCommentObj(){User = kvp.Key,Comments = kvp.Value,StoryId = id};
                    usercommentObjList.Add(userCommentObj);
                }
                so = new StoryObj() { StoryId = id, StoryTitle = fs.title, Author = fs.author, StoryText = fs.text, Url = fs.url ?? commentUrl, CommentUrl = commentUrl, StoryComments = commentCount,AllUserComments = usercommentObjList};
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return so;
        }

        public async Task<object> GetCommentTreeForId(string idTuple)
        {
            string sStoryId = idTuple.Split(':')[1];
            string sNodeId = idTuple.Split(':')[0];
            int storyid = Convert.ToInt32(sStoryId);
            int nodeid = Convert.ToInt32(sNodeId);
            FullStory fs = FullStoryFactory.GetFullStory(storyid);
            children child = fs.GetNodeById(nodeid);
            TagCloudNode tgn = fs.GetCommentTreeString(child);
            return JsonConvert.SerializeObject(tgn);
        }

        public async Task<object> GetFullStory(int storyid)
        {
            FullStory fs = FullStoryFactory.GetFullStory(storyid);
            string json = fs.GetCommentTreeString();
            Dictionary<int, string> commentDictionary = GetCommentDictionary(fs);
            List<CommentObj> comments = new List<CommentObj>();
            List<SentenceObj> topSentenceObjs = fs.GetTopSentences(5);
            foreach (var item in commentDictionary)
            {
                comments.Add(new CommentObj() { Id = item.Key, Text = item.Value });
            }
            List<UserCommentObj> userComments = new List<UserCommentObj>();
            var userCommentsForStory = fs.GetUserComments();
            foreach (var kvp in userCommentsForStory)
            {
                userComments.Add(new UserCommentObj(){User = kvp.Key,Comments = kvp.Value});
            }
            List<KeywordCommentObj> keywordComments = new List<KeywordCommentObj>();
            var namedObjectsForStory = fs.GetNamedObjects(20);
            foreach (var kvp in namedObjectsForStory)
            {
                keywordComments.Add(new KeywordCommentObj(){Keyword = kvp.Key,Comments = kvp.Value});
            }
            Directory.CreateDirectory("data");
            string fileName = Path.Combine("data", "following.txt");
            string[] following = File.ReadAllLines(fileName);
            string csv = string.Join(",", following);
            FullStoryObj fullStoryObj = new FullStoryObj() { Json = json, TotalComments = commentDictionary.Count,Comments = comments,Sentences = topSentenceObjs,UserComments = userComments,KeywordComments = keywordComments,AllFollowing = csv};
            return fullStoryObj;
        }

        public List<int> GetTop100(int number = 100)
        {
            List<string> top100URLs = new List<string>();
            string top100URL = Globals.ApiUrl + Globals.Top100;
            string response = Util.FetchJson(top100URL);
            top100URLs = response.Replace("[", string.Empty).Replace("]", string.Empty).Split(',').ToList();
            List<int> topN = top100URLs.ConvertAll(x => Convert.ToInt32(x)).Take(number).ToList();
            return topN;
        }

        Dictionary<int, string> GetCommentDictionary(FullStory fs)
        {
            Dictionary<int,string> commentDictionary = new Dictionary<int, string>();
            foreach (children child in fs.children)
            {
                commentDictionary[child.id] = child.text;
                GetCommentDictionary(child, ref commentDictionary);
            }
            return commentDictionary;
        }

        void GetCommentDictionary(children childlist, ref Dictionary<int, string> dict)
        {
            foreach (children child in childlist.Children)
            {
                dict[child.id] = child.text;
                GetCommentDictionary(child, ref dict);
            }
        }
        
        public int GetStoryCommentCount(int storyID)
        {
            FullStory fs = FullStoryFactory.GetFullStory(storyID);
            return fs.TotalComments;
        }

        class StoryObj
        {
            public int StoryId { get; set; }
            public string StoryTitle { get; set; }
            public string Author { get; set; }
            public string StoryText { get; set; }
            public string Url { get; set; }
            public string CommentUrl { get; set; }
            public int StoryComments { get; set; }
            public DateTime ArchivedOn { get; set; }
            public List<UserCommentObj> AllUserComments { get; set; } 
        }

        class WordObj
        {
            public string Word { get; set; }
            public int Count { get; set; }
        }

        class FullStoryObj
        {
            public string Json { get; set; }
            public int TotalComments { get; set; }
            public List<CommentObj> Comments { get; set; }
            public List<SentenceObj> Sentences { get; set; }
            public List<UserCommentObj> UserComments { get; set; }
            public List<KeywordCommentObj> KeywordComments { get; set; }
            public string AllFollowing { get; set; }
        }

        class UserCommentObj
        {
            public string User { get; set; }
            public List<CommentObj> Comments { get; set; }
            public int StoryId { get; set; }
        }

        class KeywordCommentObj
        {
            public string Keyword { get; set; }
            public List<CommentObj> Comments { get; set; }
        }
    }
}
