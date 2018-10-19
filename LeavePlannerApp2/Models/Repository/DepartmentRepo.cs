using LeavePlannerApp2.Data;
using LeavePlannerApp2.Models.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LeavePlannerApp2.Models.Repository
{
    public class DepartmentRepo : IDept
    {
        private ApplicationDbContext _context;

        public DepartmentRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public Models.Department Add(Models.Department dept)
        {
            _context.Departments.Add(dept);
            _context.SaveChanges();
            return dept;
        }

    
        public void Delete(int id)
        {
            var dept = GetById(id);
            _context.Departments.Remove(dept);
            _context.SaveChanges();
        }

        public List<Models.Department> GetAllDepartments()
        {
         var depts =  _context.Departments.ToList();
            _context.SaveChanges();
            return depts;
        }

        public Models.Department GetById(int deptId)
        {
           return _context.Departments.FirstOrDefault(x => x.DepartmentId == deptId);
        }

        public List<Models.Department> GetSome(Expression<Func<Models.Department, bool>> where)
        {
           return _context.Departments.Where(where).ToList();
        }

        public Models.Department Update(Models.Department department)
        {
          var dept = _context.Departments.Update(department);
            _context.SaveChanges();
            return department;
        }
        public Department SearchDepartment(string deptName)
        {
            var dept = _context.Departments.FirstOrDefault(x => x.Name == deptName);
            return dept;
        }

      
    }
}
