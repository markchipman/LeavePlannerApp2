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

namespace LeavePlannerApp2.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly IDept _dept;

        public DepartmentsController(IDept dept)
        {
            _dept = dept;
        }

        // GET: Departments
        public IActionResult Index()
        {
            return View( _dept.GetAllDepartments());
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
               var department = _dept.GetById(id);
                return View(department);
            }
        }

        // GET: Departments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("DepartmentId,Name")] Department department)
        {
            if (ModelState.IsValid)
            {
                var dept = _dept.Add(department);
                RedirectToAction("Index");
            }
            else
            {
                return View();
            }
            return View(department);
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (ModelState.IsValid)
            {
               var dept = _dept.GetById(id);
                return View(dept);

            }
            return View();
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("DepartmentId,Name")] Department department)
        {
            if (ModelState.IsValid)
            {
                var dept = _dept.Update(department);
                return RedirectToAction("Index");

            }
            else
            {
                return View();
            }

        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
           var dept = _dept.GetById(id);
            return View(dept);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
             _dept.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        //private bool DepartmentExists(int id)
        //{
        //    return _dept.Departments.Any(e => e.DepartmentId == id);
        //}
    }
}
