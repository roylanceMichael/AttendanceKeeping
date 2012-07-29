using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendanceKeeping.Models
{
    public class RosterAttendanceViewModel
    {
        public Dictionary<string, List<RosterAttendance>> SortedRosterAttendances
        {
            get
            {
                if(RosterAttendances != null)
                {
                    var newDic = new Dictionary<string, List<RosterAttendance>>();
                    var keys = RosterAttendances.GroupBy(t => t.ClassDate.ToShortDateString());
                    foreach(var key in keys)
                    {
                        newDic[key.Key] = key.ToList();
                    }
                    return newDic;
                }
                return new Dictionary<string, List<RosterAttendance>>();
            }
        }

        public List<RosterAttendance> RosterAttendances { get; set; }  
    }
}