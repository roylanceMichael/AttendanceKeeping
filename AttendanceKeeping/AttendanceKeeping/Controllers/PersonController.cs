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
	public class PersonController : Controller
	{
		private AttendanceKeepingContext db = new AttendanceKeepingContext();

		//
		// GET: /Person/
		[NoCache]
		[Authorize]
		public ActionResult Index()
		{
			return View(db.Persons.ToList());
		}

		//
		// GET: /Person/Details/5
		[NoCache]
		[Authorize]
		public ActionResult Details(int id = 0)
		{
			Person person = db.Persons.Find(id);
			if (person == null)
			{
				return HttpNotFound();
			}
			return View(person);
		}

		//
		// GET: /Person/Create
		[NoCache]
		[Authorize]
		public ActionResult Create()
		{
			return View();
		}

		//
		// POST: /Person/Create

		[HttpPost]
		[NoCache]
		[Authorize]
		public ActionResult Create(Person person)
		{
			if (ModelState.IsValid)
			{
				db.Persons.Add(person);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(person);
		}

		//
		// GET: /Person/Edit/5
		[NoCache]
		[Authorize]
		public ActionResult Edit(int id = 0)
		{
			Person person = db.Persons.Find(id);
			if (person == null)
			{
				return HttpNotFound();
			}
			return View(person);
		}

		//
		// POST: /Person/Edit/5

		[HttpPost]
		[NoCache]
		[Authorize]
		public ActionResult Edit(Person person)
		{
			if (ModelState.IsValid)
			{
				db.Entry(person).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(person);
		}

		//
		// GET: /Person/Delete/5
		[NoCache]
		[Authorize]
		public ActionResult Delete(int id = 0)
		{
			Person person = db.Persons.Find(id);
			if (person == null)
			{
				return HttpNotFound();
			}
			return View(person);
		}

		//
		// POST: /Person/Delete/5

		[HttpPost, ActionName("Delete")]
		[NoCache]
		[Authorize]
		public ActionResult DeleteConfirmed(int id)
		{
			Person person = db.Persons.Find(id);
			db.Persons.Remove(person);
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