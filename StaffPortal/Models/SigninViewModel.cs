//using System;
using System.ComponentModel.DataAnnotations;
using StaffPortal.Entities;

namespace StaffPortal.Models
{
    public class SigninViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        //[DataType(DataType.Text)]
        public string UserName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public int NewStateId { get; set; }

        [Required]
        public int LGAId { get; set; }


        public NewState NewState { get; set; }
        public LGA LGA { get; set; }

       

        [Required]
        public string Country { get; set; }


        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
