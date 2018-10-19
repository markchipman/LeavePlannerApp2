using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeavePlannerApp2.Models.IRepository
{
    public interface ILeaveRepo
    {
        LeaveApplication Apply(LeaveApplication leaveApplication, string user);
        List<LeaveApplication> GetAllApplicants();
        List<LeaveApplication> ApprovedApplicants();
        List<LeaveApplication> RejectedApplicants();
        List<LeaveApplication> GetLeaveApplicationHistory(string userId);
        List<LeaveApplication> GetAllLeaveApplicants();
        int GetTotalDaysEmloyeeAppliedForLeave(string employeeId);
    }
}
