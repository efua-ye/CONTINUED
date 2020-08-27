using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffPortal.Entities
{
    public class Salary
    {
        public int Id { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public UserProfile UserProfile { get; set; }
        public int UserProfileId { get; set; }
        public string CreatedBy { get; set; }
        private DateTime? dateCreated = null;
        public DateTime DateCreated
        {
            get
            {
                return dateCreated ?? DateTime.Now;
            }

            set { dateCreated = value; }

        }

        public double TransportPercent_ { get; set; }
        public double Transport  { get; set; }
        public double Allowance { get; set; }
       // public double  { get; set; }


    }
}
