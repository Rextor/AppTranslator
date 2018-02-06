using dnlib.DotNet;
using dnlib.DotNet.Writer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App_Translator
{
    public partial class Save : Form
    {
        public Save()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog sv = new SaveFileDialog();
            sv.FileName = Global.module.Assembly.Name + "_translated.exe";
            if (sv.ShowDialog() == DialogResult.OK)
            {
                ModuleWriterOptions moduleWriterOptions = new ModuleWriterOptions(Global.module);
                moduleWriterOptions.MetaDataOptions.Flags = moduleWriterOptions.MetaDataOptions.Flags | MetaDataFlags.PreserveAll;
                moduleWriterOptions.MetaDataOptions.Flags = moduleWriterOptions.MetaDataOptions.Flags | MetaDataFlags.KeepOldMaxStack;
                moduleWriterOptions.Logger = DummyLogger.NoThrowInstance;
                Global.module.Write(sv.FileName, moduleWriterOptions);
                Application.Exit();
            }
        }

        private void Save_Load(object sender, EventArgs e)
        {
            label1.Text = "String chenged: " + Global.changed.ToString();
            label2.Text = "Name: " + Global.module.Assembly.Name;
            label3.Text = "Version: " + Global.module.Assembly.Version;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Save_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
