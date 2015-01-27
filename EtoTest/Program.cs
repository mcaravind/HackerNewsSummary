using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtoTest
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            new Eto.Forms.Application().Run(new MyForm());
        }
    }
}
