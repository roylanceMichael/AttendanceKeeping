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
	public class ClassScheduleController : Controller
	{
		private AttendanceKeepingContext db = new AttendanceKeepingContext();

		//
		// GET: /ClassSchedule/
		[NoCache]
		public ActionResult Index()
		{
			return View(db.ClassSchedules.ToList());
		}

		//
		// GET: /ClassSchedule/Details/5
		[NoCache]
		public ActionResult Details(int id = 0)
		{
			ClassSchedule classschedule = db.ClassSchedules.Find(id);
			if (classschedule == null)
			{
				return HttpNotFound();
			}
			return View(classschedule);
		}

		//
		// GET: /ClassSchedule/Create
		[NoCache]
		public ActionResult Create()
		{
			return View();
		}

		//
		// POST: /ClassSchedule/Create
		[NoCache]
		[HttpPost]
		public ActionResult Create(ClassSchedule classschedule)
		{
			if (ModelState.IsValid)
			{
				db.ClassSchedules.Add(classschedule);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(classschedule);
		}

		//
		// GET: /ClassSchedule/Edit/5
		[NoCache]
		public ActionResult Edit(int id = 0)
		{
			ClassSchedule classschedule = db.ClassSchedules.Find(id);
			if (classschedule == null)
			{
				return HttpNotFound();
			}
			return View(classschedule);
		}

		//
		// POST: /ClassSchedule/Edit/5
		[NoCache]
		[HttpPost]
		public ActionResult Edit(ClassSchedule classschedule)
		{
			if (ModelState.IsValid)
			{
				db.Entry(classschedule).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(classschedule);
		}

		//
		// GET: /ClassSchedule/Delete/5
		[NoCache]
		public ActionResult Delete(int id = 0)
		{
			ClassSchedule classschedule = db.ClassSchedules.Find(id);
			if (classschedule == null)
			{
				return HttpNotFound();
			}
			return View(classschedule);
		}

		//
		// POST: /ClassSchedule/Delete/5
		[NoCache]
		[HttpPost, ActionName("Delete")]
		public ActionResult DeleteConfirmed(int id)
		{
			ClassSchedule classschedule = db.ClassSchedules.Find(id);
			db.ClassSchedules.Remove(classschedule);
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