using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StaffPortal.Entities;

namespace StaffPortal.Interface
{
    public interface IUserProfile
    {
        void Add(UserProfile userprofile);
        Task<bool> AddAsync(UserProfile userprofile);
        Task<bool> Update(UserProfile userprofile, Grade grade);
        Task<bool> UpdateUser(UserProfile userprofile);
        //Task<bool> UpdateUser(UserProfile userprofile);
        Task<IEnumerable<UserProfile>> GetAll();
        int GetIdByEmail(string email);
        Task<bool> UpdateGrade(Grade grade);
        Task<Grade> FindGradeById(int id);
        string FindNameByDepartmentId(int id);
        string FindFacultyNameByDepartmentId(int id);
        Task<UserProfile> GetById(int Id);
        Task<bool> Delete(int Id);
    }
}
