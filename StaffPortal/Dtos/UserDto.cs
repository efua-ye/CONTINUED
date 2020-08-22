using System;
namespace StaffPortal.Dtos
{
    public class UserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string Password { get; set; }
    }

    public class LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}


//[Required]
//public string UserName { get; set; }
//[Required]
//[DataType(DataType.Password)]
//public string Password { get; set; }
//[Required]
//[DataType(DataType.Password)]
//public string ConfirmPassword { get; set; }
//[Required]
//[DataType(DataType.EmailAddress)]
//public string Email { get; set; }
//[Required]
//public string FullName { get; set; }
//[Required]
//[DataType(DataType.Date)]
//public DateTime BirthDate { get; set; }