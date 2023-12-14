using System.ComponentModel.DataAnnotations.Schema;

namespace CalendarCourseWork.DataBase.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }  
        public string Password { get; set; }

        [ForeignKey ("UserId")]
        public virtual List<Event> Events { get; set; }

        [ForeignKey ("UserId")]
        public virtual List<Category> Categories { get; set; }
    }
}
