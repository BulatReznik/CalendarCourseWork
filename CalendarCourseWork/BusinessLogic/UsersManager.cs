using CalendarCourseWork.BusinessLogic.Models;
using CalendarCourseWork.BusinessLogic.Storages;
using CalendarCourseWork.Models;
using CalendarCourseWork.Security;
using System.Security.Claims;

namespace CalendarCourseWork.Logic
{
    public class UsersManager
    {
        private readonly UsersDataAccess _usersStorage;

        public UsersManager()
        {

        }

        public UsersManager(UsersDataAccess usersStorage)
        {
            _usersStorage = usersStorage;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await _usersStorage.GetUsersAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _usersStorage.GetUserByIdAsync(id);
        }

        public async Task<bool> UpdateUserAsync(int id, UserInputModel user)
        {
            return await _usersStorage.UpdateUserAsync(id, user);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            if (UserExists(user.Email))
            {
                return null;
            }

            return await _usersStorage.CreateUserAsync(user);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _usersStorage.DeleteUserAsync(id);
        }

        public bool UserExists(string email)
        {
            return _usersStorage.UserExists(email);
        }

        public async Task<User> GetUserByEmailAndPassword(string email, string password)
        {
            return await _usersStorage.GetUserByEmailAndPassword(email, password); 
        }

        public async Task<User> ReadElement(User user)
        {
            User retrievedUser = new();

            if (user.Password != null)
            {
                retrievedUser = await _usersStorage.GetUserByEmailAndPassword(user.Email, user.Password);
            }
            else if (user.Id != null)
            {
                retrievedUser = await _usersStorage.GetUserByIdAsync(user.Id);
            }

            return retrievedUser;
        }

        public async Task<User> GetCurrentUserCreds(HttpContext httpContext, JWTUser jwtUser)
        {
            ClaimsIdentity? identity = httpContext.User.Identity as ClaimsIdentity;
            return await jwtUser.GetUser(identity);
        }
    }
}
