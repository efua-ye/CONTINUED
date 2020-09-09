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

        public async Task<bool> DeleteYear(int Id)//Delete
        {
            // find the entity/object
            var sal = await _context.Salaries.FindAsync(Id);
            var salrows = _context.Salaries.Where(a => a.Year == sal.Year && a.UserProfileId == sal.UserProfileId);
            if (sal != null)
            {
                foreach (var row in salrows)
                {
                    _context.Salaries.Remove(row);
                }


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
        public async Task<IEnumerable<Salary>> GetAllMonthlyReport(int id) //GetAll
        {
            // return await _context.Salaries.ToListAsync();
            return await _context.Salaries.Include(u => u.UserProfile).Where(a => a.UserProfileId == id).ToListAsync();
        }


        //public async Task<IEnumerable<Salary>> GetUserMonths(int id) //GetAll
        //{
        //    return await _context.Salaries.Include(u => u.UserProfile).Where(a => a.UserProfileId == id).ToListAsync();
        //}

        public async Task<IEnumerable<Salary>> GetYearReport(Salary sal) //GetAll

   
        {
            // return await _context.Salaries.ToListAsync();
            var x = _context.Salaries.Include(u => u.UserProfile).Where(a => a.Year == sal.Year).Where(a => a.UserProfileId == sal.UserProfileId).ToListAsync();
            return await x;
        }

        public async Task<IEnumerable<Salary>> GetYear() //GetAll
        {

            // return await _context.Salaries.ToListAsync();
            var x = _context.Salaries.Include(u => u.UserProfile).GroupBy(b => new { b.Year, b.UserProfileId, b.UserProfile }).Select(c => new Salary
            {
                YearAllow = (c.Sum(p => p.UserProfile.Grade.TotAllowance)),
                Year = c.Key.Year,
                UserProfileId = c.Key.UserProfileId,
                UserProfile = c.Key.UserProfile,
                YearDeduction = (c.Sum(p => p.UserProfile.Grade.TotDeduction)),
                YearPay = (c.Sum(p => p.UserProfile.Grade.NetSalary)),
                Id = _context.Salaries.First(f => f.UserProfileId == c.Key.UserProfileId && f.Year == c.Key.Year).Id


            }
            );

            return await x.ToListAsync();



        }

        public async Task<IEnumerable<Salary>> GetUserYear(int id) //GetAll
        {
            // return await _context.Salaries.ToListAsync();
            var x = _context.Salaries.Include(u => u.UserProfile).GroupBy(b => new { b.Year, b.UserProfileId, b.UserProfile }).Select(c => new Salary
            {
                Year = c.Key.Year,
                UserProfileId = c.Key.UserProfileId,
                UserProfile = c.Key.UserProfile,
                YearAllow = (c.Sum(p => p.UserProfile.Grade.TotAllowance)),
                YearDeduction = (c.Sum(p => p.UserProfile.Grade.TotDeduction)),
                YearPay = (c.Sum(p => p.UserProfile.Grade.NetSalary)),
                Id = _context.Salaries.First(f => f.UserProfileId == c.Key.UserProfileId && f.Year == c.Key.Year).Id
            }
            ).Where(i => i.UserProfileId == id);
            return await x.ToListAsync();
        }
        public async Task<IEnumerable<Salary>> GetUserMonth(int id) //GetAll
        {
            var x = _context.Salaries.Include(u => u.UserProfile).GroupBy(b => new { b.Month, b.UserProfileId, /*b.UserProfile */}).Select(c => new Salary
            {
                Month = c.Key.Month,
                UserProfileId = c.Key.UserProfileId,
                //UserProfile = c.Key.UserProfile,
                Id = _context.Salaries.First(f => f.UserProfileId == c.Key.UserProfileId && f.Month == c.Key.Month).Id
            }
            ).Where(i => i.UserProfileId == id);
            return await x.ToListAsync();
            // return await _context.Salaries.ToListAsync();
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

        public async Task<IEnumerable<Salary>> GetById(Salary sal)
        {
            var x = _context.Salaries.Include(u => u.UserProfile).Where(a => a.Year == sal.Year).Where(a => a.UserProfileId == sal.UserProfileId).ToListAsync();
            return await x;
        }
        /*public async Task<IEnumerable<Salary>> GetUserMonth(int id) //GetAll
{
   var x = _context.Salaries.Include(u => u.UserProfile).GroupBy(b => new { b.Month, b.UserProfileId, /*b.UserProfile }).Select(c => new Salary
   {
       Month = c.Key.Month,
       UserProfileId = c.Key.UserProfileId,
       //UserProfile = c.Key.UserProfile,
       Id = _context.Salaries.First(f => f.UserProfileId == c.Key.UserProfileId && f.Month == c.Key.Month).Id
   }
   ).Where(i => i.UserProfileId == id);
   return await x.ToListAsync();
   // return await _context.Salaries.ToListAsync();
}
*/

    }
}



