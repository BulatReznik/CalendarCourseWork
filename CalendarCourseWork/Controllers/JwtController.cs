using CalendarCourseWork.DataBase.Models;
using CalendarCourseWork.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CalendarCourseWork.Security
{
    [Route("api/[controller]")]
    [ApiController]
    public class JwtController : ControllerBase
    {
        private readonly JWTUser _jwtUser;

        public JwtController(JWTUser jwtUser)
        {
            _jwtUser = jwtUser;
        }

        [HttpPost("token")]
        public async Task<IActionResult> Token(JWTInputModel userInputModel)
        {
            User user = new() 
            {
                Email = userInputModel.Email,
                Password = userInputModel.Password,
            };

            ClaimsIdentity identity = await _jwtUser.GetIdentityAsync(user);

            if (identity != null)
            {
                Claim? userIdClaim = identity.FindFirst("Id");
                int userId = userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;

                DateTime now = DateTime.UtcNow;
                JwtSecurityToken jwt = new(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

                string encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                RegisterOutputModel response = new()
                {
                    UserId = userId,
                    Token = encodedJwt
                };

                // Возвращается успешный HTTP-ответ с Id пользователя и токеном
                return Ok(response);
            }

            return BadRequest(new { errorText = "Неправильное имя пользователя или пароль" });
        }
    }
}