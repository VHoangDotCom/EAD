using CrawWebAssignment.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CrawWebAssignment.Data
{
    public class CrawDBContext : DbContext
    {
        public CrawDBContext() : base("name=CrawDB")
        {
        }

        public DbSet<Source> Sources { get; set; }
        public DbSet<Article> Articles { get; set; }

        public System.Data.Entity.DbSet<CrawWebAssignment.Models.Content> Contents { get; set; }
        //public DbSet<Content> Contents { get; set; }
    }
}