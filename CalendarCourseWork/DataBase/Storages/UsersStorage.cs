using CalendarCourseWork.DataBase.Models;
using CalendarCourseWork.Models;
using Microsoft.EntityFrameworkCore;

namespace CalendarCourseWork.DataBase.Storages
{
    public class UsersStorage
    {
        private readonly CalendarCourseWorkContext _context;

        public UsersStorage(CalendarCourseWorkContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            if (_context.User == null)
            {
                return new List<User>();
            }

            return await _context.User.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            if (_context.User == null)
            {
                return null;
            }

            return await _context.User.FindAsync(id);
        }

        public async Task<bool> UpdateUserAsync(int id, UserInputModel userInputModel)
        {
            User user = await GetUserByIdAsync(id);

            user.Name = userInputModel.Name;
            user.Email = userInputModel.Email;
            user.Password = userInputModel.Password;

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            if (_context.User == null)
            {
                return null;
            }

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            if (_context.User == null)
            {
                return false;
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public bool UserExists(string email)
        {
            return (_context.User?.Any(e => e.Email == email)).GetValueOrDefault();
        }

        public User GetUserByEmailAndPassword(string email, string password)
        {
            User user = _context.User.
                Include(rec => rec.Categories).
                    ThenInclude(rec => rec.Events).
                FirstOrDefault(u => u.Email == email && u.Password == password);
            return user;
        }
    }
}
