using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StaffPortal.Data;
using StaffPortal.Entities;
using StaffPortal.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace StaffPortal.Services
{
    public class LeaveService : ILeave
    {
        private StaffPortalDataContext _context;
        public LeaveService(StaffPortalDataContext context)
        {
            _context = context;
        }

        public void Add(Leave leave) //Add
        {
              _context.Add(leave);
           
            _context.SaveChanges();
        }
        public async Task<bool> AddAsync(Leave leave) //AddAsync
        {
            //var existingCount = _context.Leaves.Count(g => g.Name == leave.Name && g.Code == leave.Code );
            var existingCount = _context.Leaves.Count(g => g.UserProfileId == leave.UserProfileId && g.StartDate == leave.StartDate && g.EndDate == leave.EndDate);
            if (existingCount == 0)
            {

                try
                {

                    await _context.AddAsync(leave);

                    await _context.SaveChangesAsync();
                    return true;
                }

                catch (Exception)
                {
                    return false;
                }
               

            }
            else
            {
                return false;
            }
        }

        public int GetBusinessDays(DateTime startD, DateTime endD)
        {
            double calcBusinessDays =  1 + ((endD - startD).TotalDays * 5 - (startD.DayOfWeek - endD.DayOfWeek) * 2) / 7;

            if (endD.DayOfWeek == DayOfWeek.Saturday) calcBusinessDays--;
            if (startD.DayOfWeek == DayOfWeek.Sunday) calcBusinessDays--;

            return Convert.ToInt32(calcBusinessDays);
        }

        public async Task<bool> Delete(int Id)//Delete
        {
            // find the entity/object
            var leave = await _context.Leaves.FindAsync(Id);

            if(leave != null)
            {
                _context.Leaves.Remove(leave);
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<Leave>> GetAll() //GetAll
        {

            return await _context.Leaves.Include(u => u.UserProfile).ToListAsync();
        }
        public async Task<IEnumerable<Leave>> GetPersonalAll(string email) //GetPersonalAll
        {

            return await _context.Leaves.Include(u => u.UserProfile).Where(u => u.UserProfile.Email == email).ToListAsync();
        }
       
        public async Task<Leave> GetById(int Id) //GetById
        {
            var leave = await _context.Leaves.FindAsync(Id);

            return leave;
        }

        public async Task<bool> Update(Leave leave) //Update
        {
            var leave_ = await _context.Leaves.FindAsync(leave.Id);
            if(leave_ != null)
            {
                //leave_.Name = leave.Name;
                //leave_.Code = leave.Code;

               await  _context.SaveChangesAsync();
                return true;
            }

            return false;

        }


    }
}
