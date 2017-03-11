using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SubtitleTranslation
{
    public class StringValue
    {
        public StringValue(string s)
        {
            _value = s;
        }
        public string Value { get { return _value; } set { _value = value; } }
        string _value;
    }
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

        public async Task Do(string path, string fromLang = "en", string toLang = "zh")
        {

            FileInfo fi = new FileInfo(path);
            var newPath = Path.Combine(fi.Directory.FullName, fi.Name.TrimEnd(fi.Extension.ToCharArray()) + ".srt");
            MessageBoxButtons btns;
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
            LoadFile(path);
            GetSentences();
            await TranslateFile(fromLang, toLang);
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
            var fileContent = Encoding.Convert(Encoding.UTF8, Encoding.Default, Encoding.UTF8.GetBytes(sb.ToString()));
            using (var stream = fi.OpenWrite())
            {
                await stream.WriteAsync(fileContent, 0, fileContent.Length);

            }
            //using (StreamWriter sw = new StreamWriter(fi.Open(FileMode.OpenOrCreate)))
            //{
            //    await sw.WriteAsync(Encoding.UTF8.GetString(fileContent));
            //}
            ////File.WriteAllText(path, sb.ToString());
        }

        public async Task TranslateFile(string fromLang = "en", string toLang = "zh")
        {
            int time = 0;
            int part = 50;
            Start:
            string fifties = string.Empty;
            int senLength = 0;
            for (int i = 0; i < sentences.Count - time * part && i < part; i++, senLength++)
            {
                fifties += sentences[i + time * part].Text + "\n";
            }
            fifties.TrimEnd('\n');
            string[] ns;
            try
            {
                string s = await Translate.Trans(fifties, fromLang, toLang);
                ns = TranslationResult.GetTranslatedTexts(s);
            }
            catch (Exception)
            {
                try
                {
                    string s = await Translate.Trans(fifties, fromLang, toLang);
                    ns = TranslationResult.GetTranslatedTexts(s);
                }
                catch (Exception)
                {
                    try
                    {
                        string s = await Translate.Trans(fifties, fromLang, toLang);
                        ns = TranslationResult.GetTranslatedTexts(s);
                    }
                    catch (Exception)
                    {
                        string s = await Translate.Trans(fifties, fromLang, toLang);
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
                sentences[i + time * part].SetTranslation(ns[i]);
            }
            if (ns.Length + time * part < sentences.Count)
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
#if DEBUG
            int cur = 0;
#endif
            foreach (var line in lines)
            {
                try
                {
                    CurrentSeg.Parse(line);
                }
                catch (NotImplementedException e)
                {
                    if (e.Message == "not vtt")
                    {
                        CreateNextSegment();
                        CurrentSeg.Parse(line);
                    }
                    else
                    {
                        throw e;
                    }

                }
#if DEBUG
                cur++;
#endif
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
