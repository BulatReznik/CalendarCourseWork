using Microsoft.EntityFrameworkCore;
using MimeKit;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CalendarCourseWork.DataBase.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// Для обеспечения связи один-ко-многим пользователь-категории
        /// </summary>
        [ForeignKey("UserId")]
        public virtual List<Category>? Categories { get; set; }
    }
}