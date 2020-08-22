using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StaffPortal.Data;
using StaffPortal.Entities;
using Microsoft.EntityFrameworkCore;
using StaffPortal.Interface;

namespace StaffPortal.Services
{
    public class DepartmentService : IDepartment
    {
        private StaffPortalDataContext _context;
        public DepartmentService(StaffPortalDataContext context)
        {
            _context = context;
        }

        public void Add(Department department) //Add
        {
            _context.Add(department);

            _context.SaveChanges();
        }
        public async Task<bool> AddAsync(Department department) //AddAsync
        {
            try
            {
                
                await _context.AddAsync(department);

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> Delete(int Id)//Delete
        {
            // find the entity/object
            var dept = await _context.Departments.FindAsync(Id);

            if (dept != null)
            {
                _context.Departments.Remove(dept);
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<Department>> GetAll() //GetAll
        {

            return await _context.Departments.Include(f => f.Faculty).ToListAsync();
        }

        public async Task<Department> GetById(int Id) //GetById
        {
            var dept = await _context.Departments.FindAsync(Id);

            return dept;
        }

        public async Task<bool> Update(Department department) //Update
        {
            var dept = await _context.Departments.FindAsync(department.Id);
            if (dept != null)
            {
                dept.DeptName = department.DeptName;
                dept.DeptCode = department.DeptCode;

                await _context.SaveChangesAsync();
                return true;
            }

            return false;

        }


    }
}
