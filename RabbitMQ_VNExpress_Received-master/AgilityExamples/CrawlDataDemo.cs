using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AgilityExamples
{
    public class CrawlDataDemo
    {
        public void CrawlData()
        {
            {
                HtmlDocument htmlDoc = new HtmlDocument();

                string url = "https://www.w3schools.com/html/html_elements.asp";
                //string url = "https://www.thestar.com/";
                //string url = "https://www.c-sharpcorner.com/UploadFile/mahesh/split-string-in-C-Sharp/";
                //string url = "https://www.google.com/search?q=anchor+tags+html&oq=anchor+tags";
                string urlResponse = URLRequest(url);

                //Convert the Raw HTML into an HTML Object
                htmlDoc.LoadHtml(urlResponse);

                //Find all title tags in the document
                /*
                <head>
                    <title>Page Title</title>
                </head>
                 */
                var titleNodes = htmlDoc.DocumentNode.SelectNodes("//title");

                Console.WriteLine("The title of the page is:");
                Console.WriteLine(titleNodes.FirstOrDefault().InnerText);

                //Find all keywords tags in the document
                //<meta name="keywords" content="HTML, CSS, XML, JavaScript">
                //- [translate] converts upper case letters to lower in cases where the author used Keywords, KEYWORDS, keywords or other
                var keywwordNodes = htmlDoc.DocumentNode.SelectNodes("//meta[translate(@name, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz') = 'keywords']");

                if (keywwordNodes != null)
                {
                    foreach (var keywordNode in keywwordNodes)
                    {
                        string content = keywordNode.GetAttributeValue("content", "");
                        if (!String.IsNullOrEmpty(content))
                        {
                            string[] keywords = content.Split(new string[] { "," }, StringSplitOptions.None);
                            Console.WriteLine("Here are the keywords:");
                            Console.WriteLine(String.Format("{0}", content));

                            foreach (var keyword in keywords)
                            {
                                Console.WriteLine(String.Format("\t- {0}", keyword));
                            }
                        }

                    }
                }

                //Find all A tags in the document
                var anchorNodes = htmlDoc.DocumentNode.SelectNodes("//a");

                if (anchorNodes != null)
                {
                    Console.WriteLine(String.Format("We found {0} anchor tags on this page. Here is the text from those tags:", anchorNodes.Count));

                    foreach (var anchorNode in anchorNodes)
                    {
                        Console.WriteLine(String.Format("{0} - {1}", anchorNode.InnerText, anchorNode.GetAttributeValue("href", "")));
                    }
                }

            }

            //General Function to request data from a Server
            static string URLRequest(string url)
            {
                // Prepare the Request
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                // Set method to GET to retrieve data
                request.Method = "GET";
                request.Timeout = 6000; //60 second timeout
                request.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows Phone OS 7.5; Trident/5.0; IEMobile/9.0)";

                string responseContent = null;

                // Get the Response
                using (WebResponse response = request.GetResponse())
                {
                    // Retrieve a handle to the Stream
                    using (Stream stream = response.GetResponseStream())
                    {
                        // Begin reading the Stream
                        using (StreamReader streamreader = new StreamReader(stream))
                        {
                            // Read the Response Stream to the end
                            responseContent = streamreader.ReadToEnd();
                        }
                    }
                }

                return (responseContent);
            }
        }
    }
}
