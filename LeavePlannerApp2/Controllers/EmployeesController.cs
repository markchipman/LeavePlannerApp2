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
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using LeavePlannerApp2.Models.Services;
using LeavePlannerApp2.Models.Repository;
using Microsoft.AspNetCore.Identity;
using LeavePlannerApp2.ViewModel;

namespace LeavePlannerApp2.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<MyUserStore> _userManager;
        private readonly ILeaveRepo _leaveRepo;
        private IEmployee _employee;
        private IDept _dept;

        public EmployeesController(IEmployee employee , 
                                    IDept dept, ApplicationDbContext context, 
                                    UserManager<MyUserStore> userManager,
                                    ILeaveRepo leaveRepo)
        {
            _employee = employee;
            _dept = dept;
            _context = context;
            _userManager = userManager;
            _leaveRepo = leaveRepo;
        }

        public IActionResult Dashboard()
        {
            var user = _userManager.GetUserId(User);
            int daysTaken = 0;
            int days = 0;
            var approvedLeave = _context.LeaveApplications.Where(x => x.Employee.Id == user && x.IsApproved == true).ToList();
            var notifyEmployee = _context.LeaveApplications.Where(x => x.Employee.Id == user && ( x.IsApproved == true || x.IsApproved == false)).ToList();
            var notifyHasRead = _context.LeaveApplications.Where(x => x.Employee.Id == user && x.hasread == true).FirstOrDefault();
            var totalLeaveDaysTaken = _leaveRepo.GetTotalDaysEmloyeeAppliedForLeave(user);
            var leaveDays = _context.LeaveType.ToList();
            foreach (var item in leaveDays)
            {
                days += item.LeaveTypeDays;
            }
            foreach (var dayCount in approvedLeave)
            {
                daysTaken += GetNumberOfWorkingDaysJeroen(dayCount.DateFrom,dayCount.DateTo);
            }
            var employee = new EmployeeVm();

            var emp = _employee.GetEmployee(user);
            employee.FirstName = emp.FirstName; 
            employee.LastName = emp.LastName;
            employee.MobileNumber = emp.MobileNumber;
            employee.EmployeeNumber = emp.EmployeeNumber;
            employee.Department = emp.Department;
            employee.Address = emp.Address;
            employee.LeaveDaysTaken = daysTaken;
            employee.TotalLeaveDays = days;
            employee.NotifyEmployee = notifyEmployee;
            employee.hasRead = notifyHasRead;
            employee.TotalDaysEmployeeAppliedForLeave = totalLeaveDaysTaken;
           return View(employee);
        }
        private static int GetNumberOfWorkingDaysJeroen(DateTimeOffset start, DateTimeOffset stop)
        {
            int days = 0;
            while (start <= stop)
            {
                if (start.DayOfWeek != DayOfWeek.Saturday && start.DayOfWeek != DayOfWeek.Sunday)
                {
                    ++days;
                }
                start = start.AddDays(1);
            }
            return days;
        }

        public IActionResult ConfirmHasReadMessage(string empId)
        {
            var user = _userManager.GetUserId(User);
            var hasRead = _context.LeaveApplications.Where(x => x.Employee.Id == user).ToList();
            if (hasRead != null)
            {
                foreach (var item in hasRead)
                {
                    item.hasread = true;
                }
                _context.LeaveApplications.UpdateRange(hasRead);
                _context.SaveChanges();
            }
            
            return View();
        }
        
        public IActionResult GetLeaveHistory()
        {
            var user = _userManager.GetUserId(User);
            var leaveHistory = _leaveRepo.GetLeaveApplicationHistory(user);
            return View(leaveHistory);
        }

        // GET: Employees
        public IActionResult Index()
        {
            var allemployee = _employee.GetAllEmployees();
            return View(allemployee);
        }

        // GET: Employees/Details/5
        public IActionResult Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = _employee.GetById(id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeId,FirstName,LastName,MobileNumber")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _employee.Add(employee);
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = _employee.GetById(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,FirstName,LastName,MobileNumber")] Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _employee.Update(employee);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeId))
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
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = _employee.GetById(id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
             _employee.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }

        public IActionResult UploadEmployees()
        {
            return View();
        }
        public async Task<IActionResult> UploadEmployeeRecords(IFormFile excelfile)
        {
            if (excelfile == null || excelfile.Length == 0)
            {
                ViewBag.Error = "Please Select a excel file <br/>";
                TempData["UserMessage"] = "Please Select a excel file.";
                TempData["Title"] = "Error.";

                return View(nameof(Index));
            }
            IFormFile file = Request.Form.Files["excelfile"];

            if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
            {
                string lastrecord = "";
                int recordCount = 0;
                string message = "";
                string fileContentType = file.ContentType;
                byte[] fileBytes = new byte[file.Length];
                var data = file.OpenReadStream();


                using (var package = new ExcelPackage(file.OpenReadStream()))
                {
                    ExcelValidation myExcel = new ExcelValidation();
                    var currentSheet = package.Workbook.Worksheets;
                    var workSheet = currentSheet.First();
                    var noOfCol = workSheet.Dimension.End.Column;
                    var noOfRow = workSheet.Dimension.End.Row;
                    int requiredField = 4;


                    string validCheck = myExcel.ValidateExcel(noOfRow, workSheet, requiredField);
                    if (!validCheck.Equals("Success"))
                    {
                        //string row = "";
                        //string column = "";
                        string[] ssizes = validCheck.Split(' ');

                        string lineError = $"Line/Row number {ssizes[0]}  and column {ssizes[1]} is not rightly formatted, Please Check for anomalies ";
                        //ViewBag.LineError = lineError;
                        ViewBag.Message = lineError;
                        RedirectToAction(nameof(Create));
                    }

                    var studentId = DateTime.Now.Ticks;
                    for (int row = 2; row <= noOfRow; row++)
                    {
                        string firstName = workSheet.Cells[row, 1].Value.ToString().Trim();
                        string lastName = workSheet.Cells[row, 2].Value.ToString().Trim();
                        string mobileNumber = workSheet.Cells[row, 3].Value.ToString().Trim();
                        string department = workSheet.Cells[row, 4].Value.ToString().Trim();

                        var dept = _dept?.SearchDepartment(department);

                        var generateNumber = new EmployeeRepo();

                        try
                        {
                            var employee = new Employee()
                            {
                                FirstName = firstName,
                                LastName = lastName,
                                MobileNumber = mobileNumber,
                                Department = dept,
                                EmployeeNumber = generateNumber.GenerateEmployeeNumber(dept.Name)
                            };

                            _context.Employees.Add(employee);
                            recordCount++;
                           // lastrecord = $"The last Updated record has the Last Name {student.LastName} and First Name {student.FirstName} with Student Id {student.StudentId}";
                        }
                        catch (Exception ex)
                        {
                            ViewBag.ErrorInfo = "Please Leave no column or row Empty/Blank";
                            ViewBag.ErrorMessage = ex.Message;
                            return View("ErrorException");
                        }
                    }
                    await _context.SaveChangesAsync();
                    message = $"You have successfully Uploaded {recordCount} records...  and {lastrecord}";
                    TempData["UserMessage"] = message;
                    TempData["Title"] = "Success.";
                }
                return RedirectToAction("Index", "Employees");

            }
            return View();
        }

        public IActionResult GetEmployeeRecord()
        {
           
            return View();
        }
        [HttpPost]
        public IActionResult GetEmployeeRecord(string empNumber)
        {
            var employee = _employee.GetByEmployeeNumber(empNumber);
            return RedirectToActionPreserveMethod("Register","Identity/Account",employee);
        }

        public IActionResult ApplyForLeave()
        {
            var user = _userManager.GetUserId(User);
            var totalLeaveDaysTaken = _leaveRepo.GetTotalDaysEmloyeeAppliedForLeave(user);

            var leaveApplication = new LeaveApplicationVm();
            var leaveType = _context.LeaveType.ToList();
            leaveApplication.TotalLeaveDaystaken = totalLeaveDaysTaken;
            leaveApplication.LeaveType = leaveType;
            return View(leaveApplication);
        }
    }
}
