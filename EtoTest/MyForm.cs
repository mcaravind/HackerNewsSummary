using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto.Forms;

namespace EtoTest
{
    class MyForm:Eto.Forms.Form
    {
        public MyForm()
        {
            // sets the client (inner) size of the window for your content
            this.ClientSize = new Eto.Drawing.Size(600, 400);

            this.Title = "Hello, Eto.Forms";

            Content = new Label { Text = "Some content", VerticalAlign = VerticalAlign.Middle, HorizontalAlign = HorizontalAlign.Center };

            TreeGridView view = new TreeGridView();
            

            view.Columns.Add(new GridColumn() { HeaderText = "Test", DataCell = new TextBoxCell(0), AutoSize = true, Resizable = true, Editable = false });
            view.Columns.Add(new GridColumn() { HeaderText = "Id", DataCell = new TextBoxCell(0), AutoSize = true, Resizable = true, Editable = false });

            TreeGridItemCollection data = new TreeGridItemCollection();

            TreeGridItem child = new TreeGridItem() { Values = new object[] { "Testing" } };
            child.Children.Add(new TreeGridItem() { Values = new object[] { "1", "2", "3", "4" } });
            data.Add(child);

            view.DataStore = data;

            Content = view;
        }
    }
}
