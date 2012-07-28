using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendanceKeeping.Models
{
	public class ClassSchedule
	{
		public int ClassScheduleId { get; set; }
		public string ClassName { get; set; }

		public virtual ICollection<ClassRoster> ClassRosters { get; set; }
	}
}