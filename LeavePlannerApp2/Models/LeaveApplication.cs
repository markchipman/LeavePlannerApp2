using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LeavePlannerApp2.Models
{
    public class LeaveApplication
    {
       
        public int LeaveApplicationId { get; set; }
        public DateTimeOffset  DateFrom { get; set; }
        public DateTimeOffset DateTo { get; set; }
        public DateTimeOffset ApplicationDate { get; set; }
        [Required]
        public string ReasonForLeave { get; set; }
        [Required]
        public string Responsibilities { get; set; }
        public bool? IsApproved { get; set; }
        public string Message { get; set; }
        public List<LeaveType> LeaveType { get; set; } 
        public int LeaveDays { get; set; }
        public MyUserStore Employee { get;set; }
        public bool hasread { get; set; }
        public List<ApprovalANDRejection> GetApprovalANDRejection { get; set; }
        [NotMapped]
        public int GenerateDays {
            get {
                 var days = DateTo.Day -  DateFrom.Day;
                 return days;
                }
            set
            {
                LeaveDays = value;
            }
        }
    }
}
