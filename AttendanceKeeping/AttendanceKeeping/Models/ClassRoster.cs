using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AttendanceKeeping.Models
{
	public class ClassRoster
	{
		public int ClassRosterId { get; set; }

		public int ClassScheduleId { get; set; }

		[ForeignKey("ClassScheduleId")]
		public ClassSchedule ClassSchedule { get; set; }

		public int PersonId { get; set; }

		[ForeignKey("PersonId")]
		public Person Person { get; set; }
	}
}