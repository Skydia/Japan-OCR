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
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            if (openfile.ShowDialog() == DialogResult.OK)
            {
                Bitmap img = new Bitmap(openfile.FileName);
                TesseractEngine ocr = new TesseractEngine("./tessdata", "jpn", EngineMode.Default);
                Page page = ocr.Process(img, PageSegMode.Auto);
                textBox1.Text = page.GetText();
            }
        }
    }
}
