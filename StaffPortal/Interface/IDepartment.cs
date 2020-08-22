using StaffPortal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffPortal.Interface
{
    public interface IDepartment
    {
        void Add(Department department);
        Task<bool> AddAsync(Department department);
        Task<bool> Update(Department department);
        Task<IEnumerable<Department>> GetAll();
        Task<Department> GetById(int Id);
        Task<bool> Delete(int Id);
    }
}
