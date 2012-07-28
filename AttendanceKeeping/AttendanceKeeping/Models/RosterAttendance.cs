using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AttendanceKeeping.Models
{
	public class RosterAttendance
	{
		public int RosterAttendanceId { get; set; }
		public int ClassRosterId { get; set; }
		[ForeignKey("ClassRosterId")]
		public ClassRoster ClassRoster { get; set; }
		public DateTime ClassDate { get; set; }
		public override string ToString()
		{
			return "WASSUP";
		}
	}
}