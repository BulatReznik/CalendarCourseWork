using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace CalendarCourseWork.BusinessLogic.Models
{
    public class Event
    {
        public int Id { get; set; }
        [Required]
        public DateTime EventTime { get; set; }
        [Required]
        public string EventName { get; set; }
        public string EventDescription { get; set; }

        /// <summary>
        /// Для обеспечения свиязи один-ко-многим категория-события
        /// </summary>
        [JsonIgnore]
        public virtual Category? Category { get; set; }
        public int CategoryId { get; set; }
    }
}