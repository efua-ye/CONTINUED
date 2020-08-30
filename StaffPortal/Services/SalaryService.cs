using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StaffPortal.Data;
using StaffPortal.Entities;
using Microsoft.EntityFrameworkCore;
using StaffPortal.Interface;
using System.Linq;

namespace StaffPortal.Services
{
    public class SalaryService : ISalary
    {
        private StaffPortalDataContext _context;
        public SalaryService(StaffPortalDataContext context)
        {
            _context = context;
        }

        public void Add(Salary salary) //Add
        {
            //if (salary.Equals(_context))
            //{

            //    Console.WriteLine("ERRRORRRR!!!!!!!!!!");            }
            //else
            
                _context.Add(salary);
                _context.SaveChanges();
            
           
        }
        public async Task<bool> AddAsync(Salary salary) //AddAsync
        {

            var existingGradeCount = _context.Salaries.Count(g => g.Month == salary.Month && g.Year == salary.Year && g.UserProfileId == salary.UserProfileId);

            if (existingGradeCount == 0)
            {

                try
                {

                    await _context.AddAsync(salary);

                    await _context.SaveChangesAsync();
                }

                catch (Exception)
                {
                    return false;
                }
                return true;

            }
            else
            {
               return false;
            }
            
                
          
           
            
        }

        public async Task<bool> Delete(int Id)//Delete
        {
            // find the entity/object
            var sal = await _context.Salaries.FindAsync(Id);

            if (sal != null)
            {
                _context.Salaries.Remove(sal);
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<Salary>> GetAll() //GetAll
        {
           // return await _context.Salaries.ToListAsync();

            return await _context.Salaries.Include(u => u.UserProfile).ToListAsync();
        }

        public async Task<Salary> GetById(int Id) //GetById
        {
            var sal = await _context.Salaries.FindAsync(Id);

            return sal;
        }

        public async Task<bool> Update(Salary salary) //Update
        {
            var sal = await _context.Salaries.FindAsync(salary.Id);
            if (sal != null)
            {
                sal.Month = salary.Month;
                sal.Year = salary.Year;

                await _context.SaveChangesAsync();
                return true;
            }

            return false;

        }


    }
}
