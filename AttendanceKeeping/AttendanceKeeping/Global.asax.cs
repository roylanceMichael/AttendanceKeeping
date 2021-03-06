﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AttendanceKeeping.Models;

namespace AttendanceKeeping
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            var timer = new Timer(s =>
                {
                    var lastDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1).Day;
                    if(DateTime.Now.Day == lastDay)
                    {
                        EmailClient.EmailClient.SendAttendanceEmail();
                    }
                }, 
                null, 0, 86400000);
            
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //make sure I exist
            using (var context = new AttendanceKeepingContext())
            {
                if (!context.Persons.Any(t => string.Equals(t.Email, "roylance.michael@gmail.com")))
                {
                    var mike = new Person
                        {
                            LastName = "Roylance",
                            FirstName = "Mike",
                            BirthDate = new DateTime(1982, 12, 9),
                            Email = "roylance.michael@gmail.com"
                        };
                    context.Persons.Add(mike);
                    context.SaveChanges();
                }
            }
        }
    }
}