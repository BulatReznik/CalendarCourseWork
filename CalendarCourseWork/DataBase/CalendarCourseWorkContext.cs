using Microsoft.EntityFrameworkCore;
using CalendarCourseWork.DataBase.Models;

namespace CalendarCourseWork.DataBase
{
    public class CalendarCourseWorkContext : DbContext
    {
        public CalendarCourseWorkContext(DbContextOptions<CalendarCourseWorkContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=CalendarCourseWorkContext;Trusted_Connection=True;Trust Server Certificate=true;");

            }
            base.OnConfiguring(optionsBuilder);
        }

        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Event> Event { get; set; }
        public virtual DbSet<User> User { get; set; }  
    }
}
