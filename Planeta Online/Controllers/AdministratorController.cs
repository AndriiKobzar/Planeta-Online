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
using System.Data.Entity.Validation;

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
                                            Description=application.Description,
                                            CreatorEmail = application.CreatorEmail, 
                                            CreatorName = application.CreatorName, 
                                            CreatorPhone = application.CreatorPhone});
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
        }
        public ActionResult DeleteEvent(int id)
        {

            db.EventApplications.Remove(db.EventApplications.Find(id));
            db.SaveChanges();
            return RedirectToAction("SubmittedEvents");
        }
        public FileResult SendApplication(int id)
        {
            EventApplication _event = db.EventApplications.Find(id);
            string filepath = GeneratePDF(_event);
            //string from = "planeta.workspace@gmail.com";
            //using (MailMessage mail = new MailMessage(from, "andrykobzar@gmail.com"))
            //{
            //    mail.Subject = "";
            //    mail.Body = "";
            //    FileStream inputStream = new FileStream(filepath, FileMode.Open);
            //    mail.Attachments.Add(new Attachment(inputStream, filepath));
            //    mail.IsBodyHtml = false;
            //    SmtpClient smtp = new SmtpClient();
            //    smtp.Host = "smtp.gmail.com";
            //    smtp.EnableSsl = true;
            //    smtp.Port = 25;
            //    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            //    smtp.UseDefaultCredentials = false;
            //    NetworkCredential networkCredential = new NetworkCredential(from, "planetahub");
            //    smtp.Credentials = networkCredential;
            //    smtp.Send(mail);
            //}
            byte[] fileBytes = System.IO.File.ReadAllBytes(filepath);
            string fileName = _event.Name+"_Podannya.pdf";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        // returns a path to pdf application for this event
        private string GeneratePDF(EventApplication @event)
        {
            // initialize the file name
            string filename = "application" + @event.Id + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".pdf";
            filename = Path.Combine(Server.MapPath("~/Applications/"), filename);
            Document document = new Document(PageSize.A4, 72, 65, 72, 65);

            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filename, FileMode.Create));
            document.AddAuthor("Planeta Hub");
            document.AddTitle("Подання на проведення заходу");
            document.AddCreationDate();

            string sylfaenpath = Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\sylfaen.ttf";
            BaseFont sylfaen = BaseFont.CreateFont(sylfaenpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font head = new Font(sylfaen, 14f, Font.NORMAL, BaseColor.BLUE);
            Font normal = new Font(sylfaen, 14f, Font.NORMAL, BaseColor.BLACK);
            Font bold = new Font(sylfaen, 14f, Font.BOLD, BaseColor.BLACK);

            document.Open();
            Paragraph toWho = new Paragraph("Проректору з навчально-виховної роботи\nКиївського національного університету ім. Т. Шевченка\nШамраю Володимиру Анатолійовичу", normal);
            toWho.Alignment = 2; //makes text left aligned
            document.Add(toWho);

            Paragraph fromWho = new Paragraph("", normal);
            fromWho.Alignment = 2;
            document.Add(fromWho);

            document.Add(new Paragraph("\n\nПодання\n\n", normal) { Alignment = 1 });

            string text = string.Format("Просимо дозволу провести {0} у диско-клубі \"Планета\" з {1:dd.mm.yyyy hh:mm} по {2:dd.mm.yyyy hh:mm}", @event.Name, @event.From, @event.Till);
            Paragraph body = new Paragraph(text, normal);
            document.Add(body);

            string signature = "\n\n________________ Шамрай Володимир Анатолійович";
            Paragraph ending = new Paragraph(signature, bold);
            ending.Alignment = 2;
            document.Add(ending);

            string responsible = "Відповідальні:\n";
            Paragraph resp1 = new Paragraph(responsible, normal);
            document.Add(resp1);

            Paragraph resp2 = new Paragraph("Заславська Інга", bold);
            document.Add(resp2);

            responsible = "студентка філософського факультету, напрямку культурологія ІІІ курс\nтел. 050 440 19 05";
            Paragraph resp3 = new Paragraph(responsible, normal);
            document.Add(resp3);
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
        public ActionResult GoToFilePicker(int? id)
        {
            if (db.EventApplications.Find(id) != null) return View("AddPoster", id);
            else return new HttpNotFoundResult();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPoster(int id, HttpPostedFileBase fileUpload)
        {
            EventApplication application = db.EventApplications.Find(id);
            if(application==null)
            {
                return new HttpNotFoundResult();
            }
            else
            {
                var path = "";
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];

                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        path = Path.Combine(Server.MapPath("~/Posters/"), fileName);
                        file.SaveAs(path);
                        path = "/Posters/" + fileName;
                    }
                }
                db.EventApplications.Remove(application);
                Event e = db.Events.Add(new Event()
                {
                    Name = application.Name,
                    CreatorEmail = application.CreatorEmail,
                    CreatorName = application.CreatorName,
                    CreatorPhone = application.CreatorPhone,
                    Description = application.Description,
                    From = application.From,
                    Till = application.Till,
                    PosterPath = path
                });
                db.SaveChanges();
            }
            return RedirectToAction("Index");
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
        
        #region Blog
        public ActionResult Blog()
        {
            var list = db.Blog.ToList();
            list.Sort(delegate (BlogPost p1, BlogPost p2) { return p2.TimeStamp.CompareTo(p1.TimeStamp); });
            return View(list);
        }
        public ActionResult CreatePost()
        {
           return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreatePost(BlogPostViewModel model)
        {
            
            db.Blog.Add(new BlogPost() {TimeStamp = DateTime.Now, Title = model.Title, Text= model.Text });
            db.Blog.OrderByDescending(elem => elem.TimeStamp);
            try
            { db.SaveChanges(); }
            catch (DbEntityValidationException e)
            {
                return View(new BlogPost() { Title=e.Message });
            }
            return RedirectToAction("Blog");
        }
        public ActionResult EditPost(int? id)
        {
            if(id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var instance = db.Blog.Find(id);
            return View(instance);
        }
        [HttpPost]
        public ActionResult EditPost(BlogPost model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Blog");
            }
            return View(model);
        }
        public ActionResult DeletePost(int id)
        {
            var instance = db.Blog.Find(id);
            return View(instance);
        }
        [HttpPost, ActionName("DeletePost")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePostConfirmed(int? id)
        {
            var instance = db.Blog.Find(id);
            db.Blog.Remove(instance);
            db.SaveChanges();
            return RedirectToAction("Books");
        }
        #endregion
   }
}