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
using System.Net.Mail;

namespace Planeta_Online.Controllers
{
    [Authorize(Roles="SuperAdmin")]
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
                                            Description=application.Description,
                                            CreatorEmail = application.CreatorEmail, 
                                            CreatorName = application.CreatorName, 
                                            CreatorPhone = application.CreatorPhone});
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
        }
        public ActionResult SendApplication(int id)
        {
            EventApplication _event = db.EventApplications.Find(id);
            string filepath = GeneratePDF(_event);
            string from = "planeta.workspace@gmail.com"; //example:- sourabh9303@gmail.com
            using (MailMessage mail = new MailMessage(from, "andrykobzar@gmail.com"))
            {
                mail.Subject = "";
                mail.Body = "";
                FileStream inputStream = new FileStream(filepath, FileMode.Open);
                mail.Attachments.Add(new Attachment(inputStream, filepath));
                mail.IsBodyHtml = false;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                smtp.Port = 25;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                NetworkCredential networkCredential = new NetworkCredential(from, "planetahub");
                smtp.Credentials = networkCredential;
                smtp.Send(mail);
            }
            return RedirectToAction("Index");
        }
        // returns a path to pdf application for this event
        private string GeneratePDF(EventApplication @event)
        {
            Document document = new Document(PageSize.A4, 72, 65, 72, 65);

            string filename = "application" + @event.Id + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".pdf";
            filename = Server.MapPath("~\\Applications\\" + filename);
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filename, FileMode.Create));
            document.AddAuthor("Planeta Hub");
            document.AddTitle("Подання на проведення заходу");
            document.AddCreationDate();

            string sylfaenpath = Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\sylfaen.ttf";
            BaseFont sylfaen = BaseFont.CreateFont(sylfaenpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font head = new Font(sylfaen, 12f, Font.NORMAL, BaseColor.BLUE);
            Font normal = new Font(sylfaen, 10f, Font.NORMAL, BaseColor.BLACK);
            Font underline = new Font(sylfaen, 10f, Font.UNDERLINE, BaseColor.BLACK);
            
            document.Open();
            Paragraph leftside = new Paragraph("Проректору з науково-педагогічної роботи\nБугрову Володимиру Анатолійовичу", normal);
            leftside.Alignment = 300;
            document.Add(leftside);
            document.Add(new Paragraph(string.Format("Просимо дозволу провести захід {0} у диско-клубі \"Планета\" ", @event.Name), normal));
            document.Add(new Paragraph(" ", normal));
            
            document.Close();
            return filename;
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
                model.Visitors = new List<VisitorViewModel>();
                var query = from entry in db.EventRegistrations where entry.EventId == @event.Id select entry;
                foreach(EventRegistration registration in query.ToList())
                {
                    model.Visitors.Add(new VisitorViewModel() { Name = registration.VisitorName, Email = registration.VisitorEmail });
                }
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