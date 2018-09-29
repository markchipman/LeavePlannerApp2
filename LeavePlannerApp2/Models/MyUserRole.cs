using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeavePlannerApp2.Models
{
    public class MyUserRole : IdentityRole
    {
        public string Description { get; set; }
    }
}
