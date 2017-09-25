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
using TextAnalisis.TextMetrics;

namespace TextAnalisis
{
    public partial class MainForm : Form
    {
        TextAnaliser ctrl;

        public MainForm()
        {
            InitializeComponent();

            
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            OpenFile();
            /* string text = String.Empty;
            using (StreamReader sr = new StreamReader(@"e:\ExtremelyLargeImage.data", Encoding.Default))
            {
                text = sr.ReadToEnd();
            }

            richTextBox1.Text = text;*/
        }

        private void OpenFile()
        {
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            try
            {
                
                ctrl = new TextAnaliser(openFileDialog1.FileName);
                this.Text = "Анализ текста - " + openFileDialog1.SafeFileName;

                long offset = 0x00000000; // 0
                long length = 0x00020000; // 128 kilobytes            

                ReadFile(offset, length);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
            }
           
            GetMetrics();
        }

        private void GetMetrics()
        {
            richTextBox2.Clear();

            IMetric textLength = new TextLengthMetric();
            
            richTextBox2.Text += ctrl.GetMetric(textLength) + "\n";

            IMetric lineCount = new LineCountMetric();

            richTextBox2.Text += ctrl.GetMetric(lineCount) + "\n";
        }

        private void ReadFile(long offset, long length)
        {
            try
            {
                richTextBox1.Clear();
                richTextBox1.Text = ctrl.ReadFile(offset, length);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (StreamWriter sw = new StreamWriter(@"e:\ExtremelyLargeImage.data", true, Encoding.Default))
            {
                for (int i = 0; i < 134217728; i++)
                {
                    sw.Write('k');
                }
            } 
        }
    }
}
