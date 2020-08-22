using StaffPortal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffPortal.Interface
{
    public interface IGrade
    {
        void Add(Grade grade);
        Task<bool> AddAsync(Grade grade);
        Task<bool> Update(Grade grade);
        Task<IEnumerable<Grade>> GetAll();
        Task<Grade> GetById(int Id);
        Task<bool> Delete(int Id);

        IEnumerable<Grade> GetLevelsById(string gradename);
        IEnumerable<Grade> GetStepsById( int gradelevelid);
        /*
        Task<bool> Update(Salary dept);
        Task<IEnumerable<Salary>> GetAll();
        Task<Salary> GetById(int Id);
        int GetIdByEmail(string email);
        Task<bool> Delete(int Id);
     */
    }
}


