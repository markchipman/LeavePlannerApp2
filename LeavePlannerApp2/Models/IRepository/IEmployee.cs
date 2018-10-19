using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LeavePlannerApp2.Models.IRepository
{
    public interface IEmployee
    {
        Employee Add(Employee emp);
        Employee GetById(int empId);
        Employee Update(Employee emp);
        List<Employee> GetAllEmployees();
        List<Employee> GetSome(Expression<Func<Employee, bool>> where);
        void Delete(int id);
        Employee GetByEmployeeNumber(string empNo);
        Employee GetEmployee(string userid);
    }
}
