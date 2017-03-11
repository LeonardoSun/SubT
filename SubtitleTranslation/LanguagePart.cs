using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SubtitleTranslation
{
    public class LanguagePart : Part
    {
        public bool Finished { get; private set; } = false;

        public List<string> LanguageLines { get; private set; } = new List<string>();

        public List<string> TranslationLines { get; set; } = new List<string>();

        public override void GetText(StringBuilder sb)
        {
            foreach (var line in LanguageLines)
            {
                sb.AppendLine(line);
            }
            foreach (var line in TranslationLines)
            {
                sb.AppendLine(line);
            }
        }

        public override void Parse(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                if (LanguageLines.Count > 0)
                {
                    Finished = true;
                    return;
                }
                else
                {
                    //throw new NotImplementedException();
                    LanguageLines.Add(string.Empty);
                }
            }
            LanguageLines.Add(line);
        }
    }
}