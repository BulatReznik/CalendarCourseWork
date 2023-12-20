using CalendarCourseWork.BusinessLogic.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace CalendarCourseWork.Models
{
    public class JWTInputModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
