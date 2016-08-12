using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SubtitleTranslation
{
    class ExpectContent
    {
        public void Parse(Segment seg, string line)
        {
            seg.Parse(line);
        }
    }
}
