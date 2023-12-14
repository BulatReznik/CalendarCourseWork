using System;

namespace CalendarCourseWork.DataBase.Models
{
    public class Event
    {
        public int Id { get; set; }
        public DateTime EventTime { get; set; }
        public string EventName { get; set;}
        public string EventDescription { get; set;}

        public Category? Category { get; set;}
        public int CategoryId { get; set;}

        public User? User { get; set; }
        public int UserId { get; set;}
    }
}
