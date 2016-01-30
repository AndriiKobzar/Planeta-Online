using Planeta_Online.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Planeta_Online.Controllers
{
    public class BlogController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Blog
        public ActionResult Index()
        {
            var list = db.Blog.ToList();
            list.Sort(delegate (BlogPost p1, BlogPost p2) { return p2.TimeStamp.CompareTo(p1.TimeStamp); });
            return View(list);
        }
    }
}