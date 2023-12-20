using CalendarCourseWork.BusinessLogic.Models;

namespace CalendarCourseWork.Models
{
    public class CalendarViewModel
    {
        public DateTime FirstDayOfMonth { get; set; }
        public List<Event>? Events { get; set; }
        public int UserId { get; set; }
    }
}
