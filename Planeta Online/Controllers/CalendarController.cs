using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planeta_Online.Models;

namespace Planeta_Online.Controllers
{
    public class CalendarController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Calendar
        public ActionResult Index()
        {
            return View(db.Events.ToList());
        }
        
    }
}