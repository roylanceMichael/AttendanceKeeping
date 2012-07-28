using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AttendanceKeeping.Models
{
	public class AttendanceKeepingContext : DbContext
	{
		public DbSet<Person> Persons { get; set; }
		public DbSet<ClassSchedule> ClassSchedules { get; set; }
		public DbSet<ClassRoster> ClassRosters { get; set; }
		public DbSet<RosterAttendance> RosterAttendances { get; set; }

		public static bool AuthorizeEmail(string email)
		{
			using(var context = new AttendanceKeepingContext())
			{
				return context.Persons.Any(t => string.Equals(t.Email, email));
			}
		}
	}
}