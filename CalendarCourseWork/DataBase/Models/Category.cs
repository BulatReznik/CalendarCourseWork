using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CalendarCourseWork.DataBase.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Header { get; set; }
        [Required]
        public bool EmailSend { get; set; }

        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Для обеспечения связи один-ко-многим категория-события
        /// </summary>
        [ForeignKey("CategoryId")]
        public virtual List<Event>? Events { get; set; }
    }
}