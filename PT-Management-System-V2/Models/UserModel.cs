using System.ComponentModel.DataAnnotations;

namespace PT_Management_System_V2.Models
{
    // Exclusively handles the user logins for the system.
    public class UserLogin
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }

        public string Password { get; set; }
    }

    public class UserModel
    {
        [Key]
        public int UserId { get; set; }
    
        public string Username { get; set; }

        public string UserEmail { get; set; }

        public DateTime CreatedAt { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserPhone { get; set; }

        public DateOnly DoB {  get; set; }

        public bool IsActiveUser { get; set; }
    }

}
