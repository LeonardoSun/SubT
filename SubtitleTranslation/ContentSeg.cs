using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SubtitleTranslation
{
    public class ContentSeg : Segment
    {
        LanguagePart lang = new LanguagePart();
        List<Part> parts = null;

        public string[] OriginalText { get { return lang.LanguageLines.ToArray(); } }

        public ContentSeg()
        {
            parts = new List<Part> { new NumPart(), new TimePart(), lang, new ReturnPart() };
        }

        protected override List<Part> Parts
        {
            get
            {
                return parts;
            }
        }

        public override void GetText(StringBuilder sb)
        {
            foreach (var part in Parts)
            {
                part.GetText(sb);

            }
        }

        internal void AddTranslation(string v)
        {
            lang.TranslationLines.Add(v);
        }
    }
}