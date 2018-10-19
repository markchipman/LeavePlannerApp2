using System;
using System.Collections.Generic;
using System.Text;
using LeavePlannerApp2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LeavePlannerApp2.Data
{
    public class ApplicationDbContext : IdentityDbContext<MyUserStore,MyUserRole,string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<LeaveApplication> LeaveApplications { get; set; }
        public DbSet<LeaveType> LeaveType { get; set; }
        public DbSet<LeaveAllocationDaysRemaining> DaysRemaining { get; set; }
        public DbSet<ApprovalANDRejection> ApprovalANDRejections { get; set; }
   
    }
}
