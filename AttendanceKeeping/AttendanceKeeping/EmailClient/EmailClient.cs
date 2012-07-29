using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using AttendanceKeeping.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttendanceKeeping.Models;

namespace AttendanceKeeping.EmailClient
{
    public class EmailClient
    {
        private static readonly Regex EmailRegex = new Regex(@"^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$", RegexOptions.Compiled);
        private static Tuple<DateTime, DateTime> ReturnBeginEndMonth(DateTime? date = null)
        {
            if (date == null)
            {
                date = DateTime.Now;
            }
            var beginDate = new DateTime(date.Value.Year, date.Value.Month, 1);
            var endDate = beginDate.AddMonths(1).AddDays(-1);
            return new Tuple<DateTime, DateTime>(beginDate, endDate);
        }

        private static string ConstructEmailBody(Tuple<DateTime, DateTime> monthPeriod)
        {
            using (var context = new AttendanceKeepingContext())
            {
                var rostersFromPerdiod = context.RosterAttendances
                    .Include(r => r.ClassRoster)
                    .Include(r => r.ClassRoster.ClassSchedule)
                    .Include(r => r.ClassRoster.Person)
                    .Where(t => t.ClassDate >= monthPeriod.Item1 && t.ClassDate <= monthPeriod.Item2)
                    .ToList();
                var body = new StringBuilder();
                body.AppendFormat("Attendance Figures For {0}-{1} : {3}{2}", 
                    monthPeriod.Item1.ToShortDateString(), 
                    monthPeriod.Item2.ToShortDateString(), 
                    Environment.NewLine,
                    rostersFromPerdiod.Count);

                body.AppendLine();
                foreach (var roster in rostersFromPerdiod.GroupBy(t => t.ClassDate.ToShortDateString()))
                {
                    const string dateTemplate = "Number of attendees for {0}: {1}{2}";
                    body.AppendFormat(dateTemplate, roster.Key, roster.Count(), Environment.NewLine);
                    foreach (var item in roster)
                    {
                        const string lineTemplate = "{0}{3} ~ {1}, {2}{4}";
                        body.AppendFormat(lineTemplate, "\t", item.ClassRoster.Person.LastName,
                                          item.ClassRoster.Person.FirstName,
                                          item.ClassRoster.ClassSchedule.ClassName,
                                          Environment.NewLine);
                    }
                    body.AppendLine();
                }
                return body.ToString();
            }
        }

        public static void SendAttendanceEmail()
        {
            using (var context = new AttendanceKeepingContext())
            {
                var emailAddresses = context.Persons.Where(t => t.Email != null).ToList();
                
                foreach (var emailAddress in emailAddresses.Where(t => EmailRegex.IsMatch(t.Email)))
                {
                    var fromAddress = new MailAddress("roylance.michael@gmail.com", "Michael Roylance");
                    var fromPassword = ConfigurationManager.AppSettings["gmailPwd"];
                    var toAddress = new MailAddress(emailAddress.Email, string.Format("{0} {1}", emailAddress.FirstName, emailAddress.LastName));
                    var monthPeriod = ReturnBeginEndMonth();
                    var subject = string.Format("Attendance Figures For {0}-{1}", monthPeriod.Item1.ToShortDateString(), monthPeriod.Item2.ToShortDateString());
                    var body = ConstructEmailBody(monthPeriod);

                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                    };
                    using (var message = new MailMessage(fromAddress, toAddress)
                    {
                        Subject = subject,
                        Body = body
                    })
                    {
                        smtp.Send(message);
                    }
                }
            }
        }
    }
}