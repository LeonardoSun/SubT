using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SubtitleTranslation
{
    public class HeaderSeg : Segment
    {
        List<Part> parts = new List<Part> { new HeaderPart(), new ReturnPart() };
        protected override List<Part> Parts
        {
            get
            {
                return parts;
            }
        }

        public override void GetText(StringBuilder sb)
        {
            // do nothing, srt has no header.
        }
    }
}