using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SubtitleTranslation
{
    public class ContentSeg : Segment
    {
        List<Part> parts = new List<Part> { new NumPart(), new TimePart(), new LanguagePart(), new ReturnPart() };
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
    }
}