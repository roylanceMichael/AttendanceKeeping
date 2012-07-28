using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using AttendanceKeeping.Models;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.RelyingParty;

namespace AttendanceKeeping.Controllers
{

	[Authorize]
	public class AccountController : Controller
	{
		private static OpenIdRelyingParty _openId = new OpenIdRelyingParty();

		//
		// GET: /Account/Index
		[NoCache]
		public ActionResult Index()
		{
			return View();
		}

		//
		// GET: /Account/Login

		[AllowAnonymous]
		[ValidateInput(false)]
		[NoCache]
		public ActionResult Login()
		{
			var returnUrl = Url.Action("Index", "Home");
			var response = _openId.GetResponse();
			if (response == null)
			{
				// Stage 2: user submitting Identifier
				try
				{
					var request = _openId.CreateRequest(WellKnownProviders.Google);
					var fetch = new FetchRequest();
					fetch.Attributes.AddRequired(WellKnownAttributes.Contact.Email);
					request.AddExtension(fetch);
					return request.RedirectingResponse.AsActionResult();
				}
				catch (ProtocolException ex)
				{
					ViewData["Message"] = ex.Message;
					return View("Login");
				}
			}
			else
			{
				// Stage 3: OpenID Provider sending assertion response
				switch (response.Status)
				{
					case AuthenticationStatus.Authenticated:
						var fetch = response.GetExtension<FetchResponse>();
						string email = string.Empty;

						if (fetch != null)
						{
							email = fetch.GetAttributeValue(WellKnownAttributes.Contact.Email);
						}
						if (AttendanceKeepingContext.AuthorizeEmail(email))
						{
							FormsAuthentication.SetAuthCookie(email, false);
						}
						else
						{
							return RedirectToAction("Index", "Home");
						}

						if (!string.IsNullOrEmpty(returnUrl))
						{
							return Redirect(returnUrl);
						}
						else
						{
							return RedirectToAction("Index", "Home");
						}
					case AuthenticationStatus.Canceled:
						ViewData["Message"] = "Canceled at provider";
						return View("Login");
					case AuthenticationStatus.Failed:
						ViewData["Message"] = response.Exception.Message;
						return View("Login");
				}
			}
			return new EmptyResult();
		}

		//
		// GET: /Account/LogOff
		[NoCache]
		public ActionResult LogOff()
		{
			FormsAuthentication.SignOut();
			return RedirectToAction("Index", "Home");
		}
	}
}
