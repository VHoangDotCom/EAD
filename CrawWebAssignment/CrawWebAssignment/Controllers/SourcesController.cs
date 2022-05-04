using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CrawWebAssignment.Data;
using CrawWebAssignment.Models;
using HtmlAgilityPack;
using Newtonsoft.Json;
using PagedList;
using RabbitMQ.Client;

namespace CrawWebAssignment.Controllers
{
    public class SourcesController : Controller
    {
        private CrawDBContext db = new CrawDBContext();
        public static HashSet<Content> setLink;
        private static string getSelectorTitle;
        private static string getSelectorDescription;
        private static string getSelectorImage;

        // GET: Sources
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder; //PageList
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.UrlSortParm = String.IsNullOrEmpty(sortOrder) ? "url_desc" : "";
            ViewBag.LinkSelectorParm = sortOrder == "Desc" ? "order_desc" : "Desc";

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

            var sources = db.Sources.AsQueryable();
            if (!String.IsNullOrEmpty(searchString))
            {
                sources = sources.Where(s => s.Name.Contains(searchString)
                                       || s.LinkSelector.Contains(searchString)
                                       );
            }
            switch (sortOrder)
            {
                case "name_desc":
                    sources = sources.OrderByDescending(s => s.Name);
                    break;
                case "url_desc":
                    sources = sources.OrderByDescending(s => s.Url);
                    break;
                case "Desc":
                    sources = sources.OrderBy(s => s.LinkSelector);
                    break;
                case "order_desc":
                    sources = sources.OrderByDescending(s => s.LinkSelector);
                    break;
                default:
                    sources = sources.OrderBy(s => s.Url);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(sources.ToPagedList(pageNumber, pageSize));
            // return View(db.Sources.ToList());
        }

        public ActionResult IndexAjax(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder; //PageList
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.UrlSortParm = String.IsNullOrEmpty(sortOrder) ? "url_desc" : "";
            ViewBag.LinkSelectorParm = sortOrder == "Desc" ? "order_desc" : "Desc";

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

            var sources = db.Sources.AsQueryable();
            if (!String.IsNullOrEmpty(searchString))
            {
                sources = sources.Where(s => s.Name.Contains(searchString)
                                       || s.LinkSelector.Contains(searchString)
                                       );
            }
            switch (sortOrder)
            {
                case "name_desc":
                    sources = sources.OrderByDescending(s => s.Name);
                    break;
                case "url_desc":
                    sources = sources.OrderByDescending(s => s.Url);
                    break;
                case "Desc":
                    sources = sources.OrderBy(s => s.LinkSelector);
                    break;
                case "order_desc":
                    sources = sources.OrderByDescending(s => s.LinkSelector);
                    break;
                default:
                    sources = sources.OrderBy(s => s.Url);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(sources.ToPagedList(pageNumber, pageSize));
            // return View(db.Sources.ToList());
        }

        // GET: Sources/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Source source = db.Sources.Find(id);
            if (source == null)
            {
                return HttpNotFound();
            }
            return View(source);
        }

        // GET: Sources/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sources/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Url,LinkSelector")] Source source)
        {
            if (ModelState.IsValid)
            {
                db.Sources.Add(source);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(source);
        }

        // GET: Sources/Create
        public ActionResult GetLink()
        {
            return View();
        }

        public ActionResult Fail()
        {
            return View();
        }

        public ActionResult Success()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckLink(Content content)
        {
            if (content.UrlSource != "" && content.LinkSelector != "")
            {
                try
                {
                    var web = new HtmlWeb();
                    HtmlDocument doc = web.Load(content.UrlSource);
                    var nodeList = doc.QuerySelectorAll(content.LinkSelector); // tìm đến những thẻ a nằm trong h3 có class= title-news
                    setLink = new HashSet<Content>(); // Đảm bảo link không giống nhau, nếu có sẽ bị ghi đè ở phần tử trước

                    foreach (var node in nodeList)
                    {
                        var link = node.Attributes["href"].Value;
                        if (string.IsNullOrEmpty(link)) // nếu link này null
                        {
                            continue;
                        }
                        Content sourceLink = new Content()
                        {
                            UrlSource = link,
                            LinkSelector = content.LinkSelector
                        };

                        setLink.Add(sourceLink);
                    }

                    return PartialView("ListLink", setLink);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error: " + e.Message);
                    return PartialView("Fail");
                }
            }
            return PartialView("Fail");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Preview(Content content)
        {
            if (content.UrlArticle != "" && content.Title != "" && content.Image != "" && content.Description != "")
            {
                try
                {
                    getSelectorTitle = content.Title;
                    getSelectorDescription = content.Description;
                    getSelectorImage = content.Image;

                    var web = new HtmlWeb();
                    HtmlDocument doc = web.Load(content.UrlArticle); // Lấy nội dung bên trong link đó
                    var title = doc.QuerySelector(content.Title).InnerHtml; // tìm đến những h1 có class= title-detail
                    var description = doc.QuerySelector(content.Description).InnerHtml;
                    var image = doc.QuerySelector(content.Image).Attributes["src"].Value;


                    var article = new Article()
                    {
                        Url = content.UrlArticle,
                        Title = title,
                        Description = description,
                        Image = image,
                    };

                    return PartialView("Preview", article);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error: " + e.Message);
                    return PartialView("Fail");
                }
            }
            return PartialView("Fail");
        }

        public ActionResult SaveLink()
        {
            // Cách vừa sửa vẫn chưa tối ưu vì vẫn lưu chậm
            //1. Tạo kết nối
            RabbitMQConnect obj = new RabbitMQConnect();
            IConnection cnn = obj.GetConnection();

            //2. Gửi lên database phần link
            // 2.1 Phần source
            try
            {
                bool flag1 = obj.send(cnn, JsonConvert.SerializeObject(setLink), "task_queue");
                if (flag1)
                {
                    string JsonRecived = obj.receive(cnn, "task_queue");
                    HashSet<Content> convertToHashSet = JsonConvert.DeserializeObject<HashSet<Content>>(JsonRecived);
                    foreach (var link in convertToHashSet)
                    {
                        var source = new Source()
                        {
                            Url = link.UrlSource,
                            LinkSelector = link.LinkSelector,
                        };

                        db.Sources.Add(source);
                        db.SaveChanges();

                        // 2.2 Phần Article
                        var web = new HtmlWeb();
                        HtmlDocument doc = web.Load(link.UrlSource); // Lấy nội dung bên trong link đó
                        string title = doc.QuerySelector(getSelectorTitle).InnerHtml; // tìm đến những h1 có class= title-detail
                        string description = doc.QuerySelector(getSelectorDescription).InnerHtml;
                        string image = doc.QuerySelector(getSelectorImage).Attributes["src"].Value;

                        var article = new Article()
                        {
                            Url = link.UrlSource,
                            Title = title,
                            Description = description,
                            Image = image,
                        };
                        db.Articles.Add(article);
                        db.SaveChanges();
                    }
                }

                // Cách code cũ lưu chậm
                //foreach (var link in setLink)
                //{
                //    var source = new Source()
                //    {
                //        Url = link.UrlSource,
                //        LinkSelector = link.LinkSelector,
                //    };

                //    bool flag1 = obj.send(cnn, JsonConvert.SerializeObject(source), "task_queue");
                //    if (flag1)
                //    {
                //        string JsonRecived = obj.receive(cnn, "task_queue");
                //        Source convertToSource = JsonConvert.DeserializeObject<Source>(JsonRecived);
                //        db.Source.Add(convertToSource);
                //        db.SaveChanges();
                //    }

                //    // 2.2 Phần Article
                //    var web = new HtmlWeb();
                //    HtmlDocument doc = web.Load(link.UrlSource); // Lấy nội dung bên trong link đó
                //    string title = doc.QuerySelector(getSelectorTitle).InnerHtml; // tìm đến những h1 có class= title-detail
                //    string description = doc.QuerySelector(getSelectorDescription).InnerHtml;
                //    string image = doc.QuerySelector(getSelectorImage).Attributes["src"].Value;

                //    var article = new Article()
                //    {
                //        Url = link.UrlSource,
                //        Title = title,
                //        Description = description,
                //        Image = image,
                //    };
                //    bool flag2 = obj.send(cnn, JsonConvert.SerializeObject(article), "task_queue");
                //    if (flag2)
                //    {
                //        string JsonRecived = obj.receive(cnn, "task_queue");
                //        Article convertToArticle = JsonConvert.DeserializeObject<Article>(JsonRecived);
                //        db.Article.Add(convertToArticle);
                //        db.SaveChanges();
                //    }
                //}
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error: " + e.Message);
            }

            return View("Index", db.Sources.ToList());
        }

        // GET: Sources/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Source source = db.Sources.Find(id);
            if (source == null)
            {
                return HttpNotFound();
            }
            return View(source);
        }

        // POST: Sources/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Url,LinkSelector")] Source source)
        {
            if (ModelState.IsValid)
            {
                db.Entry(source).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(source);
        }

        // GET: Sources/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Source source = db.Sources.Find(id);
            if (source == null)
            {
                return HttpNotFound();
            }
            return View(source);
        }

        // POST: Sources/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Source source = db.Sources.Find(id);
            db.Sources.Remove(source);
            db.SaveChanges();
            return RedirectToAction("Index");
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
