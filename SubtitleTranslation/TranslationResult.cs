using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SubtitleTranslation
{
    [DataContract]
    internal class ContentText
    {
        [DataMember]
        internal string src;

        [DataMember]
        internal string dst;
    }

    [DataContract]
    internal class TransResult
    {
        [DataMember]
        internal string from;

        [DataMember]
        internal string to;

        [DataMember]
        internal ContentText[] trans_result;
    }

    public class TranslationResult
    {
        public static string GetTranslatedText(string src)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(TransResult));
            List<byte> bytes = new List<byte>();
            foreach (var item in src)
            {
                bytes.AddRange(BitConverter.GetBytes(item));
            }
            var result = (TransResult)ser.ReadObject(new MemoryStream(bytes.ToArray()));
            if (result.trans_result.Length != 1)
            {
                throw new NotImplementedException();
            }
            return result.trans_result[0].dst;
        }

        public static string[] GetTranslatedTexts(string src)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(TransResult));
            List<byte> bytes = new List<byte>();
            foreach (var item in src)
            {
                bytes.AddRange(BitConverter.GetBytes(item));
            }
            var result = (TransResult)ser.ReadObject(new MemoryStream(bytes.ToArray()));

            List<string> lr = new List<string>();
            foreach (var item in result.trans_result)
            {
                lr.Add(item.dst);
            }
            return lr.ToArray();
        }
    }
}
