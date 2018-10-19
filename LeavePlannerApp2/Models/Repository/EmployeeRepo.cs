using LeavePlannerApp2.Data;
using LeavePlannerApp2.Models.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LeavePlannerApp2.Models.Repository
{
    public class EmployeeRepo : IEmployee
    {
        private ApplicationDbContext _context;

        public EmployeeRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public EmployeeRepo()
        {

        }
        public Employee Add(Employee emp)
        {
            var employee = _context.Employees.Add(emp);
            _context.SaveChanges();
            return employee.Entity;
        }

        public void Delete(int id)
        {
           var employee =  _context.Employees.FirstOrDefault(x => x.EmployeeId == id);
            _context.Employees.Remove(employee);
            _context.SaveChanges();
        }

        public List<Employee> GetAllEmployees()
        {
            var employees = _context.Employees.ToList();
            return employees;
        }

        public Employee GetById(int id)
        {
            var employee = _context.Employees.FirstOrDefault(x => x.EmployeeId == id);
            return employee;
        }

        public List<Employee> GetSome(Expression<Func<Employee, bool>> where)
        {
            var employees = _context.Employees.Where(where).ToList();
            return employees;
        }

        public Employee Update(Employee employee)
        {
            var updateEmployee = _context.Employees.Update(employee);
            _context.SaveChanges();
            return updateEmployee.Entity;
        }

        //this method needs to check for possible clashes of Employee
        //Number number in the database which is not factored now
        public  string GenerateEmployeeNumber(string deptName)
        {
            //var dept = _context.Departments.Find(deptName);
            var random = new Random();
         
                          
                int result = random.Next();
                var employeeNumber = $" {deptName}-{result}";
       
           // var number = random.Next();
            return $"{employeeNumber}";
        }
        public Employee GetByEmployeeNumber(string empNo)
        {
            var employee = _context.Employees.FirstOrDefault(x => x.EmployeeNumber == empNo);
            return employee;
        }

        public Employee GetEmployee(string userId)
        {
            var userstore = _context.Users.Where(x => x.Id == userId).FirstOrDefault();
            var employee = _context.Employees.Include("Department").Where(x => x.EmployeeNumber == userstore.EmployeeNumber).FirstOrDefault();
            return employee;
        }
    }
}
