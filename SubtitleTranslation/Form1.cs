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
        private string[] _languages =
        {
            "Auto",
            "English",
            "Chinese",
            "German",
            "Japanese",
            "Korean",
            "French",
            "Russian",
            "Thai",
        };

        [Localizable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string[] Languages
        {
            get
            {
                return _languages;
            }
        }

        public Form1()
        {
            InitializeComponent();

            comboBox_from.DataSource = GetLanguagesSource();
            comboBox_to.DataSource = GetLanguagesSource();
            //Translate.Trans();//"苹果", "zh", "en"
        }

        private BindingSource GetLanguagesSource()
        {
            BindingSource bs = new BindingSource();

            bs.DataSource = typeof(string);
            foreach (var item in Languages)
            {
                bs.Add(item);
            }

            return bs;
        }

        private async void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = null;
            openFileDialog1.Filter = "Web字幕文件(*.vtt)|*.vtt|Srt字幕文件(*.srt)|*.srt";// |所有文件(*.*)|*.*
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
            string fromLang = GetLanguageName(comboBox_from.Text);
            string toLang = GetLanguageName(comboBox_to.Text);
            await p.Do(path, fromLang, toLang);
        }

        private string GetLanguageName(string text)
        {
            string lang;
            switch (text)
            {
                case "English":
                    lang = "en";
                    break;
                case "Chinese":
                    lang = "zh";
                    break;
                case "German":
                    lang = "de";
                    break;
                case "Japanese":
                    lang = "jp";
                    break;
                case "Korean":
                    lang = "kor";
                    break;
                case "French":
                    lang = "fra";
                    break;
                case "Russian":
                    lang = "ru";
                    break;
                case "Thai":
                    lang = "th";
                    break;
                default:
                case "Auto":
                    lang = "auto";
                    break;
            }

            return lang;
        }
    }
}
