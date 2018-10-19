using LeavePlannerApp2.Data;
using LeavePlannerApp2.Models.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeavePlannerApp2.Models.Repository
{
    public class LeaveRepo : ILeaveRepo
    {
        private ApplicationDbContext _context;
        private UserManager<MyUserStore> _userManager;

        public LeaveRepo(ApplicationDbContext context, UserManager<MyUserStore> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public LeaveApplication Apply(LeaveApplication leaveApplication, string user)
        {
            // var employee = new LeaveApplication();
            var applicationdate = DateTime.Now;


            var applicant = GetEmployeeByUserId(user);
            leaveApplication.Employee = applicant;
            leaveApplication.ApplicationDate = applicationdate;
            leaveApplication.IsApproved = null;
            _context.LeaveApplications.Add(leaveApplication);
            _context.SaveChanges();
            return leaveApplication;
        }

        public int GetTotalDaysEmloyeeAppliedForLeave(string employeeId)
        {
            var employeeRecord = GetEmployeeByUserId(employeeId);
            int totalLeaveDaysTake = 0;
            var applicants = _context.LeaveApplications.Where(x => x.Employee.Id == employeeRecord.Id).ToList();
            foreach (var applicant in applicants)
            {
                totalLeaveDaysTake += (applicant.DateTo - applicant.DateFrom).Days;
            }

            return totalLeaveDaysTake;
        }

        public List<LeaveApplication> ApprovedApplicants()
        {

            var approvedApplication = _context.LeaveApplications.Include("ApprovalANDRejection")
                                              .Where(x => x.IsApproved == true)
                                              .ToList();
            return approvedApplication;
        }

        public List<LeaveApplication> GetAllApplicants()
        {
            var rejected = _context.ApprovalANDRejections.Where(x => x.IsApproved == false).ToList();
            //var isApproved = _context.LeaveApplications.Include("ApprovalANDRejections").Where(x => x.GetApprovalANDRejection.IsApproved == true).ToList();
            return _context.LeaveApplications.Where(x => x.IsApproved == false).ToList();
        }

        public List<LeaveApplication> GetAllLeaveApplicants()
        {
            return _context.LeaveApplications.Where(x => x.IsApproved == null).ToList();
        }

        public List<LeaveApplication> GetAllApproved()
        {
            return _context.LeaveApplications.Where(x => x.IsApproved == true).ToList();
        }

        public List<LeaveApplication> RejectedApplicants()
        {
            var notApproved = _context.LeaveApplications
                                      .Where(x => x.IsApproved == false)
                                      .ToList();
            return notApproved;
        }

        public MyUserStore GetEmployeeByUserId(string userId)
        {
            var user = _context.Users.Where(x => x.Id == userId).FirstOrDefault();
            return user;
        }

        public List<LeaveApplication> GetLeaveApplicationHistory(string userId)
        {
            var leaveHistory = _context.LeaveApplications
                                       .Where(x => x.Employee.Id == userId && x.IsApproved == true)
                                       .ToList();
            return leaveHistory;
        }

    }
}
