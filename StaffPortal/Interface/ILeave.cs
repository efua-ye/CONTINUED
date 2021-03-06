﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StaffPortal.Entities;

namespace StaffPortal.Interface
{
    public interface ILeave
    {
        void Add(Leave leave);
        Task<bool> AddAsync(Leave leave);
        int GetBusinessDays(DateTime startD, DateTime endD);
        Task<bool> Update(Leave leave);
        Task<IEnumerable<Leave>> GetAll();
        Task<IEnumerable<Leave>> GetPersonalAll(string email);
        Task<Leave> GetById(int Id);
        Task<bool> Delete(int Id);
    }
}
