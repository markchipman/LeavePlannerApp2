using LeavePlannerApp2.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeavePlannerApp2.Models
{
    public class Seeder
    {
        private ApplicationDbContext _context;
        private UserManager<MyUserStore> _userManager;
        private RoleManager<MyUserRole> _roleManager;

        public Seeder(ApplicationDbContext context, UserManager<MyUserStore> userManager, RoleManager<MyUserRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task EmployeeSeeder()
        {
            _context.Database.EnsureCreated();
            var user = await _userManager.FindByEmailAsync("Admin@LeavePlanner.com");
            if (user == null)
            {
                user = new MyUserStore
                {
                    Email = "Admin@LeavePlanner.com",
                    UserName = "Admin@LeavePlanner.com"
                };

                

                var result = await _userManager.CreateAsync(user, "P@ssw0rd!");

                //Adding a role to the roles table
                if (!_roleManager.RoleExistsAsync(Roles.Admin).Result)
                {
                    var roles = new List<MyUserRole>()
                    {
                        new MyUserRole{Name = Roles.Admin },
                        new MyUserRole{Name = Roles.HR },
                        new MyUserRole{Name = Roles.Worker}
                    };
                    foreach (var role in roles)
                    {
                        await _roleManager.CreateAsync(role);
                    }

                }
                await _userManager.AddToRoleAsync(user, Roles.Admin);
                

                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Failed to create new Default User");
                }
                await _context.SaveChangesAsync();
            }
            
        }
    }
}
