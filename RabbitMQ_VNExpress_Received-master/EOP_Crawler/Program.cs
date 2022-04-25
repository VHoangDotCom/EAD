using HtmlAgilityPack;
using System;
using System.Text;

namespace EOP_Crawler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
          /*  HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load("https://vnexpress.net/bong-da");

            var nodes = document.DocumentNode.SelectNodes("//h2/a");
            foreach (var item in nodes)
            {
                var message = item.GetAttributeValue("href", "");
                var body = Encoding.UTF8.GetBytes(message);
                
                var link = Encoding.UTF8.GetString(body);

                var loadLink = new HtmlWeb();
                HtmlDocument doc = loadLink.Load(link); // Lấy nội dung bên trong link đó
                var title = doc.QuerySelector("h1.title-detail").InnerHtml; // tìm đến những h1 có class= title-detail
                var description = doc.QuerySelector("p.description").InnerText;
                var image = doc.QuerySelector("img").Attributes["src"].Value;

                Console.WriteLine("\n {0}", title);
            }*/

            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load("https://eop.edu.vn/study/unit?id=dgykc8LahtxOjQSKfR6Q%2BQjw%3D%3D&s=F840AF3ED194D4A");

            var nodes = document.DocumentNode.SelectNodes("//div[(@class='dpop allow vocabulary dgtaskdone')]/a");
            foreach (var item in nodes)
            {
                var message = item.GetAttributeValue("href", "");
                var body = Encoding.UTF8.GetBytes(message);

                var link = Encoding.UTF8.GetString(body);

                var loadLink = new HtmlWeb();
                HtmlDocument doc = loadLink.Load(link); // Lấy nội dung bên trong link đó
                var title = doc.QuerySelector("h4#title-detail").InnerHtml; // tìm đến những h1 có class= title-detail
                var description = doc.QuerySelector("p.description").InnerText;
                var image = doc.QuerySelector("img").Attributes["src"].Value;

                Console.WriteLine("\n {0}", message);
            }
        }
    }
}
