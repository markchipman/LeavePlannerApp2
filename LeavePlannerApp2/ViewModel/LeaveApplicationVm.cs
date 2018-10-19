using LeavePlannerApp2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LeavePlannerApp2.ViewModel
{
    public class LeaveApplicationVm
    {
        public DateTimeOffset DateFrom { get; set; }
        public DateTimeOffset DateTo { get; set; }
        [Required]
        public string ReasonForLeave { get; set; }
        [Required]
        public string Responsibilities { get; set; }
        public List<LeaveType> LeaveType { get; set; }
        public int TotalLeaveDaystaken { get; set; }
       
    }
}
