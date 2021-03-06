﻿using Planeta_Online.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Planeta_Online.Controllers
{
    public class EventsController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var posters = (from events
                           in db.Events
                           where events.PosterPath != null //&& events.Till > DateTime.Now
                           orderby events.Till descending
                           select events).ToList();
            if (posters.Count == 0)
                return View(posters);
            //if there are posters, sort them appropriately
            //if all events are on one side from DateTime.Now, now need to sort them
            var numberOnOneSide = (from poster
                                   in posters
                                   where poster.Till > DateTime.Now
                                   select poster).ToList().Count;
            if (numberOnOneSide == 0 || numberOnOneSide == posters.Count)
                return View(posters);
            //if there are events on both sides, apply our strange algorythm
            //find event, which has the biggest date
            var maxDate = (from poster
                           in posters
                           orderby poster.Till descending
                           select poster).ToList()[0];
            posters.Sort((e1, e2) =>
            {
                return CompareValueForEvent(e1, maxDate).CompareTo(CompareValueForEvent(e2, maxDate));
            });
            return View(posters);
        }
        private int CompareValueForEvent(Event e, Event maxDate)
        {
            if (e.Till > DateTime.Now)
            {
                return (e.Till - DateTime.Now).Days;
            }
            else
            {
                return (DateTime.Now - e.Till).Days + CompareValueForEvent(maxDate, maxDate);
            }
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
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
            if (newEvent.From < DateTime.Now)
            {
                ModelState.AddModelError(string.Empty, "Дата початку події має бути такою, яка ще не наступила.");
            }

            if (newEvent.From >= newEvent.Till)
            {
                ModelState.AddModelError(string.Empty, "Дата та час початку події мають бути раніше, ніж дата та час кінця події.");
            }

            foreach (Event e in db.Events)
            {
                if (GetIntersection(newEvent.From, newEvent.Till, e.From, e.Till) != TimeSpan.Zero)
                {
                    ModelState.AddModelError(string.Empty, "В обраний проміжок часу вже заплановано подію.");
                    return View(@event);
                }
            }
            if (ModelState.IsValid)
            {
                db.EventApplications.Add(newEvent);
                db.SaveChanges();
                return RedirectToAction("Index", "Events");
            }
            return View(@event);
        }
        public ActionResult Register(int id)
        {
            var _event = db.Events.Find(id);
            if (_event == null)
                return HttpNotFound();
            if (_event.Till < DateTime.Now)
                return View("NoRegistration");
            EventRegistrationViewModel model = new EventRegistrationViewModel() { EventName = _event.Name, EventId = id, PosterPath = _event.PosterPath };
            return View(model);
        }
        [HttpPost]
        public ActionResult Register(EventRegistrationViewModel model)
        {
            var probableVisitor = from visitor
                                  in db.EventRegistrations
                                  where visitor.EventId == model.EventId && visitor.VisitorEmail == model.VisitorEmail
                                  select visitor;
            if (probableVisitor.Count() == 0)
            {
                db.EventRegistrations.Add(new EventRegistration() { EventId = model.EventId, VisitorEmail = model.VisitorEmail, VisitorName = model.VisitorName });
                db.SaveChanges();
                return View("Success");
            }
            else
            {
                ModelState.AddModelError("VisitorEmail", "Людина з такою поштою вже зареєструвалась на подію");
                return View(model);
            }
        }
        public ActionResult Calendar()
        {
            return View();
        }
        public ActionResult GetEvents(double start, double end)
        {
            var fromDate = ConvertFromUnixTimestamp(start);
            var toDate = ConvertFromUnixTimestamp(end);

            //Get the events
            var eventList = GetEvents();

            var rows = eventList.ToArray();
            return Json(rows, JsonRequestBehavior.AllowGet);
        }

        private List<JSONEvent> GetEvents()
        {
            List<JSONEvent> eventList = new List<JSONEvent>();
            foreach (Event _event in db.Events)
            {
                eventList.Add(new JSONEvent()
                {
                    id = _event.Id.ToString(),
                    title = _event.Name,
                    start = _event.From.ToString("s"),
                    end = _event.Till.ToString("s"),
                    url = Url.Action("Details", "Events", new { id = _event.Id.ToString() })
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