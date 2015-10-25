using Planeta_Online.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;

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
    }
}