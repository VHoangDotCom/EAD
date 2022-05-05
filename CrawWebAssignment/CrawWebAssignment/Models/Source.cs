using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrawWebAssignment.Models
{
    public class Source
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string SelectorSubUrl { get; set; }
        public string SelectorTitle { get; set; }
        public string SelectorImage { get; set; }
        public string SelectorDescrition { get; set; }
        public string SelectorContent { get; set; }
        public string CategoryId { get; set; }
    }
}