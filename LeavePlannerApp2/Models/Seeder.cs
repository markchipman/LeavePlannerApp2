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
                if (!_roleManager.RoleExistsAsync(MyRoles.Admin).Result)
                {
                    var roles = new List<MyUserRole>()
                    {
                        new MyUserRole{Name = MyRoles.Admin },
                        new MyUserRole{Name = MyRoles.HR },
                        new MyUserRole{Name = MyRoles.Worker}
                    };
                    foreach (var role in roles)
                    {
                        await _roleManager.CreateAsync(role);
                    }

                }
                await _userManager.AddToRoleAsync(user, MyRoles.Admin);
                

                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Failed to create new Default User");
                }
                await _context.SaveChangesAsync();
            }
            
        }
    }
}
