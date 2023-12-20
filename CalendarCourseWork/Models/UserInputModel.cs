using CalendarCourseWork.BusinessLogic.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace CalendarCourseWork.Models
{
    public class UserInputModel
    {
        [BindProperty]
        public string? Name { get; set; }
        [BindProperty]
        public string? Email { get; set; }
        [BindProperty]
        public string? Password { get; set; }
    }
}
