using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SubtitleTranslation
{
    public class ReturnPart : Part
    {
        public override void GetText(StringBuilder sb)
        {
            sb.AppendLine(string.Empty);
        }

        public override void Parse(string line)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                throw new NotImplementedException();
            }
        }
    }
}