using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HiSum.Forms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGetURL_Click(object sender, EventArgs e)
        {
            Reader reader = new Reader();
            tbResponse.Text = reader.FetchJson(tbURL.Text);
        }

        private void btnTop100_Click(object sender, EventArgs e)
        {
            Reader reader = new Reader();
            tbResponse.Text = string.Empty;
            foreach (var url in reader.GetTop100())
            {
                tbResponse.Text += url + Environment.NewLine;
            }
        }

        private void btnGetItem_Click(object sender, EventArgs e)
        {
            Reader reader = new Reader();
            Story story = reader.GetStory(Convert.ToInt32(tbURL.Text));
            List<string> topNWords = story.GetTopNWords(5);
            foreach (string topWord in topNWords)
            {
                tbResponse.Text += topWord + Environment.NewLine;
            }

        }
    }
}
