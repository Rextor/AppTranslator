using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Translator
{
    public class Item
    {
        public MethodDef method { set; get; }
        public TypeDef type { set; get; }
        public string Value { set; get; }
        public string ToValue { set; get; }
        public int index { set; get; }
        public string des { set; get; }
    }
}
