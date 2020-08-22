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
    public class LGAService : ILocal
    {
        private StaffPortalDataContext _context;
        public LGAService(StaffPortalDataContext context)
        {
            _context = context;
        }



        public IEnumerable<LGA> GetLGAsById(int id) //GetAll
        {

            var  list =  _context.LGAs.Where(u => u.NewState.Id == id).ToList();
            //list.Insert(0, new LGA { Id = 0, Name = "Select Local Government" });
            return list;
        }

      

    }
}
