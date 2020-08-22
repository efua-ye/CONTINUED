using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StaffPortal.Data;
using StaffPortal.Entities;
using StaffPortal.Interface;
using Microsoft.EntityFrameworkCore;

namespace StaffPortal.Services
{
    public class FacultyService : IFaculty
    {
        private StaffPortalDataContext _context;
        public FacultyService(StaffPortalDataContext context)
        {
            _context = context;
        }

        public void Add(Faculty faculty) //Add
        {
              _context.Add(faculty);
           
            _context.SaveChanges();
        }
        public async Task<bool> AddAsync(Faculty faculty) //AddAsync
        {
            try
            {
                await _context.AddAsync(faculty);
               
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
            var faculty = await _context.Faculties.FindAsync(Id);

            if(faculty != null)
            {
                _context.Faculties.Remove(faculty);
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<Faculty>> GetAll() //GetAll
        {

            return await _context.Faculties.ToListAsync();
        }

        public async Task<Faculty> GetById(int Id) //GetById
        {
            var faculty = await _context.Faculties.FindAsync(Id);

            return faculty;
        }

        public async Task<bool> Update(Faculty faculty) //Update
        {
            var fac = await _context.Faculties.FindAsync(faculty.Id);
            if(fac != null)
            {
                fac.Name = faculty.Name;
                fac.Code = faculty.Code;

               await  _context.SaveChangesAsync();
                return true;
            }

            return false;

        }


    }
}
