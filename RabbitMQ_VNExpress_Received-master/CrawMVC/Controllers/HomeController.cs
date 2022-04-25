using CrawMVC.Models;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CrawMVC.Controllers
{
    public class HomeController : Controller
    {
		public List<List<string>> table;
		public HomeController()
		{
		}
		[HttpGet]
		public ActionResult Index()
		{
			HtmlWeb web = new HtmlWeb();
			var doc = web.Load("https://www.ecdc.europa.eu/en/geographical-distribution-2019-ncov-cases");
			table = doc.DocumentNode.SelectSingleNode("//table[@class='table table-bordered table-condensed table-striped']")
			.Descendants("tr")
			.Where(tr => tr.Elements("td").Any() || tr.Elements("th").Any())
			.Select(tr => (tr.Elements("td").Any() ? tr.Elements("td") : tr.Elements("th")).Select(td => td.InnerText.Trim()).ToList())
			.ToList();
			return View(table);
		}
	}
}
