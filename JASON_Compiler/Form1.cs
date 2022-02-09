using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TINY_Compiler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            string Code=textBox1.Text.ToLower();
            TINY_Compiler.Start_Compiling(Code);
            PrintTokens();
            treeView1.Nodes.Add(Parser.PrintParseTree(TINY_Compiler.treeroot));
            PrintErrors();
            treeView1.ExpandAll();
        }
        void PrintTokens()
        {
            var num_regex = new Regex("^[0-9]+([.][0-9]+)?$", RegexOptions.Compiled);
            for (int i = 0; i < TINY_Compiler.Tiny_Scanner.Tokens.Count; i++)
            {
                //push the Lexemes to the table
                if (TINY_Compiler.Tiny_Scanner.Tokens.ElementAt(i).lex.Equals("int") || TINY_Compiler.Tiny_Scanner.Tokens.ElementAt(i).lex.Equals("string") || TINY_Compiler.Tiny_Scanner.Tokens.ElementAt(i).lex.Equals("float"))
                {
                    dataGridView1.Rows.Add(TINY_Compiler.Tiny_Scanner.Tokens.ElementAt(i).lex, ("DataType(" + TINY_Compiler.Tiny_Scanner.Tokens.ElementAt(i).token_type + ")"));
                }
                else if (num_regex.IsMatch(TINY_Compiler.Tiny_Scanner.Tokens.ElementAt(i).lex))
                {
                    dataGridView1.Rows.Add(TINY_Compiler.Tiny_Scanner.Tokens.ElementAt(i).lex, "Number");
                }
                else
                {
                    dataGridView1.Rows.Add(TINY_Compiler.Tiny_Scanner.Tokens.ElementAt(i).lex, TINY_Compiler.Tiny_Scanner.Tokens.ElementAt(i).token_type);
                }
            }
        }

        void PrintErrors()
        {
            for(int i=0; i<Errors.Error_List.Count; i++)
            {
                textBox2.Text += Errors.Error_List[i];
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            TINY_Compiler.TokenStream.Clear();
            dataGridView1.Rows.Clear();
            treeView1.Nodes.Clear();
            Errors.Error_List.Clear();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
