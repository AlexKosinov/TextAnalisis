using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TextAnalisis.TextMetrics;
using TextAnalisis.Core;
using System.Runtime.InteropServices;

namespace TextAnalisis
{
    public partial class MainForm : Form
    {
        TextAnalisisWorker textAnaliser;       

        public MainForm()
        {
            InitializeComponent();

            textAnaliser = new TextAnalisisWorker();
            textAnaliser.OnBeginAnalis += TextAnaliser_OnBeginAnalis;
            textAnaliser.OnCompleted += TextAnaliser_OnCompleted;

            TextBoxWatermarkExtensionMethod.SetWatermark(tstbSearch.TextBox, "Поиск...");
        }

        private void TextAnaliser_OnBeginAnalis(object obj)
        {
            toolStripProgressBar1.Style = ProgressBarStyle.Marquee;
            toolStripProgressBar1.MarqueeAnimationSpeed = 20;
        }

        private void TextAnaliser_OnCompleted(object obj)
        {
            rtbMetrics.Text += ((TextAnalisisWorker)obj).MetricResults;

            toolStripProgressBar1.Style = ProgressBarStyle.Continuous;
            toolStripProgressBar1.MarqueeAnimationSpeed = 0;
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void OpenFile()
        {
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "txt files (*.txt)|*.txt";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            try
            {
                OpenAndShowFile(openFileDialog.FileName);

                this.Text = "Анализ текста - " + openFileDialog.SafeFileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: Невозможно прочитать файл с диска. Подробнее: " + ex.Message);
            }
        }

        private void OpenAndShowFile(string fileName)
        {
            if (textAnaliser.File != null)
            {
                CloseFile();
            }

            textAnaliser.Settings = new TextFileSettings(fileName);

            GetDefaultMetrics();

            // Отображение первого блока текста
            long offset = 0x00000000; // 0
            long length = 0x00020000; // 128 kilobytes

            GetTextBlock(offset, length);
        }

        private void GetDefaultMetrics()
        {
            rtbMetrics.Clear();

            if (textAnaliser.File == null)
                return;

            // Необходимо добавить требуемые метрики
            textAnaliser.Metrics = new List<IMetric>()
                {
                    new TextLengthMetric(),
                    new SymbolWithoutSpacesMetric(),
                    new LineCountMetric()
                };

            textAnaliser.Start();
        }

        private void GetTextBlock(long offset, long length)
        {
            try
            {
                rtbText.Clear();
                rtbText.Text = textAnaliser.File.ReadTextBlock(offset, length);
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

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.Show();
        }

        private void закрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseFile();
        }

        private void CloseFile()
        {
            textAnaliser.Abort();
            rtbText.Clear();
            rtbMetrics.Clear();
            this.Text = "Анализ текста";
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            CloseFile();
        }

        private void toolStripTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if (textAnaliser.File == null)
                    return;

                var textMatches = new TextMatchesCountMetric();
                textMatches.Pattern = tstbSearch.Text;

                textAnaliser.Metrics = new List<IMetric>()
                {
                    textMatches
                };

                textAnaliser.Start();
            }
        }

        private void pbRefresh_Click(object sender, EventArgs e)
        {
            GetDefaultMetrics();
        }
    }

    public static class TextBoxWatermarkExtensionMethod
    {
        private const uint ECM_FIRST = 0x1500;
        private const uint EM_SETCUEBANNER = ECM_FIRST + 1;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, uint wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        public static void SetWatermark(this TextBox textBox, string watermarkText)
        {
            SendMessage(textBox.Handle, EM_SETCUEBANNER, 0, watermarkText);
        }
    }
}
