using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LeavePlannerApp2.Models.IRepository
{
    public interface IDept
    {
        Department Add(Department dept);
        Department GetById(int deptId);
        Department Update(Department department);
        List<Department> GetAllDepartments();
        List<Department> GetSome(Expression<Func<Department, bool>> where);
        void Delete(int id);
        Department SearchDepartment(string deptName);
    }
}
