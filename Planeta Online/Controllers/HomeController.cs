using Planeta_Online.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Planeta_Online.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View(db.Events.ToList());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }
        
        public ActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Create(EventApplication @event)
        {
            if (ModelState.IsValid)
            {
                //add attachments to event
                foreach(HttpPostedFileBase file in Request.Files)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        //get attachment's path
                        var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                        //add file path and semicolon for separation
                        @event.Attachments += path+";";
                        //save attachment's path
                        file.SaveAs(path);
                    }
                }
                db.EventApplications.Add(@event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(@event);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Events()
        {
            return RedirectToAction("Index", "Events");
        }
        public ActionResult Register(int id)
        {
            EventRegistrationViewModel model = new EventRegistrationViewModel() { EventName=db.Events.Find(id).Name };
            return View(model);
        }
        [HttpPost]
        public ActionResult Register(EventRegistrationViewModel model)
        {
            db.EventRegistrations.Add(new EventRegistration() { EventId = model.EventId, VisitorEmail = model.VisitorEmail, VisitorName = model.VisitorName });
            db.SaveChanges();
            return View("Success");
        }
    }
}