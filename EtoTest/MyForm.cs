using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtoTest
{
    class MyForm:Eto.Forms.Form
    {
        public MyForm()
        {
            // sets the client (inner) size of the window for your content
            this.ClientSize = new Eto.Drawing.Size(600, 400);

            this.Title = "Hello, Eto.Forms";
        }
    }
}
