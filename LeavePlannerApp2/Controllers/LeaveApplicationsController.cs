using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LeavePlannerApp2.Data;
using LeavePlannerApp2.Models;
using LeavePlannerApp2.Models.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace LeavePlannerApp2.Controllers
{
    public class LeaveApplicationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILeaveRepo _leaveRepo;
        private readonly UserManager<MyUserStore> _userManager;

        public LeaveApplicationsController(ApplicationDbContext context,ILeaveRepo leaveRepo, UserManager<MyUserStore> userManager)
        {
            _context = context;
            _leaveRepo = leaveRepo;
            _userManager = userManager;
        }

        // GET: LeaveApplications
        [Authorize(Roles = MyRoles.Admin)]
        public async Task<IActionResult> Index()
        {

            //this return all leave applicant that there 
            //leave has not been review ie neither reject nor approved
            var allLeaveApplicant = _leaveRepo.GetAllLeaveApplicants();
            return View(allLeaveApplicant);
        }

        // GET: LeaveApplications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.LeaveApplications
                .FirstOrDefaultAsync(m => m.LeaveApplicationId == id);
            if (leaveApplication == null)
            {
                return NotFound();
            }

            return View(leaveApplication);
        }

        // GET: LeaveApplications/Create    
        public IActionResult Create()
        {
            var leaveApplication = new LeaveApplication();
            var leaveType = _context.LeaveType.ToList();
            leaveApplication.LeaveType = leaveType;
            return View();
        }

        // POST: LeaveApplications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LeaveApplicationId,DateFrom,DateTo,ReasonForLeave,Responsibilities,LeaveType")] LeaveApplication leaveApplication)
        {
            if (ModelState.IsValid)
            {
                string user = _userManager.GetUserId(User);
                _leaveRepo.Apply(leaveApplication,user);
                return RedirectToAction("Dashboard", "Employees");
            }
            return RedirectToAction("Dashboard","Employees");
        }

        // GET: LeaveApplications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.LeaveApplications.FindAsync(id);
            if (leaveApplication == null)
            {
                return NotFound();
            }
            return View(leaveApplication);
        }

        // POST: LeaveApplications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LeaveApplicationId,DateFrom,DateTo,ReasonForLeave,Responsibilities,LeaveType")] LeaveApplication leaveApplication)
        {
            if (id != leaveApplication.LeaveApplicationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(leaveApplication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveApplicationExists(leaveApplication.LeaveApplicationId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(leaveApplication);
        }

        // GET: LeaveApplications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.LeaveApplications
                .FirstOrDefaultAsync(m => m.LeaveApplicationId == id);
            if (leaveApplication == null)
            {
                return NotFound();
            }

            return View(leaveApplication);
        }

        // POST: LeaveApplications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leaveApplication = await _context.LeaveApplications.FindAsync(id);
            _context.LeaveApplications.Remove(leaveApplication);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

   
        [HttpPost]
        public IActionResult Approved(int id)
        {
            var emp = _context.LeaveApplications.FirstOrDefault(x => x.LeaveApplicationId == id && x.IsApproved == null);
            if (emp != null)
            {
                var remainingdays = new LeaveAllocationDaysRemaining()
                {
                    Employee = _userManager.GetUserId(User),
                    DaysRemaining = (emp.DateTo - emp.DateFrom).Days,

                };
                _context.DaysRemaining.Add(remainingdays);

                var rejectionOrApproval = new ApprovalANDRejection() {
                    LeaveApplicationId = emp.LeaveApplicationId,
                    IsApproved = true,
                    LeaveAllocationDaysRemainingId = remainingdays.LeaveAllocationDaysRemainingId
                    
                };

                emp.IsApproved = true;
                emp.Message = "Leave Have Been Approved";

             
              _context.LeaveApplications.Update(emp);
                _context.ApprovalANDRejections.Add(rejectionOrApproval);
                
                _context.SaveChanges();
                ViewBag.Leave = "Leave Has been Approves Successfully";
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpPost]
        public IActionResult Rejected(int id)
        {
            var emp = _context.LeaveApplications
                              .FirstOrDefault(x => x.LeaveApplicationId == id && x.IsApproved == null);
            var remainingdays = new LeaveAllocationDaysRemaining()
            {
                Employee = _userManager.GetUserId(User),
                DaysRemaining = (emp.DateTo - emp.DateFrom).Days,

            };
            emp.IsApproved = false;
            emp.Message = "Leave Have Been Approved";
            _context.LeaveApplications.Update(emp);
            _context.DaysRemaining.Add(remainingdays);
            var rejectionOrApproval = new ApprovalANDRejection()
            {
                LeaveApplicationId = emp.LeaveApplicationId,
                IsApproved = false,
                LeaveAllocationDaysRemainingId = remainingdays.LeaveAllocationDaysRemainingId
            };

            _context.ApprovalANDRejections.Add(rejectionOrApproval);

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
  
        private bool LeaveApplicationExists(int id)
        {
            return _context.LeaveApplications.Any(e => e.LeaveApplicationId == id);
        }

    }
}
