using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrawWebAssignment.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Source> Sources { get; set; }//1 bai viet co nhieu link
    }
}