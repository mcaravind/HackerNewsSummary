using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto.Forms;

namespace HiSum.Forms
{
    public class HiSumDisplay : Eto.Forms.Form
    {
        public HiSumDisplay()
        {
            // sets the client (inner) size of the window for your content
            this.ClientSize = new Eto.Drawing.Size(600, 400);

            this.Title = "Hello, Eto.Forms";

            Content = new Label { Text = "Some content", VerticalAlign = VerticalAlign.Middle, HorizontalAlign = HorizontalAlign.Center };

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

            Content = view;
        }
    }
}
