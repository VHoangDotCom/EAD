using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrawWebAssignment.Models
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

        public bool Validation()
        {
            if (string.IsNullOrEmpty(Title))
            {
                return false;
            }
            if (string.IsNullOrEmpty(Image))
            {
                return false;
            }
            if (string.IsNullOrEmpty(Description))
            {
                return false;
            }
            if (string.IsNullOrEmpty(Content))
            {
                return false;
            }
            return true;
        }
    }
}