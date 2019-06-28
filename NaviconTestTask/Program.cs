using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace TestCase
{
    class Program
    {
        public string ReadUri()
        {
            var uri = Console.ReadLine();
            return uri;
        }

        public string GetHtml(string parsedHtml)
        {
            StringBuilder sb = new StringBuilder();
            byte[] array = new byte[8192];

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(parsedHtml);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream resStream = response.GetResponseStream();

            int count = 0;

            do
            {
                count = resStream.Read(array, 0, array.Length);
                if (count != 0)
                {
                    sb.Append(Encoding.Default.GetString(array, 0, count));
                }
            }
            while (count > 0);

            return sb.ToString();
        }
        public void GetHrefsAndWritetoFile(string uri)
        {
            var web = new HtmlAgilityPack.HtmlDocument();

            web.LoadHtml(uri);

            var htmlNodes = web.DocumentNode.SelectNodes("//a[@href]")
                  .Select(p => p.GetAttributeValue("href", "not found"))
                  .ToList();

            WriteHrefsToDocument(htmlNodes.Take(20));
        }

        public void WriteHrefsToDocument(IEnumerable<string> hrefs)
        {
            string fileName = @"file.txt";

            using (StreamWriter sw = new StreamWriter(fileName, false, System.Text.Encoding.Default))
            {
                foreach (var node in hrefs.Take(20))
                {
                    sw.WriteLine(node);
                    Console.WriteLine(node);
                }
            }
        }

        static void Main(string[] args)
        {
            Program p = new Program();

            p.GetHrefsAndWritetoFile(p.GetHtml(p.ReadUri()));

            Console.ReadKey();
        }
    }
}
