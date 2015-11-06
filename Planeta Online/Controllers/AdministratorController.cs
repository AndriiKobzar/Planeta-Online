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
        public ActionResult SendApplication(int id)
        {
            EventApplication _event = db.EventApplications.Find(id);
            GeneratePDF(_event);
            return RedirectToAction("Index");
        }
        private void GeneratePDF(EventApplication @event)
        {
            Document document = new Document(PageSize.A4, 72, 65, 72, 65);

            string filename = "application" + @event.Id + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".pdf";
            filename = "~\\Applications\\" + filename;
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(Server.MapPath(filename), FileMode.Create));
            document.AddAuthor("RussianPDFtest");
            document.AddTitle("Подтверждение бронирования");
            document.AddCreationDate();

            string sylfaenpath = Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\sylfaen.ttf";
            BaseFont sylfaen = BaseFont.CreateFont(sylfaenpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font head = new Font(sylfaen, 12f, Font.NORMAL, BaseColor.BLUE);
            Font normal = new Font(sylfaen, 10f, Font.NORMAL, BaseColor.BLACK);
            Font underline = new Font(sylfaen, 10f, Font.UNDERLINE, BaseColor.BLACK);

            document.Open();
            document.Add(new Paragraph("Подання", head));
            document.Add(new Paragraph(" Бугрову Володимиру Анатолійовичу", normal));
            document.Add(new Paragraph("Просимо дозволу провести захід ЄЄЄ у диско-клубі \"Планета\" ", normal));
            document.Add(new Paragraph("Мы подтверждаем бронирование, содержащее следующую информацию:", underline));
            document.Add(new Paragraph(" ", normal));
            PdfPTable table1 = new PdfPTable(2);
            table1.TotalWidth = document.PageSize.Width - 72f - 65f;
            table1.LockedWidth = true;
            float[] widths1 = new float[] { 1f, 4f };
            table1.SetWidths(widths1);
            table1.HorizontalAlignment = 0;
            PdfPCell table1cell11 = new PdfPCell(new Phrase("Объект:", normal));
            table1cell11.Border = 0;
            table1.AddCell(table1cell11);
            PdfPCell table1cell12 = new PdfPCell(new Phrase("Ferienhaus 'Waldesruh'", normal));
            table1cell12.Border = 0;
            table1.AddCell(table1cell12);
            PdfPCell table1cell21 = new PdfPCell(new Phrase("Адрес:", normal));
            table1cell21.Border = 0;
            table1.AddCell(table1cell21);
            PdfPCell table1cell22 = new PdfPCell(new Phrase("15344 Strausberg, Am Marienberg 45", normal));
            table1cell22.Border = 0;
            table1.AddCell(table1cell22);
            PdfPCell table1cell31 = new PdfPCell(new Phrase("Номер объекта:", normal));
            table1cell31.Border = 0;
            table1.AddCell(table1cell31);
            PdfPCell table1cell32 = new PdfPCell(new Phrase("czr04012012", normal));
            table1cell32.Border = 0;
            table1.AddCell(table1cell32);
            PdfPCell table1cell41 = new PdfPCell(new Phrase("Дата заезда:", normal));
            table1cell41.Border = 0;
            table1.AddCell(table1cell41);
            PdfPCell table1cell42 = new PdfPCell(new Phrase("12.02.2012", normal));
            table1cell42.Border = 0;
            table1.AddCell(table1cell42);
            PdfPCell table1cell51 = new PdfPCell(new Phrase("Дата выезда:", normal));
            table1cell51.Border = 0;
            table1.AddCell(table1cell51);
            PdfPCell table1cell52 = new PdfPCell(new Phrase("18.02.2012", normal));
            table1cell52.Border = 0;
            table1.AddCell(table1cell52);
            PdfPCell table1cell61 = new PdfPCell(new Phrase("Человек:", normal));
            table1cell61.Border = 0;
            table1.AddCell(table1cell61);
            PdfPCell table1cell62 = new PdfPCell(new Phrase("5", normal));
            table1cell62.Border = 0;
            table1.AddCell(table1cell62);
            document.Add(table1);
            document.Close();
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