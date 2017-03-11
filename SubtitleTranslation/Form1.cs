using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            //string path = null;
            openFileDialog1.Filter = "Web字幕文件(*.vtt)|*.vtt|Srt字幕文件(*.srt)|*.srt";// |所有文件(*.*)|*.*
            var result = openFileDialog1.ShowDialog();
            if (result != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            var pathes = openFileDialog1.FileNames;
            foreach (var path in pathes)
            {
                if (string.IsNullOrEmpty(path))
                {
                    return;
                }
                Process p = new Process();
                string fromLang = GetLanguageName(comboBox_from.Text);
                string toLang = GetLanguageName(comboBox_to.Text);
                await p.Do(path, fromLang, toLang);
            }
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

        private void onlyConvExtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //string path = null;
            openFileDialog1.Filter = "Web字幕文件(*.vtt)|*.vtt";// |所有文件(*.*)|*.*
            var result = openFileDialog1.ShowDialog();
            if (result != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            var pathes = openFileDialog1.FileNames;
            var errors = new List<string>();
            foreach (var path in pathes)
            {
                if (string.IsNullOrEmpty(path))
                {
                    continue;
                }
                try
                {
                    ToSrt(path);
                }
                catch (Exception)
                {
                    errors.Add(path);
                }
            }
            if (errors.Count<1)
            {
                MessageBox.Show("Done.", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information); 
            }
            else
            {
                string es = "These are failed:\r\n";
                foreach (var item in errors)
                {
                    es += item+"\r\n";
                }
                MessageBox.Show(es, "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ToSrt(string path)
        {
            FileInfo fi = new FileInfo(path);
            var newPath = Path.Combine(fi.Directory.FullName, fi.Name.TrimEnd(fi.Extension.ToCharArray()) + ".srt");
            if (fi.Extension == ".srt")
            {
                path = Path.Combine(fi.Directory.FullName, fi.Name.Replace(fi.Extension, "_old") + fi.Extension);
                fi.MoveTo(path);
            }
            if (File.Exists(newPath))
            {
                var dr = MessageBox.Show("srt exists, over write?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dr == DialogResult.Cancel)
                {
                    return;
                }
            }
            string modified = string.Empty;
            using (var sr = fi.OpenText())
            {
                var text = sr.ReadToEnd();
                text = text.Replace(@"WEBVTT

", string.Empty);
                Regex reg = new Regex(@"(\d\d:\d\d:\d\d).(\d\d\d) --> (\d\d:\d\d:\d\d).(\d\d\d)");

                MatchCollection matches = reg.Matches(text);
                modified = reg.Replace(text, "$1,$2 --> $3,$4");
            }
            var fiw = new FileInfo(newPath);
            using (var sw = fiw.OpenWrite())
            {
                var fileContent = Encoding.Convert(Encoding.UTF8, Encoding.Default, Encoding.UTF8.GetBytes(modified));
                sw.Write(fileContent, 0, fileContent.Length);
            }
        }
    }
}
