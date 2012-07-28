using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttendanceKeeping.Models;

namespace AttendanceKeeping.Tests.Utilities
{
	public static class HelperUtilities
	{
		public static void ClearDataBase()
		{
			using(var context = new AttendanceKeepingContext())
			{
				var people = context.Persons.ToList();
				foreach(var p in people)
				{
					context.Persons.Remove(p);
				}
				context.SaveChanges();
			}
		}

		public static DisposableDatabaseWrapper CreateNewPerson()
		{
			var person = new Person
				{
					BirthDate = new DateTime(1982, 12, 9),
					FirstName = Guid.NewGuid().ToString(),
					LastName = Guid.NewGuid().ToString()
				};
			var attendanceContext = new AttendanceKeepingContext();
			attendanceContext.Persons.Add(person);
			attendanceContext.SaveChanges();

			var disposable = new DisposableDatabaseWrapper();
			disposable.ExecuteWhenDispose = () =>
				{
					attendanceContext.Persons.Remove(person);
					attendanceContext.SaveChanges();
					attendanceContext.Dispose();
				};
			disposable.Id = person.PersonId;
			disposable.ResultingObject = person;
			return disposable;
		}
	}


	public class DisposableDatabaseWrapper : IDisposable
	{
		public Action ExecuteWhenDispose { get; set; }

		public int Id { get; set; }
		public string StringId { get; set; }
		public object ResultingObject { get; set; }

		public void Dispose()
		{
			if (ExecuteWhenDispose != null)
			{
				ExecuteWhenDispose();
			}
		}
	}
}
