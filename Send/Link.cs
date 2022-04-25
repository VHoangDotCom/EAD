using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Send
{
    public class Link
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string link { get; set; }
        public string LinkSelector { get; set; }
        public string TitleDetail { get; set; }
        public string ContentDetail { get; set; }
        public string ImageDetail { get; set; }
        public string RemoteSelector { get; set; }
        public int status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
