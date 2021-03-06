﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LeavePlannerApp2.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LeavePlannerApp2.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using LeavePlannerApp2.Areas.Identity.Services;
using LeavePlannerApp2.Models.IRepository;
using LeavePlannerApp2.Models.Repository;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace LeavePlannerApp2
{
    public class Startup
    {
      
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddSingleton<IFileProvider>(
               new PhysicalFileProvider(
                   Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));


            //services.AddDefaultIdentity<IdentityUser>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentity<MyUserStore, MyUserRole>(cfg => {
                cfg.User.RequireUniqueEmail = true;
                cfg.SignIn.RequireConfirmedEmail = true;
               
            }).AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddTransient<Seeder>();
            //services.AddTransient<IEmailSender, EmailSender>(i =>
            //    new EmailSender(
            //        Configuration["EmailSender:Host"],
            //        Configuration.GetValue<int>("EmailSender:Port"),
            //        Configuration.GetValue<bool>("EmailSender:EnableSSL"),
            //        Configuration["EmailSender:UserName"],
            //        Configuration["EmailSender:Password"]
            //    )
            //);
            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddTransient<IDept, DepartmentRepo>();
            services.AddTransient<IEmployee, EmployeeRepo>();
            services.AddTransient<ILeaveRepo, LeaveRepo>();
            services.Configure<AuthMessageSenderOptions>(Configuration);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        private void DepartmentRepo()
        {
            throw new NotImplementedException();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                //seeding the database 
                if (env.IsDevelopment())
                {
                    using (var scope = app.ApplicationServices.CreateScope())
                    {
                        var seeder = scope.ServiceProvider.GetService<Seeder>();
                            seeder.EmployeeSeeder().Wait(); 
                    }
                }
                
            });
        }
    }
}
