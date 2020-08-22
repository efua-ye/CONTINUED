using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StaffPortal.Entities;

namespace StaffPortal.Interface
{
    public interface IFaculty
    {
        void Add(Faculty faculty);
        Task<bool> AddAsync(Faculty faculty);
        Task<bool> Update(Faculty faculty);
        Task<IEnumerable<Faculty>> GetAll();
        Task<Faculty> GetById(int Id);
        Task<bool> Delete(int Id);
    }
}
