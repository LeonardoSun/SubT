using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SubtitleTranslation
{
    public class NumPart : Part
    {
        string _line;
        public override void GetText(StringBuilder sb)
        {
            sb.AppendLine(_line);
        }

        public override void Parse(string line)
        {
            int result;
            if (!int.TryParse(line.Trim(), out result))
            {
                throw new NotImplementedException();
            }
            // no changed.
            _line = line.Trim();
        }
    }
}