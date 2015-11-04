using Planeta_Online.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using System.Data.Entity;
using System.Net;

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
        #region Events
        public ActionResult Events()
        {
            List<EventViewModelForAdmin> events = new List<EventViewModelForAdmin>();
            foreach(Event _event in db.Events.ToList())
            {
                EventViewModelForAdmin model = new EventViewModelForAdmin() { Description = _event.Description, From = _event.From, Till = _event.Till, Name = _event.Name, Id=_event.Id };
                // make a query to select visitors for this event
                var query = from entry in db.EventRegistrations where entry.EventId == _event.Id select entry;
                query = db.EventRegistrations.Where(m => m.EventId == _event.Id);
                model.Visitors = query.ToList().Count;
                events.Add(model);
            }
            return View(events);
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
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest); 
            }
            else
            {
                db.EventApplications.Remove(application);
                db.Events.Add(new Event() { Name=application.Name,
                                            From=application.From,
                                            Till=application.Till,
                                            Attachments=application.Attachments,
                                            Description=application.Description});
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
        }
        private void GeneratePDF(Event @event)
        {
            Document doc = new Document(PageSize.LETTER, 10, 10, 42, 35);
            //PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream());
            doc.Open();
            Paragraph p = new Paragraph("Podannya");
            doc.Close();
        }
        public ActionResult Visitors(int? id)
        {
            if(id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if(@event!=null)
            {
                var model = new VisitorsViewModel();
                model.EventName = @event.Name;
                var query = from entry in db.EventRegistrations where entry.EventId == @event.Id select entry.VisitorName;
                model.Visitors = query.ToList();
                return View(model);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        #endregion
        #region Books
        public ActionResult Books()
        {
            return View(db.Books.ToList());
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
        public ActionResult EditBook(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book != null)
            {
                return View(db.Books.Find(id));
            }
            else return RedirectToAction("Books");
        }
        [HttpPost]
        public ActionResult EditBook(Book model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        // GET: ApplicationUsers/Delete/5
        public ActionResult DeleteBook(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Administrator/DeleteBook/5
        [HttpPost, ActionName("DeleteBook")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChanges();
            return RedirectToAction("Books");
        }

        #endregion
    }
}