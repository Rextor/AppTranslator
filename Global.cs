using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Translator
{
    public class Global
    {
        public static int changed = 0;
        public static ModuleDefMD module { set; get; }
        public static string Location { set; get; }
    }
}
