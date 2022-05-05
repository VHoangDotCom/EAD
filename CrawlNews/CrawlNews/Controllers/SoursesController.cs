using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CrawlNews.Models;
using CrawlNews.config;

namespace CrawlNews.Controllers
{
    public class SoursesController : Controller
    {
        private DBConnection db = new DBConnection();

        // GET: Sourses
        public ActionResult Index()
        {
            return View(db.Sourses.ToList());
        }

        // GET: Sourses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sourse sourse = db.Sourses.Find(id);
            if (sourse == null)
            {
                return HttpNotFound();
            }
            return View(sourse);
        }

        // GET: Sourses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sourses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Url,SelectorSubUrl,SelectorTitle,SelectorImage,SelectorDescrition,SelectorContent,CategoryId")] Sourse sourse)
        {
            if (ModelState.IsValid)
            {
                db.Sourses.Add(sourse);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sourse);
        }

        // GET: Sourses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sourse sourse = db.Sourses.Find(id);
            if (sourse == null)
            {
                return HttpNotFound();
            }
            return View(sourse);
        }

        // POST: Sourses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Url,SelectorSubUrl,SelectorTitle,SelectorImage,SelectorDescrition,SelectorContent,CategoryId")] Sourse sourse)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sourse).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sourse);
        }

        // GET: Sourses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sourse sourse = db.Sourses.Find(id);
            if (sourse == null)
            {
                return HttpNotFound();
            }
            return View(sourse);
        }

        // POST: Sourses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sourse sourse = db.Sourses.Find(id);
            db.Sourses.Remove(sourse);
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
