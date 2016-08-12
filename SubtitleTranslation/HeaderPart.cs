using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SubtitleTranslation
{
    public class HeaderPart : Part
    {
        public override void GetText(StringBuilder sb)
        {
            // do nothing, srt has no header.
        }

        public override void Parse(string line)
        {
            if (!string.Equals(line.Trim(), "WEBVTT"))
            {
                throw new NotImplementedException();
            }
        }
    }
}