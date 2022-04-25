using HtmlAgilityPack;
using System;
using System.Text;

namespace AgilityExamples
{
    class Program
    {
        public static void Main()
        {
            Parser ps = new Parser();
            Selectors sl = new Selectors();
            Manipulation mp = new Manipulation();
            CrawlDataDemo cdm = new CrawlDataDemo();
            ParseData pd = new ParseData();

            //Parser
            // ps.NodeName();
            //ps.GetHref();
            //ps.GetImage();
            ps.GetIcon();

            //Selector
            // sl.SelectNode();
            //  sl.SelectSingleNode();
            //Manipulation
            // mp.InnerHTML();

            //Crawdata Demo
            //cdm.CrawlData();

            //Parse Data
            //pd.Parse_Information();
        }
               
        
    } 
}
