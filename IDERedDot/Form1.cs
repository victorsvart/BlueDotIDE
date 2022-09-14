using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bunifu.Utils;

namespace IDERedDot
{
    public partial class Form1 : BunifuForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void codeRichTextBox_TextChanged(object sender, EventArgs e)
        {
            // keywords e funções
            string keywords = @"\b(public|private|partial|static|namespace|class|using|void|foreach|in|import|as|from|canvas|place)\b";
            MatchCollection keywordsMatches = Regex.Matches(input: codeRichTextBox.Text, pattern: keywords);

            // tipos e classes 
            string type = @"\b(Console)\b";
            MatchCollection typeMatches = Regex.Matches(input: codeRichTextBox.Text, pattern: type);

            // comentarios
            string comments = @"(\/\/.*?$|\/\*.*?\*\/|\*#)";
            MatchCollection commentsMatches = Regex.Matches(input: codeRichTextBox.Text, pattern: comments, RegexOptions.Multiline);

            // strings
            string String = "\".+?\"";
            MatchCollection StringMatches = Regex.Matches(input: codeRichTextBox.Text, pattern: String);

            // salvando posição
            int originalIndex = codeRichTextBox.SelectionStart;
            int originalLength = codeRichTextBox.SelectionLength;
            Color originalColor = Color.LightGray;

            tree.Focus();

            codeRichTextBox.SelectionStart = 0;
            codeRichTextBox.SelectionLength = codeRichTextBox.Text.Length;
            codeRichTextBox.SelectionColor = originalColor;

            foreach (Match m in keywordsMatches)
            {
                codeRichTextBox.SelectionStart = m.Index;
                codeRichTextBox.SelectionLength = m.Length;
                codeRichTextBox.SelectionColor = this.HeaderBackColor;
            }
            foreach (Match m in typeMatches)
            {
                codeRichTextBox.SelectionStart = m.Index;
                codeRichTextBox.SelectionLength = m.Length;
                codeRichTextBox.SelectionColor = Color.DarkCyan;
            }
            foreach (Match m in commentsMatches)
            {
                codeRichTextBox.SelectionStart = m.Index;
                codeRichTextBox.SelectionLength = m.Length;
                codeRichTextBox.SelectionColor = Color.Green;
            }
            foreach (Match m in StringMatches)
            {
                codeRichTextBox.SelectionStart = m.Index;
                codeRichTextBox.SelectionLength = m.Length;
                codeRichTextBox.SelectionColor = Color.Crimson;
            }

            codeRichTextBox.SelectionStart = originalIndex;
            codeRichTextBox.SelectionLength = originalLength;
            codeRichTextBox.SelectionColor = originalColor;

            codeRichTextBox.Focus();


        }


        private void button1_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Show(MousePosition);
        }
        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }
        private void button5_Click(object sender, EventArgs e)
        {

        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            Cursor.Current = Cursors.WaitCursor;
            tree.Nodes.Clear();
            foreach (var item in Directory.GetDirectories(folderBrowserDialog1.SelectedPath))
            {
                DirectoryInfo di = new DirectoryInfo(item);
                var node = tree.Nodes.Add(di.Name, di.Name,0);
                node.Tag = di;
            }
            foreach (var item in Directory.GetFiles(folderBrowserDialog1.SelectedPath))
            {
                FileInfo di = new FileInfo(item);
                var node = tree.Nodes.Add(di.Name, di.Name, 1);
                node.Tag = di;
            }
            Cursor.Current = Cursors.Default;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void tree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag == null)
            {
                //return
            }
            else if (e.Node.Tag.GetType() == typeof(DirectoryInfo))
            {
                e.Node.Nodes.Clear();
                foreach (var item in Directory.GetDirectories(((DirectoryInfo)e.Node.Tag).FullName))
                {
                    {
                        DirectoryInfo di = new DirectoryInfo(item);
                        var node = e.Node.Nodes.Add(di.Name, di.Name, 0,0);
                        node.Tag = di;
                    }
                }
                foreach (var item in Directory.GetFiles(((DirectoryInfo)e.Node.Tag).FullName))
                {
                    FileInfo di = new FileInfo(item);
                    var node = e.Node.Nodes.Add(di.Name, di.Name, 1,1);
                    node.Tag = di;
                }
            }
            else
            {
                codeRichTextBox.Text = File.ReadAllText(((FileInfo)e.Node.Tag).FullName);
            }



            }
    }
}
