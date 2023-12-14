using CalendarCourseWork.DataBase.Models;
using CalendarCourseWork.Logic;
using System.Security.Claims;


namespace CalendarCourseWork.Security
{
    public class JWTUser
    {
        private readonly UserLogic _userLogic;

        public JWTUser(UserLogic logic)
        {
            _userLogic = logic;
        }

        public ClaimsIdentity GetIdentity(User model)
        {
            User user = _userLogic.ReadElement(new User
            {
                Email = model.Email,
                Password = string.IsNullOrEmpty(model.Password) ? " " : model.Password
            });

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email)
                };
                ClaimsIdentity claimsIdentity = new(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // если пользователя не найдено
            return null;
        }

        public User GetUser(ClaimsIdentity identity)
        {
            if (identity != null)
            {
                string? email = identity.Name;
                return _userLogic.ReadElement(new User { Email = email });
            }
            return null;
        }
    }
}
