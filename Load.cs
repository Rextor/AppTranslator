using dnlib.DotNet;
using System;
using System.Windows.Forms;

namespace App_Translator
{
    public partial class Load : Form
    {
        public Load()
        {
            InitializeComponent();
        }
        bool loaded = false;
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            if(op.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = op.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                AssemblyResolver asmResolver = new AssemblyResolver();
                ModuleContext modCtx = new ModuleContext(asmResolver);
                asmResolver.DefaultModuleContext = modCtx;
                asmResolver.EnableTypeDefCache = true;
                Global.module = ModuleDefMD.Load(textBox1.Text);
                Global.module.Context = modCtx;
                Global.module.Context.AssemblyResolver.AddToCache(Global.module);
                Global.Location = textBox1.Text;
                Reap r = new Reap();
                r.Show();
                Hide();
            }
            catch { }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
