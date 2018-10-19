using LeavePlannerApp2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeavePlannerApp2.ViewModel
{
    public class UserViewModel
    {

         public EmployeeVm Employee { get; set; }
    }

    public class EmployeeVm
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string EmployeeNumber { get; set; }
        public int TotalDaysEmployeeAppliedForLeave { get; set; }
        public Address Address { get; set; }
        public Department Department { get; set; }
        public int LeaveDaysTaken { get; set; }
        public int TotalLeaveDays { get; set; }
        public LeaveApplication hasRead { get; set; }
        public List<LeaveApplication> NotifyEmployee { get; set; }
    }
}
