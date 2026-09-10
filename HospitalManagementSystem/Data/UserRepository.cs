using System.Linq;
using HospitalManagementSystem.Models;

namespace HospitalManagementSystem.Data
{
    public interface IUserRepository
    {
        User GetByUsername(string username);
        User Authenticate(string username, string password);
    }

    public class UserRepository : IUserRepository
    {
        private readonly HospitalContext _context;

        public UserRepository(HospitalContext context)
        {
            _context = context;
        }

        public User GetByUsername(string username) =>
            _context.Users.FirstOrDefault(u => u.Username == username);

        public User Authenticate(string username, string password)
        {
            var user = GetByUsername(username);
            if (user == null || !PasswordHasher.Verify(password, user.PasswordHash))
                throw new InvalidCredentialsException("Incorrect username or password.");

            return user;
        }
    }
}
