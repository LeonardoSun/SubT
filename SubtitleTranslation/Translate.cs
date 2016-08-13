using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Security.Cryptography;
namespace SubtitleTranslation
{
    public class Translate
    {
        public static string appid = "20160810000026568";
        public static string secretKey = "xXCNghHXary5AEgwaAmU";
        public static string myurl = "/api/trans/vip/translate";
        public static async Task<string> Trans(string q = "apple", string fromLang = "en", string toLang = "zh")
        {
            q = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(q));
            Random rand = new Random();
            var salt = rand.Next(32768, 65536);
            var sign = appid + q + salt.ToString() + secretKey;
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] OutBytes = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(sign));
            string signstr = "";
             for (int i = 0; i < OutBytes.Length; i++)
             {
                 signstr += OutBytes[i].ToString("x2");
             }
             myurl = myurl + "?appid=" + appid + "&q=" + System.Web.HttpUtility.UrlEncode(q) + "&from=" + fromLang + "&to=" + toLang + "&salt=" + salt.ToString() + "&sign=" + signstr;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://api.fanyi.baidu.com");
                //client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // New code:
                HttpResponseMessage response = await client.GetAsync(myurl);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    //Console.WriteLine(result);
                    return result;
                    //Product product = await response.Content.ReadAsAsync>Product>();
                    //Console.WriteLine("{0}\t${1}\t{2}", product.Name, product.Price, product.Category);
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }
}
