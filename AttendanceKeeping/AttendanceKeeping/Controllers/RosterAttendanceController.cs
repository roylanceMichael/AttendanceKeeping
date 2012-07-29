using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttendanceKeeping.Models;

namespace AttendanceKeeping.Controllers
{
	public class RosterAttendanceController : Controller
	{
		private AttendanceKeepingContext db = new AttendanceKeepingContext();

		//
		// GET: /RosterAttendance/
		[NoCache]
		public ActionResult Index()
		{
			var rosterattendances = db.RosterAttendances
				.Include(r => r.ClassRoster)
				.Include(r => r.ClassRoster.ClassSchedule)
				.Include(r => r.ClassRoster.Person);

		    var viewModel = new RosterAttendanceViewModel
		        {
		            RosterAttendances = rosterattendances.ToList()
		        };

			return View(viewModel);
		}

		//
		// GET: /RosterAttendance/Details/5
		[NoCache]
		public ActionResult Details(int id = 0)
		{
			RosterAttendance rosterattendance = db.RosterAttendances.Find(id);
			if (rosterattendance == null)
			{
				return HttpNotFound();
			}
			return View(rosterattendance);
		}

		//
		// GET: /RosterAttendance/Create
		[NoCache]
		public ActionResult Create()
		{
			ViewBag.ClassRosterId = new SelectList(db.ClassRosters, "ClassRosterId", "ClassRosterId");
			var rosterAttendance = new RosterAttendance
				{
					ClassDate = DateTime.Now
				};
			return View(rosterAttendance);
		}

		//
		// POST: /RosterAttendance/Create
		[NoCache]
		[HttpPost]
		public ActionResult Create(RosterAttendance rosterattendance)
		{
			if (ModelState.IsValid)
			{
				db.RosterAttendances.Add(rosterattendance);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			ViewBag.ClassRosterId = new SelectList(db.ClassRosters, "ClassRosterId", "ClassRosterId", rosterattendance.ClassRosterId);
			return View(rosterattendance);
		}

		//
		// GET: /RosterAttendance/Edit/5
		[NoCache]
		public ActionResult Edit(int id = 0)
		{
			RosterAttendance rosterattendance = db.RosterAttendances.Find(id);
			if (rosterattendance == null)
			{
				return HttpNotFound();
			}
			ViewBag.ClassRosterId = new SelectList(db.ClassRosters, "ClassRosterId", "ClassRosterId", rosterattendance.ClassRosterId);
			return View(rosterattendance);
		}

		//
		// POST: /RosterAttendance/Edit/5
		[NoCache]
		[HttpPost]
		public ActionResult Edit(RosterAttendance rosterattendance)
		{
			if (ModelState.IsValid)
			{
				db.Entry(rosterattendance).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			ViewBag.ClassRosterId = new SelectList(db.ClassRosters, "ClassRosterId", "ClassRosterId", rosterattendance.ClassRosterId);
			return View(rosterattendance);
		}

		//
		// GET: /RosterAttendance/Delete/5
		[NoCache]
		public ActionResult Delete(int id = 0)
		{
			RosterAttendance rosterattendance = db.RosterAttendances.Find(id);
			if (rosterattendance == null)
			{
				return HttpNotFound();
			}
			return View(rosterattendance);
		}

		//
		// POST: /RosterAttendance/Delete/5
		[NoCache]
		[HttpPost, ActionName("Delete")]
		public ActionResult DeleteConfirmed(int id)
		{
			RosterAttendance rosterattendance = db.RosterAttendances.Find(id);
			db.RosterAttendances.Remove(rosterattendance);
			db.SaveChanges();
			return RedirectToAction("Index");
		}

		protected override void Dispose(bool disposing)
		{
			db.Dispose();
			base.Dispose(disposing);
		}
	}
}