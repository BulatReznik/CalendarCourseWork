using CalendarCourseWork.BusinessLogic.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace CalendarCourseWork.Models
{
    public class CategoryOutputModel
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public bool EmailSend { get; set; }
        public int UserId { get; set; }
    }
}
