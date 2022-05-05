using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrawlNews.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string UrlSource { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string CategoryId { get; set; }
        public long CreatedAt { get; set; }

    }
}