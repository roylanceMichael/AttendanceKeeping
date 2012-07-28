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
	public class ClassRosterController : Controller
	{
		private AttendanceKeepingContext db = new AttendanceKeepingContext();

		//
		// GET: /ClassRoster/
		[NoCache]
		public ActionResult Index()
		{
			var classrosters = db.ClassRosters
				.Include(c => c.ClassSchedule)
				.Include(c => c.Person);
			return View(classrosters.ToList());
		}

		//
		// GET: /ClassRoster/Details/5
		[NoCache]
		public ActionResult Details(int id = 0)
		{
			ClassRoster classroster = db.ClassRosters.Find(id);
			if (classroster == null)
			{
				return HttpNotFound();
			}
			return View(classroster);
		}

		//
		// GET: /ClassRoster/Create
		[NoCache]
		public ActionResult Create()
		{
			ViewBag.ClassScheduleId = new SelectList(db.ClassSchedules, "ClassScheduleId", "ClassName");
			ViewBag.PersonId = new SelectList(db.Persons, "PersonId", "LastName");
			return View();
		}

		//
		// POST: /ClassRoster/Create
		[NoCache]
		[HttpPost]
		public ActionResult Create(ClassRoster classroster)
		{
			if (ModelState.IsValid)
			{
				db.ClassRosters.Add(classroster);
				db.SaveChanges();
				return RedirectToAction("Index", "ClassSchedule");
			}

			ViewBag.ClassScheduleId = new SelectList(db.ClassSchedules, "ClassScheduleId", "ClassName");
			ViewBag.PersonId = new SelectList(db.Persons, "PersonId", "LastName", classroster.PersonId);
			return View(classroster);
		}

		//
		// GET: /ClassRoster/Edit/5
		[NoCache]
		public ActionResult Edit(int id = 0)
		{
			ClassRoster classroster = db.ClassRosters.Find(id);
			if (classroster == null)
			{
				return HttpNotFound();
			}
			ViewBag.ClassScheduleId = new SelectList(db.ClassSchedules, "ClassScheduleId", "ClassName", classroster.ClassScheduleId);
			ViewBag.PersonId = new SelectList(db.Persons, "PersonId", "LastName", classroster.PersonId);
			return View(classroster);
		}

		//
		// POST: /ClassRoster/Edit/5
		[NoCache]
		[HttpPost]
		public ActionResult Edit(ClassRoster classroster)
		{
			if (ModelState.IsValid)
			{
				db.Entry(classroster).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			ViewBag.ClassScheduleId = new SelectList(db.ClassSchedules, "ClassScheduleId", "ClassName", classroster.ClassScheduleId);
			ViewBag.PersonId = new SelectList(db.Persons, "PersonId", "LastName", classroster.PersonId);
			return View(classroster);
		}

		//
		// GET: /ClassRoster/Delete/5
		[NoCache]
		public ActionResult Delete(int id = 0)
		{
			ClassRoster classroster = db.ClassRosters.Include("ClassSchedule").Include("Person").FirstOrDefault(t => t.ClassRosterId == id);
			if (classroster == null)
			{
				return HttpNotFound();
			}
			return View(classroster);
		}

		//
		// POST: /ClassRoster/Delete/5
		[NoCache]
		[HttpPost, ActionName("Delete")]
		public ActionResult DeleteConfirmed(int id)
		{
			ClassRoster classroster = db.ClassRosters.Find(id);
			db.ClassRosters.Remove(classroster);
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