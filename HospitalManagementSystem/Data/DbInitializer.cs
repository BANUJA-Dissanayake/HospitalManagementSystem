using System.Linq;
using HospitalManagementSystem.Models;

namespace HospitalManagementSystem.Data
{
    // Creates the SQLite database file on first run and seeds a default admin
    // account, so the app works out of the box on any teammate's machine
    // without a manual migration step.
    public static class DbInitializer
    {
        public const string DefaultAdminUsername = "admin";
        public const string DefaultAdminPassword = "admin123";

        public static void Initialize(HospitalContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Users.Any())
            {
                context.Users.Add(new User(
                    DefaultAdminUsername,
                    PasswordHasher.Hash(DefaultAdminPassword),
                    UserRole.Admin));
                context.SaveChanges();
            }
        }
    }
}
