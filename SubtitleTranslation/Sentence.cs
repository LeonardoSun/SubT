using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace SubtitleTranslation
{
    public class Sentence
    {
        List<string> texts = new List<string>();
        Dictionary<ContentSeg, int> contents = new Dictionary<ContentSeg, int>();

        public string Text
        {
            get
            {
                string result = string.Empty;
                foreach (var line in texts)
                {
                    result += line.Trim();
                    result += " ";
                }
                return result.Trim();
            }
        }

        public bool Get(ContentSeg content, string[] originalText, out string[] rest)
        {
            if (!contents.ContainsKey(content))
            {
                contents.Add(content, 0);
            }
            for (int i = 0; i < originalText.Length; i++)
            {
                var index = originalText[i].IndexOfAny(new char[] { '.', '?', '!' });
                if (index != -1)
                {
                    texts.Add(originalText[i].Substring(0, index + 1));
                    contents[content]++;
                    List<string> result = new List<string>();
                    var restLine = originalText[i].Substring(index + 1);
                    if (!string.IsNullOrWhiteSpace(restLine))
                    {
                        result.Add(restLine);
                    }
                    for (int j = i + 1; j < originalText.Length; j++)
                    {
                        var line = originalText[j];
                        if (string.IsNullOrWhiteSpace(line))
                        {
                            Debugger.Break();
                            MessageBox.Show("Original text has empty line.", "warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        result.Add(line);
                    }
                    rest = result.ToArray();
                    return true;
                }
                texts.Add(originalText[i]);
                contents[content]++;
            }
            rest = new string[0];
            return false;
        }

        internal void SetTranslation(string s)
        {
            int total = 0;
            foreach (var item in contents.Values)
            {
                total += item;
            }
            if (total == 0)
            {
                throw new NotImplementedException();
            }

            string[] lines = SeparateTranslation(s, total);
            int index = 0;
            foreach (var item in contents)
            {
                for (int i = index; i < item.Value + index; i++)
                {
                    item.Key.AddTranslation(lines[i]);
                }
                index += item.Value;
            }
        }

        private string[] SeparateTranslation(string s, int total)
        {
            var perLine = s.Length / total;
            perLine = perLine < 1 ? 1 : perLine;

            List<string> result = new List<string>();
            Loop:
            if (perLine >= s.Length || result.Count + 1 >= total)
            {
                result.Add(s);
                if (result.Count != total)
                {
                    throw new NotImplementedException();
                }
                return result.ToArray();
            }
            var sub = s.Substring(0, perLine);
            result.Add(sub);
            s = s.Substring(perLine);
            goto Loop;
        }
    }
}