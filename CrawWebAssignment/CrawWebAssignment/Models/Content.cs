﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrawWebAssignment.Models
{
    public class Content
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UrlSource { get; set; }
        public string LinkSelector { get; set; }
        public string UrlArticle { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
    }
}