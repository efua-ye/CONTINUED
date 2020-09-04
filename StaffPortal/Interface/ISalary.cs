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
        Task<IEnumerable<Salary>> GetAllMonthlyReport(int id);
        Task<IEnumerable<Salary>> GetYear();
        Task<IEnumerable<Salary>> GetUserYear(int id);
        Task<Salary> GetById(int Id);
        Task<bool> Delete(int Id);
        Task<IEnumerable<Salary>> GetById(Salary sal);
        Task<IEnumerable<Salary>> GetUserMonth(int id);
        Task<bool> DeleteYear(int Id);
        //Task<IEnumerable<Salary>> GetYearReport(Salary sal);

        //Task<IQueryable<string Year, int UserProfileId>> GetYear();

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