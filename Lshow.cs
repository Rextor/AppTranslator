using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App_Translator
{
    public partial class Lshow : Form
    {
        MethodDef method;
        int showindex;
        public Lshow(MethodDef md, int show)
        {
            method = md;
            showindex = show;
            InitializeComponent();
        }
        
        private void Lshow_Load(object sender, EventArgs e)
        {
            Text = method.FullName;
            int indexs = 0;
            foreach (Instruction inst in method.Body.Instructions)
            {
                try
                {
                    ListViewItem item;
                    item = listView1.Items.Add(indexs++.ToString());
                    item.SubItems.Add(inst.Offset.ToString());
                    item.SubItems.Add(inst.OpCode.ToString());
                    item.SubItems.Add(inst.Operand.ToString());
                }
                catch { }
            }
            listView1.Items[showindex].Selected = true;
            listView1.Items[showindex].Focused = true;
            listView1.Select();
            listView1.EnsureVisible(showindex);
        }
    }
}
