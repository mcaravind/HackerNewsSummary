using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using Eto.Forms;
using Eto.Drawing;

namespace HiSum.Forms
{
    public class HiSumDisplay : Eto.Forms.Form
    {
        public HiSumDisplay()
        {
            // sets the client (inner) size of the window for your content
            ClientSize = new Eto.Drawing.Size(600, 400);

            Title = "HiSum";
            TreeGridView view = new TreeGridView(){Height = 500};

            view.Columns.Add(new GridColumn() { HeaderText = "Summary", DataCell = new TextBoxCell(0), AutoSize = true, Resizable = true, Editable = false });
            var textbox = new TextBox() {Width = 1000};
            var button = new Button(){Text = "Go", Width = 15};
            var label = new Label() {Width = 100};
            var tbResult = new TextArea() {Width = 1000};
            
            button.Click += (sender, e) =>
            {
                Reader reader = new Reader();
                List<int> top100 = reader.GetTop100();
                List<FullStory> fullStories = new List<FullStory>();
                foreach (int storyID in top100.Take(30))
                {
                    FullStory fullStory = reader.GetStoryFull(storyID);
                    fullStories.Add(fullStory);
                }
                TreeGridItemCollection data = GetTree(fullStories);
                view.DataStore = data;
            };
            Content = new TableLayout
            {
                Spacing = new Size(5, 5), // space between each cell
                Padding = new Padding(10, 10, 10, 10), // space around the table's sides
                Rows = {
					new TableRow(
                        new Label{Text = "Input URL from Hacker News: ",Width=200},
						textbox,
                        button,
                        label
					),
                    new TableRow(
                        null,
                        tbResult,
                        null,
                        null
                        ),
					new TableRow(
                        new Label(),
						view
					),
                    
					// by default, the last row & column will get scaled. This adds a row at the end to take the extra space of the form.
					// otherwise, the above row will get scaled and stretch the TextBox/ComboBox/CheckBox to fill the remaining height.
					new TableRow { ScaleHeight = true }
				}
            };
        }

        private TreeGridItemCollection GetTree(Story story)
        {
            TreeGridItemCollection data = new TreeGridItemCollection();
            foreach (Comment comment in story.Comments)
            {
                TreeGridItem tgi = GetCommentTree(comment);
                if(tgi.Tag != "empty")
                {
                    data.Add(tgi);
                }
            }
            return data;
        }

        private TreeGridItemCollection GetTree(List<FullStory> stories)
        {
            TreeGridItemCollection data = new TreeGridItemCollection();
            foreach (FullStory story in stories)
            {
                TreeGridItem tgiParent = new TreeGridItem() { Values = new object[] { WebUtility.HtmlDecode(story.title) } };
                string all5 = string.Empty;
                foreach (string s in story.GetTopNWords(5))
                {
                    all5 += s + " ";
                }
                tgiParent.Children.Add(new TreeGridItem() { Values = new object[] { WebUtility.HtmlDecode(all5) } });
                data.Add(tgiParent);
                foreach (children child in story.children)
                {
                    TreeGridItem tgi = GetCommentTree(child);
                    if (tgi.Tag != "empty")
                    {
                        tgiParent.Children.Add(tgi);
                    }
                }
            }
            return data;
        }

        private TreeGridItem GetCommentTree(children child)
        {
            List<string> top5 = child.GetTopNWords(5);

            TreeGridItem tgi = new TreeGridItem();
            if (top5.Count > 0)
            {
                string all5 = string.Empty;
                foreach (string s in top5)
                {
                    all5 += s + " ";
                }
                tgi.Values = new object[] { all5 };
                RichTextArea rta = new RichTextArea();
                rta.Text = WebUtility.HtmlDecode(child.text);

                tgi.Children.Add(new TreeGridItem() { Values = new object[] {WebUtility.HtmlDecode(child.text) } });
                foreach (children commentchild in child.Children)
                {
                    TreeGridItem tgic = GetCommentTree(commentchild);
                    tgi.Children.Add(tgic);
                }
            }
            else
            {
                tgi.Tag = "empty";
            }
            return tgi;
        }

        private TreeGridItem GetCommentTree(Comment comment)
        {
            List<string> top5 = comment.GetTopNWords(5);
            
            TreeGridItem tgi = new TreeGridItem();
            if (top5.Count > 0)
            {
                string all5 = string.Empty;
                foreach (string s in top5)
                {
                    all5 += s + " ";
                }
                tgi.Values = new object[] { all5 };
                RichTextArea rta = new RichTextArea();
                rta.Text = comment.Text;
                
                tgi.Children.Add(new TreeGridItem() { Values = new object[] { comment.Text } });
                foreach (Comment commentChild in comment.Comments)
                {
                    TreeGridItem tgic = GetCommentTree(commentChild);
                    tgi.Children.Add(tgic);
                }
            }
            else
            {
                tgi.Tag = "empty";
            }
            return tgi;
        }
    }
}
