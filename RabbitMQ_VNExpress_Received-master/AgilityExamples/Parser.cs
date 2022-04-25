using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AgilityExamples
{
    public class Parser
    {
        public  void NodeName()
        {
            Console.OutputEncoding = Encoding.UTF8;
            var html = @"https://eop.edu.vn/study/task/18229?id=dgo0iW%2BfyH3r6mKHAG6s4hLw%3D%3D";

            HtmlWeb web = new HtmlWeb();

            var htmlDoc = web.Load(html);

            var node = htmlDoc.DocumentNode.SelectSingleNode("//head/title");

            Console.WriteLine("Node Name: " + node.Name + "\n" + node.OuterHtml);
        }
        public void GetHref()
        {
            HtmlWeb hw = new HtmlWeb();
            HtmlDocument doc = new HtmlDocument();
            doc = hw.Load("https://eop.edu.vn/study/task/18229?id=dgo0iW%2BfyH3r6mKHAG6s4hLw%3D%3D");
            foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
            {
                Console.WriteLine("\n");
                Console.WriteLine(link.GetAttributeValue("href", string.Empty));
            }
        }

        public void GetImage()
        {
            // declare html document
            var document = new HtmlWeb().Load("https://eop.edu.vn/study/task/18229?id=dgo0iW%2BfyH3r6mKHAG6s4hLw%3D%3D");

            // now using LINQ to grab/list all images from website
            var ImageURLs = document.DocumentNode.Descendants("img")
                                            .Select(e => e.GetAttributeValue("src", null))
                                            .Where(s => !String.IsNullOrEmpty(s));

            // now showing all images from web page one by one
            foreach (var item in ImageURLs)
            {
                if (item != null)
                {
                    Console.WriteLine(item);
                }
            }
        }

        public void GetIcon()
        {
            // website URL
            var html = @"https://nguyenvanhieu.vn/1000-bai-tap-lap-trinh-c-cua-thay-khang/";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            // declare htmlweb and load html document
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(html);

            var favicon = (dynamic)null;
            // extracting icon
            var el = htmlDoc.DocumentNode.SelectSingleNode("/html/head/link[@rel='icon' and @href]");
            if (el != null)
            {
                favicon = el.Attributes["href"].Value;

                // showing output here
                Console.WriteLine(Convert.ToString(favicon));
            }
        }



    }
}
