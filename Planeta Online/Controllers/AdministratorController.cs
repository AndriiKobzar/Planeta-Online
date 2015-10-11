using Planeta_Online.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Planeta_Online.Controllers
{
    //[Authorize(Roles="SuperAdmin")]
    public class AdministratorController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Administrator
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CreateBook()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateBook(BookViewModel book)
        {
            db.Books.Add(new Book() { Author = book.Author, Genre = book.Genre, Title = book.Title });
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult SubmittedEvents()
        {
            return View(db.EventApplications.ToList());
        }
        public ActionResult Confirm(int id)
        {
            EventApplication application = db.EventApplications.Find(id);
            if(application==null)
            {
                return null;
            }
            else
            {
                db.EventApplications.Remove(application);
                db.Events.Add(new Event() { Name=application.Name,
                                            Time=application.Time,
                                            Attachments=application.Attachments,
                                            Description=application.Description});
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}