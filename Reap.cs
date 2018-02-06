using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace App_Translator
{
    public partial class Reap : Form
    {
        public Reap()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }
        List<Item> list = new List<Item>();
        bool checkistext(MethodDef md, int i)
        {
            try
            {
                //Set text control
                if (md.Body.Instructions[i + 1].OpCode == OpCodes.Callvirt)
                    if (md.Body.Instructions[i + 1].Operand.ToString().Contains("set_Text"))
                        return true;
                //Messagebox forc#
                if (md.Body.Instructions[i + 1].OpCode == OpCodes.Call)
                    if (md.Body.Instructions[i + 1].Operand.ToString().Contains("MessageBox::Show"))
                        return true;
                if (md.Body.Instructions[i + 2].OpCode == OpCodes.Call)
                    if (md.Body.Instructions[i + 2].Operand.ToString().Contains("MessageBox::Show"))
                        return true;
                if (md.Body.Instructions[i + 3].OpCode == OpCodes.Call)
                    if (md.Body.Instructions[i + 3].Operand.ToString().Contains("MessageBox::Show"))
                        return true;
                if (md.Body.Instructions[i + 4].OpCode == OpCodes.Call)
                    if (md.Body.Instructions[i + 4].Operand.ToString().Contains("MessageBox::Show"))
                        return true;
                //Messagebox forvb
                if (md.Body.Instructions[i + 1].OpCode == OpCodes.Call)
                    if (md.Body.Instructions[i + 1].Operand.ToString().Contains("Interaction::MsgBox"))
                        return true;
                if (md.Body.Instructions[i + 2].OpCode == OpCodes.Call)
                    if (md.Body.Instructions[i + 2].Operand.ToString().Contains("Interaction::MsgBox"))
                        return true;
                if (md.Body.Instructions[i + 3].OpCode == OpCodes.Call)
                    if (md.Body.Instructions[i + 3].Operand.ToString().Contains("Interaction::MsgBox"))
                        return true;
                if (md.Body.Instructions[i + 4].OpCode == OpCodes.Call)
                    if (md.Body.Instructions[i + 4].Operand.ToString().Contains("Interaction::MsgBox"))
                        return true;
            }
            catch { }
            return false;
        }
        private void LoadStr()
        {

            foreach (TypeDef td in Global.module.GetTypes())
            {
                foreach (MethodDef md in td.Methods)
                {
                    if (md.HasBody)
                    {
                        for (int i = 0; i < md.Body.Instructions.Count; i++)
                        {
                            if (md.Body.Instructions[i].OpCode == OpCodes.Ldstr)
                            {
                                string des = "";
                                if (checkistext(md, i))
                                    des = "[+]";
                                
                                list.Add(new Item { type = td, method = md, index = i, Value = md.Body.Instructions[i].Operand.ToString(), des = des });
                            }
                        }
                    }
                }
            }
            foreach (Item it in list)
                AddItemInListView1(it);
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            if (textBox1.Text == "")
            {
                foreach (Item lvi in list)
                {
                    if (checkBox1.Checked)
                    {
                        if (lvi.des == "[+]")
                            AddItemInListView1(lvi);
                    }
                    else AddItemInListView1(lvi);
                }
            }
            else
            {
                foreach (Item lvi in list)
                {
                    if (lvi.Value.Contains(textBox1.Text))
                    {
                        if (checkBox1.Checked)
                        {
                            if (lvi.des == "[+]")
                                AddItemInListView1(lvi);
                        }
                        else AddItemInListView1(lvi);
                    }
                }
            }
        }
        private void AddItemInListView2(Item lvi)
        {
            ListViewItem item;
            item = listView2.Items.Add(lvi.type.Name.String + "::" + lvi.method + "=>" + lvi.Value);
            item.SubItems.Add(lvi.ToValue);
        }
        private void AddItemInListView1(Item lvi)
        {
            ListViewItem item;
            item = listView1.Items.Add(lvi.type.Name.String);
            item.SubItems.Add(lvi.method.Name.String);
            item.SubItems.Add(lvi.index.ToString());
            item.SubItems.Add(lvi.Value);
            item.SubItems.Add(lvi.des);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                ac.RemoveAt(listView2.SelectedItems[0].Index - 1);
                listView2.Items.Remove(listView2.SelectedItems[0]);
            }
            catch { }
        }
        List<Item> ac = new List<Item>();
        bool onupdate = false;
        int indexupdate;
        private void button3_Click(object sender, EventArgs e)
        {
            if (!onupdate)
            {
                try {
                    foreach (TypeDef td in Global.module.GetTypes())
                    {
                        if (td.Name.String == textBox2.Text)
                        {
                            foreach (MethodDef md in td.Methods)
                            {
                                if (!md.HasBody) continue;

                                if (md.Name.String == textBox3.Text)
                                {
                                    if (md.Body.Instructions[int.Parse(textBox4.Text)].Operand.ToString() == textBox5.Text)
                                    {

                                        ac.Add(new Item { type = td, method = md, index = int.Parse(textBox4.Text), Value = textBox5.Text, ToValue = textBox6.Text });
                                        AddItemInListView2(ac[ac.Count - 1]);
                                    }
                                }
                            }
                        }
                    }
                }
                catch { MessageBox.Show("An error in progress!!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
            else
            {
                onupdate = false;
                ac[indexupdate].ToValue = textBox6.Text;
                listView2.Items[indexupdate].SubItems[1].Text = textBox6.Text;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            foreach (Item it in ac)
            {
                foreach (TypeDef td in Global.module.GetTypes())
                {
                    if (td.Name.String == it.type.Name.String)
                    {
                        foreach (MethodDef md in td.Methods)
                        {
                            if (!md.HasBody) continue;

                            if (md.Name.String == it.method.Name.String)
                            {
                                if (md.Body.Instructions[it.index].OpCode == OpCodes.Ldstr)
                                    if (md.Body.Instructions[it.index].Operand.ToString() == it.Value)
                                    {
                                        md.Body.Instructions[it.index].Operand = it.ToValue;
                                        Global.changed += 1;
                                    }
                            }
                        }
                    }
                }
            }
            Save s = new Save();
            s.Show();
            Hide();
        }

        Dictionary<string,string> wordlist = new Dictionary<string, string>();
        private void Reap_Load(object sender, EventArgs e)
        {
            LoadWordList(Application.StartupPath + @"\Wordlist.txt");
            this.comboBox1.Items.AddRange(Translator.Languages.ToArray());
            this.comboBox3.Items.AddRange(Translator.Languages.ToArray());
            this.comboBox1.SelectedItem = "Persian";
            this.comboBox3.SelectedItem = "English";
            LoadStr();
        }

        private void Reap_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void sendToReaplacerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox2.Text = listView1.SelectedItems[0].SubItems[0].Text;
            textBox3.Text = listView1.SelectedItems[0].SubItems[1].Text;
            textBox4.Text = listView1.SelectedItems[0].SubItems[2].Text;
            textBox5.Text = listView1.SelectedItems[0].SubItems[3].Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This option has comming at next version.");
        }

        private void showInViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (TypeDef td in Global.module.GetTypes())
            {
                if (td.Name.String == listView1.SelectedItems[0].SubItems[0].Text)
                {
                    foreach (MethodDef md in td.Methods)
                    {
                        if (md.Name.String == listView1.SelectedItems[0].SubItems[1].Text)
                        {

                            Lshow l = new Lshow(md, int.Parse(listView1.SelectedItems[0].SubItems[2].Text));
                            l.Show();
                        }
                    }

                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            listView1.Items.Clear();

            foreach (Item lvi in list)
            {
                if (checkBox1.Checked)
                {
                    if (lvi.des == "[+]")
                        AddItemInListView1(lvi);
                }
                else AddItemInListView1(lvi);
            }
        }

        private void copyTypeDefToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(listView1.SelectedItems[0].SubItems[0].Text);
            }
            catch { }
        }

        private void copyMethodDefToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(listView1.SelectedItems[0].SubItems[1].Text);
            }
            catch { }
        }

        private void copyIndexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(listView1.SelectedItems[0].SubItems[3].Text);
            }
            catch { }
        }

        private void copyValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(listView1.SelectedItems[0].SubItems[4].Text);
            }
            catch { }
        }

        private string Normalization(string input)
        {
            input = input.Replace("\n", " ");
            input = input.Replace("\r", " ");
            return input;
        }
        private void trans()
        {
            button5.Text = "Translating...";
            groupBox1.Enabled = false;
            groupBox3.Enabled = false;
            foreach (ListViewItem lvi in listView1.Items)
            {
                textBox2.Text = lvi.SubItems[0].Text;
                textBox3.Text = lvi.SubItems[1].Text;
                textBox4.Text = lvi.SubItems[2].Text;
                textBox5.Text = lvi.SubItems[3].Text;
                if (textBox5.Text.Length != 1)
                {
                    Translator t = new Translator();
                    string toval = textBox5.Text;
                    toval = Normalization(toval);
                    if (checkBox2.Checked)
                    {
                        if (!wordlist.TryGetValue(textBox5.Text, out toval))
                            toval = t.Translate(toval, this.comboBox3.SelectedItem.ToString(), this.comboBox1.SelectedItem.ToString());
                    }
                    else toval = t.Translate(toval, this.comboBox3.SelectedItem.ToString(), this.comboBox1.SelectedItem.ToString());
                    textBox6.Text = toval;
                    button3_Click(null, null);
                }
            }
            button5.Text = "Translate";
            groupBox1.Enabled = true;
            groupBox3.Enabled = true;

        }
        public void LoadWordList(string input)
        {
            if (!File.Exists(input))
            {
                MessageBox.Show("Wordlist.txt file not found.\n" + input);
                return;
            }
            string[] list = File.ReadAllLines(input);
            foreach (string item in list)
            {
                try
                {
                    string left = item.Substring(0, item.IndexOf("=>"));
                    string right = item.Substring(item.IndexOf("=>") + 2, item.Length - item.IndexOf("=>") - 2);
                    wordlist.Add(left, right);
                }
                catch { }
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
            {
                if (MessageBox.Show("Are you sure that translate all string's (With out Filter text??)", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    return;
            }
            Thread th = new Thread(new ThreadStart(trans));
            th.IsBackground = true;
            th.Start();
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count != 0)
            {
                int index = listView2.SelectedItems[0].Index;
                Item it = ac[index];
                textBox2.Text = it.type.Name.String;
                textBox3.Text = it.method.Name.String;
                textBox4.Text = it.index.ToString();
                textBox5.Text = it.Value;
                textBox6.Text = it.ToValue;
                onupdate = true;
                indexupdate = index;
            }
            else
            {
                onupdate = false;
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            onupdate = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string tmp = comboBox1.Text;
            comboBox1.Text = comboBox3.Text;
            comboBox3.Text = tmp;
        }
    }
}
