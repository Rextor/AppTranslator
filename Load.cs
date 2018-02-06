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
                Global.module = ModuleDefMD.Load(textBox1.Text);
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
