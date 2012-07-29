using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttendanceKeeping.Models;

namespace AttendanceKeeping.Controllers
{
    public class HomeController : Controller
    {
        private readonly AttendanceKeepingContext _db = new AttendanceKeepingContext();
        [NoCache]
        public ActionResult Index(string message = "")
        {
            ViewBag.Message = message;
            return View();
        }

        [NoCache]
        public ActionResult ClassSchedule()
        {
            var classes = _db.ClassSchedules.ToList();
            classes.Insert(0, new ClassSchedule
                {
                    ClassScheduleId = -1
                });
            return PartialView("_ClassSelection", classes);
        }
        [NoCache]
        public ActionResult ClassRoster(int id)
        {
            var classRosters = _db.ClassRosters
                .Include("Person")
                .Where(t => t.ClassScheduleId == id)
                .ToList();
            return View("_RosterSelection", classRosters);
        }
        [NoCache]
        [HttpPost]
        public ActionResult ClassRosterUpdate(Dictionary<string, string> rosters)
        {
            if (rosters != null && rosters.Any())
            {
                foreach (var id in rosters.Keys)
                {
                    int actualTemp;
                    if (int.TryParse(id, out actualTemp))
                    {
                        _db.RosterAttendances.Add(
                            new RosterAttendance
                                {
                                    ClassDate = DateTime.Now,
                                    ClassRosterId = actualTemp
                                });
                    }
                }
                _db.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }

        [NoCache]
        public ActionResult SendEmail()
        {
            EmailClient.EmailClient.SendAttendanceEmail();
            return RedirectToAction("Index", "Home");
        }
    }
}
