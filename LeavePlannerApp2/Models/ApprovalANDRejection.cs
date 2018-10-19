using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeavePlannerApp2.Models
{
    public class ApprovalANDRejection
    {
        public int ApprovalANdRejectionId { get; set; }
        public int LeaveApplicationId { get; set; }
        public bool IsApproved { get; set; }
        public LeaveApplication LeaveApplication { get; set; }
        public int LeaveAllocationDaysRemainingId { get; set; }
        public LeaveAllocationDaysRemaining LeaveAllocationDaysRemaining { get; set; }

    }
}