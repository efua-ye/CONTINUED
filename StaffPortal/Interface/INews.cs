using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StaffPortal.Entities;

namespace StaffPortal.Interface
{
    public interface INews
    {
        void Add(News news);
        Task<bool> AddAsync(News news);
        Task<bool> Update(News news);
        Task<IEnumerable<News>> GetAll();
        Task<News> GetById(int Id);
        Task<bool> Delete(int Id);
    }
}
