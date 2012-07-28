using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttendanceKeeping.Models;
using AttendanceKeeping.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AttendanceKeeping.Tests.Models
{
	[TestClass]
	public class AttendanceKeepingContextTests
	{
		[TestMethod]
		public void SuccessWhenCtorInitiated()
		{
			//arrange
			//act
			using (var disposablePerson = HelperUtilities.CreateNewPerson())
			using(var atc = new AttendanceKeepingContext())
			{
				//assert
				Assert.IsTrue(atc.Persons.Any(t => t.PersonId == disposablePerson.Id));
			}
		}
	}
}
