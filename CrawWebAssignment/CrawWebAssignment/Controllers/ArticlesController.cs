using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CrawWebAssignment.Data;
using CrawWebAssignment.Models;
using PagedList;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace CrawWebAssignment.Controllers
{
    public class ArticlesController : Controller
    {
        private CrawDBContext db = new CrawDBContext();
        private readonly ElasticSearchFactory _esFactory;//khai bao elasticsearch

        public ArticlesController()
        {
            _esFactory = new ElasticSearchFactory();
        }

        // GET: Articles
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder; //PageList
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewBag.UrlSortParm = String.IsNullOrEmpty(sortOrder) ? "url_desc" : "";
            ViewBag.DescSortParm = sortOrder == "Desc" ? "order_desc" : "Desc";
           
            //PageList
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            
            var articles = db.Articles.AsQueryable();
            if (!String.IsNullOrEmpty(searchString))
            {
                articles = articles.Where(s => s.Title.Contains(searchString)
                                       || s.Description.Contains(searchString)
                                       );
            }
            switch (sortOrder)
            {
                case "title_desc":
                    articles = articles.OrderByDescending(s => s.Title);
                    break;
                case "url_desc":
                    articles = articles.OrderByDescending(s => s.Url);
                    break;
                case "Desc":
                    articles = articles.OrderBy(s => s.Description);
                    break;
                case "order_desc":
                    articles = articles.OrderByDescending(s => s.Description);
                    break;
                default:
                    articles = articles.OrderBy(s => s.Url);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(articles.ToPagedList(pageNumber, pageSize));
            //return View(db.Articles.ToList());
        }

        public ActionResult IndexAjax(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder; //PageList
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewBag.UrlSortParm = String.IsNullOrEmpty(sortOrder) ? "url_desc" : "";
            ViewBag.DescSortParm = sortOrder == "Desc" ? "order_desc" : "Desc";

            //PageList
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            var articles = db.Articles.AsQueryable();
            if (!String.IsNullOrEmpty(searchString))
            {
                articles = articles.Where(s => s.Title.Contains(searchString)
                                       || s.Description.Contains(searchString)
                                      );
            }
            switch (sortOrder)
            {
                case "title_desc":
                    articles = articles.OrderByDescending(s => s.Title);
                    break;
                case "url_desc":
                    articles = articles.OrderByDescending(s => s.Url);
                    break;
                case "Desc":
                    articles = articles.OrderBy(s => s.Description);
                    break;
                case "order_desc":
                    articles = articles.OrderByDescending(s => s.Description);
                    break;
                default:
                    articles = articles.OrderBy(s => s.Url);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return PartialView(articles.ToPagedList(pageNumber, pageSize));
            //return View(db.Articles.ToList());
        }

        //Elastic Search
        public async Task<ActionResult> Search()
        {
            return View(await DoSearchAsync());
        }
        private async Task<List<Article>> DoSearchAsync(string name = "")
        {
            var response = await (_esFactory.ElasticSearchClient().SearchAsync<Article>(s => s
                        .Index("articles")
                        .Size(50)
                        .Query(q => q
                          .Match(m => m
                            .Field(f => f.Title)
                            .Query(name)
                          )
                        )
                      ));
            return response.Hits.Select(s => s.Source).ToList();
        }

        // GET: Articles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // GET: Articles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Articles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Url,Title,Image,Description")] Article article)
        {
            /* if (ModelState.IsValid)
             {
                 db.Articles.Add(article);
                 db.SaveChanges();
                 return RedirectToAction("Index");
             }

             return View(article);*/

            if (ModelState.IsValid)
            {
                try
                {
                    article.Id = Convert.ToInt32(Guid.NewGuid().ToString());
                    var response = _esFactory.ElasticSearchClient().Index<Article>(article, i => i
                    .Index("articles")
                    .Id(article.Id)
                    .Refresh(Elasticsearch.Net.Refresh.True));
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View(article);
                }
            }
            return View(article);
        }

        // GET: Articles/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            return View(await GetByIdAsync(id));
        }
        private async Task<Article> GetByIdAsync(int id)
        {
            return (await _esFactory.ElasticSearchClient().GetAsync<Article>(id, i =>
                    i.Index("articles"))).Source;
        }
        // POST: Articles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Url,Title,Image,Description")] Article article)
        {
            /* if (ModelState.IsValid)
             {
                 db.Entry(article).State = EntityState.Modified;
                 db.SaveChanges();
                 return RedirectToAction("Index");
             }
             return View(article);*/
            if (ModelState.IsValid)
            {
                try
                {
                    var response = await _esFactory.ElasticSearchClient().UpdateAsync<Article>(article, i => i
                                .Index("articles")
                                .Refresh(Elasticsearch.Net.Refresh.True));
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View(article);
                }
            }
            return View(article);
        }

        // GET: Articles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }
        //new
        public async Task<ActionResult> Delete(int id)
        {
            return View(await GetByIdAsync(id));
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id, Article article)
        {
            /*  Article article = db.Articles.Find(id);
              db.Articles.Remove(article);
              db.SaveChanges();
              return RedirectToAction("Index");*/
            try
            {
                var response = await _esFactory.ElasticSearchClient().DeleteAsync<Article>(id, i => i
                        .Index("articles")
                        .Refresh(Elasticsearch.Net.Refresh.True));
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(article);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
