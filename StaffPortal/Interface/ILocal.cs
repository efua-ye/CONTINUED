using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StaffPortal.Entities;

namespace StaffPortal.Interface
{
    public interface ILocal
    {
        
        IEnumerable<LGA> GetLGAsById(int id);
        
    }
}
