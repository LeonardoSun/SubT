using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SubtitleTranslation
{
    public class Process
    {
        public Segment CurrentSeg { get; private set; }

        public List<Segment> Segments = new List<Segment>();

        List<Sentence> sentences = new List<Sentence>();

        public Process()
        {
            CurrentSeg = new HeaderSeg();
            CurrentSeg.Finished += CreateNextSegment;
        }

        public async Task Do(string path)
        {

            FileInfo fi = new FileInfo(path);
            var newPath = Path.Combine(fi.Directory.FullName, fi.Name.TrimEnd(fi.Extension.ToCharArray()) + ".srt");
            if (File.Exists(newPath))
            {
                var dr = MessageBox.Show("srt exists, over write?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dr == DialogResult.Cancel)
                {
                    return;
                }
            }
            LoadFile(path);
            GetSentences();
            await TranslateFile();
            await SaveFile(newPath);
            MessageBox.Show("Done.", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        public void CreateNextSegment()
        {
            Segments.Add(CurrentSeg);
            CurrentSeg = new ContentSeg();
            CurrentSeg.Finished += CreateNextSegment;
        }

        public void GetSentences()
        {
            Sentence sen = new Sentence();
            string[] rest;
            foreach (var seg in Segments)
            {
                if (seg is HeaderSeg)
                {
                    continue;
                }
                var content = seg as ContentSeg;
                if (content == null)
                {
                    throw new NotImplementedException();
                }
                var originalText = content.OriginalText;
                GetSen:
                bool complete = sen.Get(content, originalText, out rest);
                if (complete)
                {
                    sentences.Add(sen);
                    sen = new Sentence();
                    if (rest != null && rest.Length > 0)
                    {
                        originalText = rest;
                        goto GetSen;
                    }
                }
            }
        }

        public async Task SaveFile(string path)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var seg in Segments)
            {
                seg.GetText(sb);
            }
            FileInfo fi = new FileInfo(path);
            using (StreamWriter sw = new StreamWriter(fi.Open(FileMode.OpenOrCreate)))
            {
                await sw.WriteAsync(sb.ToString());
            }
            //File.WriteAllText(path, sb.ToString());
        }

        public async Task TranslateFile()
        {
            int time = 0;
            Start:
            string fifties = string.Empty;
            int senLength = 0;
            for (int i = 0; i < sentences.Count - time * 50 && i < 50; i++, senLength++)
            {
                fifties += sentences[i + time * 50].Text + "\n";
            }
            fifties.TrimEnd('\n');
            string[] ns;
            try
            {
                string s = await Translate.Trans(fifties);
                ns = TranslationResult.GetTranslatedTexts(s);
            }
            catch (Exception)
            {
                try
                {
                    string s = await Translate.Trans(fifties);
                    ns = TranslationResult.GetTranslatedTexts(s);
                }
                catch (Exception)
                {
                    try
                    {
                        string s = await Translate.Trans(fifties);
                        ns = TranslationResult.GetTranslatedTexts(s);
                    }
                    catch (Exception)
                    {
                        string s = await Translate.Trans(fifties);
                        ns = TranslationResult.GetTranslatedTexts(s);
                    }
                }
            }
            if (ns.Length != senLength)
            {
                throw new NotImplementedException();
            }
            for (int i = 0; i < ns.Length; i++)
            {
                sentences[i + time * 50].SetTranslation(ns[i]);
            }
            if (ns.Length + time * 50 < sentences.Count)
            {
                time++;
                goto Start;
            }
            //return;
            //foreach (var sen in sentences)
            //{
            //    try
            //    {
            //        string s = await Translate.Trans(sen.Text);
            //        var ns = TranslationResult.GetTranslatedText(s);
            //        sen.SetTranslation(ns);
            //    }
            //    catch (Exception)
            //    {
            //        try
            //        {
            //            string s = await Translate.Trans(sen.Text);
            //            var ns = TranslationResult.GetTranslatedText(s);
            //            sen.SetTranslation(ns);
            //        }
            //        catch (Exception)
            //        {
            //            try
            //            {
            //                string s = await Translate.Trans(sen.Text);
            //                var ns = TranslationResult.GetTranslatedText(s);
            //                sen.SetTranslation(ns);
            //            }
            //            catch (Exception)
            //            {
            //                string s = await Translate.Trans(sen.Text);
            //                var ns = TranslationResult.GetTranslatedText(s);
            //                sen.SetTranslation(ns);
            //            }
            //        }
            //    }
            //}
        }

        public void LoadFile(string path)
        {
            var lines = File.ReadAllLines(path);
            ParseFile(lines);
        }

        public void ParseFile(string[] lines)
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
        }
    }
}
