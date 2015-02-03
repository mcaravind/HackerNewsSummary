using System;
using System.Collections.Generic;
using System.Linq;
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

            

            TreeGridView view = new TreeGridView();


            view.Columns.Add(new GridColumn() { HeaderText = "Test", DataCell = new TextBoxCell(0), AutoSize = true, Resizable = true, Editable = false });
            view.Columns.Add(new GridColumn() { HeaderText = "Id", DataCell = new TextBoxCell(1), AutoSize = true, Resizable = true, Editable = false });

            TreeGridItemCollection data = new TreeGridItemCollection();

            TreeGridItem child = new TreeGridItem() { Values = new object[] { "Testing1", "Testing2" } };
            TreeGridItem child2 = new TreeGridItem() { Values = new object[] { "Testing3", "Testing4" } };
            TreeGridItem child3 = new TreeGridItem() { Values = new object[] { "Testing3", "Testing4" } };
            child.Children.Add(new TreeGridItem() { Values = new object[] { "1", "2" } });
            child2.Children.Add(new TreeGridItem() { Values = new object[] { "3", "4" } });
            child3.Children.Add(new TreeGridItem() { Values = new object[] { "5", "6" } });
            data.Add(child);
            data.Add(child2);
            child2.Children.Add(child3);
            view.DataStore = data;


            var textbox = new TextBox() {Width = 1000};
            var button = new Button(){Text = "Go", Width = 50};
            var tbResult = new TextArea() {Width = 1000};
            
            button.Click += (sender, e) =>
            {
                string url = textbox.Text;
                int storyID = Convert.ToInt32(url.Split('=')[1]);
                Reader reader = new Reader();
                Story story = reader.GetStory(storyID);
                List<string> top10words = story.GetTopNWords(5);
                foreach (string s in top10words)
                {
                    tbResult.Text += s + Environment.NewLine;
                }
            };
            Content = new TableLayout
            {
                Spacing = new Size(5, 5), // space between each cell
                Padding = new Padding(10, 10, 10, 10), // space around the table's sides
                
                Rows =
				{
					new TableRow(
                        new Label{Text = "Input URL from Hacker News: ",Width=200},
						textbox,
                        button
					),
                    new TableRow(
                        null,
                        tbResult,
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
    }
}
