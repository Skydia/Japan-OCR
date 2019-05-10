using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tesseract;

namespace Japanese_OCR
{
    public partial class Form1 : Form
    {
        OpenFileDialog openfile = new OpenFileDialog();
        String inputText;
        enum Mode
        {
            Hiragana,
            Katakana,
            Romaji,
        }

        private List<string> Database = new List<string>();

        public Form1()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            GetDatabase();
        }

        private void GetDatabase()
        {
            using (System.IO.StreamReader sr = new System.IO.StreamReader("Database.txt"))
            {
                while (!sr.EndOfStream)
                {
                    string splitMe = sr.ReadLine();
                    Database.Add(splitMe);
                }
            }
        }

        private string Convert(string text, Mode convertMode)
        {
            text = text.ToLower();

            string roma = string.Empty;
            string hira = string.Empty;
            string kata = string.Empty;

            foreach (string row in Database)
            {
                var split = row.Split('@');
                roma = split[0];
                hira = split[1];
                kata = split[2];

                switch (convertMode)
                {
                    case Mode.Romaji:
                        text = text.Replace(hira, roma);
                        text = text.Replace(kata, roma.ToUpper());
                        break;
                    case Mode.Hiragana:
                        text = text.Replace(roma, hira);
                        text = text.Replace(kata, hira);
                        break;
                    case Mode.Katakana:
                        text = text.Replace(roma, kata);
                        text = text.Replace(hira, kata);
                        break;
                }
            }
            return text;
        }

        private String process()
        {
            Bitmap img = new Bitmap(richTextBox2.Text);
            TesseractEngine ocr = new TesseractEngine("./tessdata", "jpn", EngineMode.Default);
            Page page = ocr.Process(img, PageSegMode.Auto);
            inputText = page.GetText();
            return inputText;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String inputText = process();
            richTextBox1.Text = Convert(inputText, Mode.Hiragana);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String inputText = process();
            richTextBox1.Text = Convert(inputText, Mode.Katakana);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            String inputText = process();
            richTextBox1.Text = Convert(inputText, Mode.Romaji);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (openfile.ShowDialog() == DialogResult.OK)
            {
                richTextBox2.Text = openfile.FileName;
            }
            else
            {
                inputText = "";
            }
        }
    }
}
