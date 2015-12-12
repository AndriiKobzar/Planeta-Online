using Planeta_Online.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

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
                CreatorPhone = @event.CreatorPhone,
                Name = @event.Name,
                Description = @event.Description,
                Attachments = @event.Attachment
            };
            foreach (Event e in db.Events)
            {
                if (GetIntersection(newEvent.From, newEvent.Till, e.From, e.Till) != TimeSpan.Zero)
                {
                    return View(@event);
                }
            }
            if (ModelState.IsValid)
            {
                ////add attachments to event
                //foreach (HttpPostedFile file in Request.Files)
                //{
                //    if (file != null && file.ContentLength > 0)
                //    {
                //        var fileName = Path.GetFileName(file.FileName);
                //        //get attachment's path
                //        var path = Path.Combine(Server.MapPath("~/Attachments/"), fileName);
                //        //add file path and semicolon for separation
                //        newEvent.Attachments += path + ";";
                //        //save attachment's path
                //        file.SaveAs(path);
                //    }
                //}
                db.EventApplications.Add(newEvent);
                db.SaveChanges();
                return RedirectToAction("Index", "Events");
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
        //    var events = db.Events.ToList();
        //    var list = new List<JSONEvent>();
        //    foreach(Event _event in events)
        //    {
        //        list.Add(new JSONEvent()
        //            {
        //                date = _event.From.Date.ToString().Substring(0,8),
        //                start = _event.From.TimeOfDay.ToString(),
        //                end = _event.Till.TimeOfDay.ToString(),
        //                id= _event.Id.ToString(),
        //                url = Url.Action("Details", "Events", new { id=_event.Id}), 
        //                title=_event.Name
        //            });
        //    }
        //    string json = JsonConvert.SerializeObject(list);
            return View();
        }
        public ActionResult GetEvents(double start, double end)
        {
            var fromDate = ConvertFromUnixTimestamp(start);
            var toDate = ConvertFromUnixTimestamp(end);

            //Get the events
            //You may get from the repository also
            var eventList = GetEvents();

            var rows = eventList.ToArray();
            return Json(rows, JsonRequestBehavior.AllowGet);
        }

        private List<JSONEvent> GetEvents()
        {
            List<JSONEvent> eventList = new List<JSONEvent>();
            foreach(Event _event in db.Events)
            {
                eventList.Add(new JSONEvent()
                {
                    id = _event.Id.ToString(),
                    title = _event.Name,
                    start = _event.From.ToString("s"),
                    end = _event.Till.ToString("s"),
                    url = Url.Action("Details","Events",new{id=_event.Id.ToString()})
                });
            }
            return eventList;
        }

        private static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
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