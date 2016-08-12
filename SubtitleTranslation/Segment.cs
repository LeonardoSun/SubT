using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SubtitleTranslation
{
    class Segment
    {
        public int CurrentPos { get; set; } = 0;
        List<Part> Parts { get; set; } = new List<Part>();

        public void Parse(string line)
        {

        }
    }
}
