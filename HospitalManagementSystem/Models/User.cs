using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Models
{
    public static class UserRole
    {
        public const string Admin = "Admin";
        public const string Receptionist = "Receptionist";
    }

    // Login account, separate from Staff so admin accounts don't have to
    // correspond to a front-desk employee.
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; } = UserRole.Receptionist;

        public User() { }

        public User(string username, string passwordHash, string role)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be empty.");

            Username = username.Trim();
            PasswordHash = passwordHash;
            Role = role;
        }
    }
}
