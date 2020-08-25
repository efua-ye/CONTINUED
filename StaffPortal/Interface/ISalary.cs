using StaffPortal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffPortal.Interface
{
    public interface ISalary
    {
        void Add(Salary salary);
        Task<bool> AddAsync(Salary salary);
        Task<bool> Update(Salary salary);
        Task<IEnumerable<Salary>> GetAll();
        Task<Salary> GetById(int Id);
        Task<bool> Delete(int Id);

        /*
         Task<bool> Update(UserProfile userprofile, Grade grade);
        int GetIdByEmail(string email);
        Task<bool> UpdateGrade(Grade grade);
        Task<Grade> FindGradeById(int id);
        string FindNameByDepartmentId(int id);
        string FindFacultyNameByDepartmentId(int id);
         */
    }
}
