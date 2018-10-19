using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeavePlannerApp2.Models
{
    public class LeaveAllocationDaysRemaining
    {
        public int LeaveAllocationDaysRemainingId    { get; set; }
        public string Employee { get; set; }
        public int DaysRemaining { get; set; }
    }
}
