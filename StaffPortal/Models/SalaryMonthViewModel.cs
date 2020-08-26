using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using StaffPortal.Entities;

namespace StaffPortal.Models
{
    public class SalaryMonthViewModel
    {
        
        public List<Salary> Sals { get; set; }
        public SelectList Months { get; set; }
        public string SalaryMonth { get; set; }

        public SelectList Years { get; set; }
        public string SalaryYear { get; set; }
        public string SearchString { get; set; }
        
    }
}
