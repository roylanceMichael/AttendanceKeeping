using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AttendanceKeeping.Controllers
{
	public class HomeController : Controller
	{
		[NoCache]
		public ActionResult Index(string message = "")
		{
			ViewBag.Message = message;
			return View();
		}
	}
}
