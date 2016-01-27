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
            return View(db.Blog.ToList());
        }
    }
}