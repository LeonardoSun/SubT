using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SubtitleTranslation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //Translate.Trans();//"苹果", "zh", "en"
        }

        private async void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = null;
            openFileDialog1.Filter = "Web字幕文件(*.vtt)|*.vtt";// |所有文件(*.*)|*.*
            var result = openFileDialog1.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                path = openFileDialog1.FileName;
            }
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            Process p = new Process();
            await p.Do(path);
        }

    }
}
