using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StaffPortal.Data;
using StaffPortal.Entities;
using StaffPortal.Interface;
using Microsoft.EntityFrameworkCore;

namespace StaffPortal.Services
{
    public class NewsService : INews
    {
        private StaffPortalDataContext _context;
        public NewsService(StaffPortalDataContext context)
        {
            _context = context;
        }

        public void Add(News news) //Add
        {
              _context.Add(news);
           
            _context.SaveChanges();
        }
        public async Task<bool> AddAsync(News news) //AddAsync
        {
            try
            {
                await _context.AddAsync(news);
               
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
            var news = await _context.NewsRoom.FindAsync(Id);

            if(news != null)
            {
                _context.NewsRoom.Remove(news);
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<News>> GetAll() //GetAll
        {

            return await _context.NewsRoom.ToListAsync();
        }

        public async Task<News> GetById(int Id) //GetById
        {
            var news = await _context.NewsRoom.FindAsync(Id);

            return news;
        }

        public async Task<bool> Update(News news) //Update
        {
            var fac = await _context.NewsRoom.FindAsync(news.ID);
            if(fac != null)
            {
                fac.Message = news.Message;
                //fac.Code = news.Code;

               await  _context.SaveChangesAsync();
                return true;
            }

            return false;

        }


    }
}
