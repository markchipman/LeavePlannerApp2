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
    }
}
