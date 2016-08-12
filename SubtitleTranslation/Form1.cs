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
        public Segment CurrentSeg { get; private set; }

        public List<Segment> Segments = new List<Segment>();

        public Form1()
        {
            InitializeComponent();

            CurrentSeg = new HeaderSeg();
            CurrentSeg.Finished += CreateNextSegment;

            //Translate.Trans();//"苹果", "zh", "en"
        }

        private void CreateNextSegment()
        {
            Segments.Add(CurrentSeg);
            CurrentSeg = new ContentSeg();
            CurrentSeg.Finished += CreateNextSegment;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
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
            LoadFile(path);
        }

        private void LoadFile(string path)
        {
            var lines = File.ReadAllLines(path);
            ParseFile(lines);
        }

        private void ParseFile(string[] lines)
        {
            foreach (var line in lines)
            {
                CurrentSeg.Parse(line);
            }
            if (!Segments.Contains(CurrentSeg))
            {
                // the last line is not empty, so it is not a ReturnPart.
                Segments.Add(CurrentSeg);
                //Debugger.Break();
                //MessageBox.Show("Last segment ended unexpectedly.", "Result", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            MessageBox.Show("Done.", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
