namespace CalendarCourseWork.DataBase.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public bool EmailSend { get; set; }

        public User? User { get; set; }
        public int UserId { get; set; }
    }
}
