using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Lb2_prog_2
{
    internal static class CheckWordTextGears
    {
        private static readonly string url = "https://api.textgears.com/spelling?";

        public static async Task<bool> CheckWord(string word, string apiKey)
        {
            bool isWord = false;
            WebRequest request = WebRequest.Create(url + "key=" + apiKey + "&language=ru-RU&text=" + word.ToLower());
            WebResponse response = await request.GetResponseAsync();
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    if(!reader.ReadToEnd().Contains("id"))
                        isWord = true;
                }
            }
            response.Close();
            return isWord;
        }
    }
}
