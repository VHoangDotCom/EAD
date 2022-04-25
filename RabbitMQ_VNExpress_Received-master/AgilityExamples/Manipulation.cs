using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgilityExamples
{
   public class Manipulation
    {
        public void InnerHTML()
        {
            var html =
        @"<body>
            <h1>This is <b>bold</b> heading</h1>
            <p>This is <u>underlined</u> paragraph</p>
			
			<h1>This is <i>italic</i> heading</h1>
			<p>This is <u>underlined</u> paragraph</p>
        </body>";

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var htmlNodes = htmlDoc.DocumentNode.SelectNodes("//body/h1");

            foreach (var node in htmlNodes)
            {
                //return  <h1>This is <b>bold</b> heading</h1>...
                Console.WriteLine(node.InnerHtml);
            }
        }
        public void InnerText()
        {
            var html =
        @"<body>
            <h1>This is <b>bold</b> heading</h1>
            <p>This is <u>underlined</u> paragraph</p>
			
			<h1>This is <i>italic</i> heading</h1>
			<p>This is <u>underlined</u> paragraph</p>
        </body>";

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var htmlNodes = htmlDoc.DocumentNode.SelectNodes("//body/h1");

            foreach (var node in htmlNodes)
            {
                //return This is bold heading
                Console.WriteLine(node.InnerText);
            }
        }
        public void ParentNode()
        {
            var html =
        @"<body>
            <h1>This is <b>bold</b> heading</h1>
            <p>This is <u>underlined</u> paragraph</p>
        </body>";

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var node = htmlDoc.DocumentNode.SelectSingleNode("//body/h1");

            HtmlNode parentNode = node.ParentNode;

            Console.WriteLine(parentNode.Name);//return body
        }

    }
}
