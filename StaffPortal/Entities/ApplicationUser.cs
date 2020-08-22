using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StaffPortal.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string NewStates { get; set; }
        public string LGAs { get; set; }
        public string Country { get; set; }
        public int NewStateId { get; set; }
        public int LGAId { get; set; }


        public NewState NewState { get; set; }
        public LGA LGA { get; set; }


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


        [NotMapped]
        public string FullName
        {
            get
            {

                return this.FirstName + " " + this.LastName;
            }
        }
        
    }
}
