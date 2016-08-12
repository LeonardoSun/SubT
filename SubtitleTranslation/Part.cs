using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SubtitleTranslation
{
    public abstract class Part
    {
        public abstract void Parse(string line);
        public abstract void GetText(StringBuilder sb);
    }
}
