using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SubtitleTranslation
{
    public class TimePart : Part
    {
        string modified;
        public override void GetText(StringBuilder sb)
        {
            sb.AppendLine(modified);
        }

        public override void Parse(string line)
        {
            Regex reg = new Regex(@"(\d\d:\d\d:\d\d).(\d\d\d) --> (\d\d:\d\d:\d\d).(\d\d\d)");
            Match match = reg.Match(line);
            if (match.Success)
            {
                modified = reg.Replace(line, string.Format(@"{0},{1} --> {2},{3}", match.Groups[1], match.Groups[2], match.Groups[3], match.Groups[4]));
            }
            else
            {
                throw new NotImplementedException(); 
            }
        }
    }
}