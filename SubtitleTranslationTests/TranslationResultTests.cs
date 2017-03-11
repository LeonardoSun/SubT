using Microsoft.VisualStudio.TestTools.UnitTesting;
using SubtitleTranslation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubtitleTranslation.Tests
{
    [TestClass()]
    public class TranslationResultTests
    {
        [TestMethod()]
        public void GetTranslatedTextTest()
        {
            var result = TranslationResult.GetTranslatedText("{\"from\":\"en\",\"to\":\"zh\",\"trans_result\":[{\"src\":\"Welcome to The Brain and Space.\",\"dst\":\"\\u6b22\\u8fce\\u6765\\u5230\\u5927\\u8111\\u548c\\u7a7a\\u95f4\\u3002\"}]}");

            if (string.IsNullOrWhiteSpace(result))
            {
                Assert.Fail();
            }
        }
        [TestMethod()]
        public void ConsoleInput()
        {
            var p = new ConsoleProcess.Program();
        }
    }
}