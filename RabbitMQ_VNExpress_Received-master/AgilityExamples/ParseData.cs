using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgilityExamples
{
    public class DTO
    {
        //public DateTime RefreshDateTime { get; set; }
        public string BondType { get; set; }
        public string BondCode { get; set; }
        public string Description { get; set; }
        public double BidQty { get; set; }
        public double BidYield { get; set; }
        public double BidPrice { get; set; }
        public double AskPrice { get; set; }
        public double AskYield { get; set; }
        public double AskQty { get; set; }
    }

    public class ParseData
    {
        public void Parse_Information()
        {
            var url = "https://www.mtsdata.com/content/data/public/nld/best/DSL_newtable.php";
            var web = new HtmlWeb();
            var doc = web.Load(url);

            // Using LINQ to parse HTML table smartly 
            var table = doc.DocumentNode.SelectSingleNode("(//table)[2]");
            bool header = true;
            foreach (var row in table.SelectNodes("tr"))
            {
                if (header)
                {
                    // skip header
                    header = false;
                    continue;
                }

                DTO dto = new DTO();
                var cells = row.SelectNodes("td");

                dto.BondType = cells[0].InnerText;
                dto.BondCode = cells[1].InnerText;
                dto.Description = cells[2].InnerText;
                dto.BidQty = Double.Parse(cells[3].InnerText);
                dto.BidYield = Double.Parse(cells[4].InnerText);
                dto.BidPrice = Double.Parse(cells[5].InnerText);
                dto.AskPrice = Double.Parse(cells[6].InnerText);
                dto.AskYield = Double.Parse(cells[7].InnerText);
                dto.AskQty = Double.Parse(cells[8].InnerText);

                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(dto))
                {
                    string name = descriptor.Name;
                    object value = descriptor.GetValue(dto);
                    Console.Write("{0}={1} ", name, value);
                }
                Console.WriteLine();
            }
            Console.Read();
        }
    }
}
