using System;
namespace StaffPortal.Dtos
{
    public class SignInModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string Expires { get; set; }
    
    }
}
