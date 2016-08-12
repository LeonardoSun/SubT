using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SubtitleTranslation
{
    public abstract class Segment
    {
        public int CurrentPos { get; set; } = 0;
        protected abstract List<Part> Parts { get; }

        public event Action Finished;

        public void Parse(string line)
        {
            if (Parts.Count == 0)
            {
                throw new ApplicationException("Segment subclass didn't initialize Parts.");
            }
            Parts[CurrentPos]?.Parse(line);
            if (Parts[CurrentPos] is LanguagePart)
            {
                if ((Parts[CurrentPos] as LanguagePart).Finished)
                {
                    //CurrentPos += 2;
                    Finished();
                }
            }
            else
            {
                CurrentPos++;
            }
            if (Parts.Count == CurrentPos)
            {
                Finished();
                return;
            }
            else if (Parts.Count < CurrentPos)
            {
                throw new NotImplementedException();
            }

        }

        public abstract void GetText(StringBuilder sb);
    }
}
