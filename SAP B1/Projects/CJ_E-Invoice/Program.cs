using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eDSC
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Addon ad = new Addon();
            Application.Run();
        }
    }
}
