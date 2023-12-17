using CalendarCourseWork.DataBase.Models;
using CalendarCourseWork.Logic;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CalendarCourseWork.Security
{
    public class JWTUser
    {
        private readonly UsersLogic _userLogic;

        public JWTUser(UsersLogic logic)
        {
            _userLogic = logic;
        }

        public async Task<ClaimsIdentity> GetIdentityAsync(User model)
        {
            User user = await _userLogic.ReadElement(new User
            {
                Email = model.Email,
                Password = string.IsNullOrEmpty(model.Password) ? " " : model.Password
            });

            if (user != null)
            {
                List<Claim> claims = new()
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                    new Claim("Id", user.Id.ToString()) // Добавляем Claim с типом "Id"
                };

                ClaimsIdentity claimsIdentity = new(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // если пользователя не найдено
            return null;
        }

        public async Task<User> GetUser(ClaimsIdentity identity)
        {
            if (identity != null)
            {
                Claim? idClaim = identity.FindFirst("Id");
                int id = idClaim != null ? Convert.ToInt32(idClaim.Value) : 0;

                return await _userLogic.ReadElement(new User { Id = id });
            }
            return null;
        }
    }
}