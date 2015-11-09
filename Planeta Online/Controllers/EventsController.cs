
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
    public class EventsController : Controller
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

        // POST: Event/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EventApplicationViewModel @event)
        {
            EventApplication newEvent = new EventApplication()
            {
                From = new DateTime(@event.FromDate.Year, @event.FromDate.Month, @event.FromDate.Day, @event.FromTime.Hours, @event.FromTime.Minutes, 0, DateTimeKind.Local),
                Till = new DateTime(@event.TillDate.Year, @event.TillDate.Month, @event.TillDate.Day, @event.TillTime.Hours, @event.TillTime.Minutes, 0, DateTimeKind.Local),
                CreatorEmail = @event.CreatorEmail,
                CreatorName = @event.CreatorName,
                Name = @event.Name,
                Description = @event.Description
            };
            foreach (Event e in db.Events)
            {
                if (GetIntersection(newEvent.From, newEvent.Till, e.From, e.Till) != TimeSpan.Zero)
                {
                    return View(@event);
                }
            }
            foreach (EventApplication e in db.EventApplications)
            {
                if (GetIntersection(newEvent.From, newEvent.Till, e.From, e.Till) != TimeSpan.Zero)
                {
                    return View(@event);
                }
            }
            if (ModelState.IsValid)
            {
                //add attachments to event
                foreach (HttpPostedFileBase file in Request.Files)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        //get attachment's path
                        var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                        //add file path and semicolon for separation
                        newEvent.Attachments += path + ";";
                        //save attachment's path
                        file.SaveAs(path);
                    }
                }
                db.EventApplications.Add(newEvent);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(@event);
        }
        public ActionResult Register(int id)
        {
            EventRegistrationViewModel model = new EventRegistrationViewModel() { EventName = db.Events.Find(id).Name, EventId = id };
            return View(model);
        }
        [HttpPost]
        public ActionResult Register(EventRegistrationViewModel model)
        {
            db.EventRegistrations.Add(new EventRegistration() { EventId = model.EventId, VisitorEmail = model.VisitorEmail, VisitorName = model.VisitorName });
            db.SaveChanges();
            return View("Success");
        }
        public ActionResult Calendar()
        {
            return View();
        }

        private TimeSpan GetIntersection(DateTime mainStart, DateTime mainEnd, DateTime intervalStart, DateTime intervalEnd)
        {
            if (mainStart >= mainEnd || intervalStart >= intervalEnd)
            {
                return TimeSpan.Zero;
            }

            if (intervalStart >= mainEnd || intervalEnd <= mainStart)
            {
                return TimeSpan.Zero;
            }

            if (intervalStart >= mainStart && intervalEnd <= mainEnd)
            {
                return intervalEnd - intervalStart;
            }

            DateTime tempStart = intervalStart;
            DateTime tempEnd = intervalEnd;

            if (intervalStart < mainStart) tempStart = mainStart;
            if (intervalEnd > mainEnd) tempEnd = mainEnd;
            return tempEnd - tempStart;
        } 
    }
}