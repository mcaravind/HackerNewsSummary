using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
        public Reader()
        {
            _apiURL = "apiURL".AppSettings(defaultValue: "https://hacker-news.firebaseio.com/v0/");
            _top100 = "top100".AppSettings(defaultValue: "topstories.json");
            _story = "item".AppSettings(defaultValue: "item");
            _comment = "comment".AppSettings(defaultValue: "item");
        }

        public List<string> GetTop100()
        {
            List<string> top100URLs = new List<string>();
            string top100URL = _apiURL + _top100;
            string response = FetchJson(top100URL);
            //TODO:implement getting top 100 from this json
            top100URLs = response.Replace("[", string.Empty).Replace("]", string.Empty).Split(',').ToList();
            return top100URLs;
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
            comment.Text = commentItem.text;
            comment.Parent = commentItem.parent;
            comment.Time = commentItem.time;
            comment.Comments = new List<Comment>();
            if (commentItem.kids != null)
            {
                foreach (int kid in commentItem.kids)
                {
                    Comment child = GetComment(kid);
                    comment.Comments.Add(child);
                    comment.SubtreeText += comment.Text + " " + child.Text;
                }
            }
            return comment;
        }

        //private TreeNode GetChildren(int id)
        //{
        //    TreeNode returnNode = new TreeNode();
        //    string commentURL = _apiURL + _comment + "/" + id + ".json";
        //    string responseComment = FetchJson(commentURL);
        //    CommentItem comment = JsonConvert.DeserializeObject<CommentItem>(responseComment);
        //}

        //public Item GetItem(int itemID)
        //{
        //    string storyURL = _apiURL + _story + "/" + itemID + ".json";
        //    string response = FetchJson(storyURL);
        //    Item item = JsonConvert.DeserializeObject<Item>(response);
        //    return item;
        //}

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
    }
}
