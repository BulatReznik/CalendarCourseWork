using Microsoft.EntityFrameworkCore;

namespace CalendarCourseWork.BusinessLogic.Models
{
    public class EventsDataAccess
    {
        private readonly CalendarCourseWorkContext _context;

        public EventsDataAccess(CalendarCourseWorkContext context)
        {
            _context = context;
        }

        public async Task<List<Event>> GetEventsAsync(int userId, DateTime dateTimeFrom, DateTime dateTimeTo)
        {
            if (_context.Event == null)
            {
                return new List<Event>();
            }

            return await _context.Event
                .Include(e => e.Category)
                .Where(e => e.EventTime >= dateTimeFrom && e.EventTime <= dateTimeTo && e.Category.UserId == userId)
                .ToListAsync();
        }

        public async Task<Event> GetEventByIdAsync(int id, int userId)
        {
            if (_context.Event == null)
            {
                return null;
            }

            // Проверяем, что пользователь имеет доступ к событию через категорию
            var @event = await _context.Event
                .Include(e => e.Category) // Включаем связанную категорию
                .FirstOrDefaultAsync(e => e.Id == id && e.Category.UserId == userId);

            return @event;
        }

        public async Task<bool> UpdateEventAsync(int id, int userId, Event @event)
        {
            if (id != @event.Id || _context.Event == null)
            {
                return false;
            }

            // Проверяем, что пользователь имеет доступ к событию через категорию
            Event? existingEvent = await _context.Event
                .Include(e => e.Category) // Включаем связанную категорию
                .FirstOrDefaultAsync(e => e.Id == id && e.Category.UserId == userId);

            if (existingEvent == null)
            {
                return false; // Событие не найдено или пользователь не имеет доступа
            }

            _context.Entry(existingEvent).CurrentValues.SetValues(@event);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Event> CreateEventAsync(Event @event)
        {
            if (_context.Event == null)
            {
                // Handle the case where the entity set is null
                return null;
            }

            _context.Event.Add(@event);
            await _context.SaveChangesAsync();

            return @event;
        }

        public async Task<bool> DeleteEventAsync(int id, int userId)
        {
            if (_context.Event == null)
            {
                return false;
            }

            // Проверяем, что пользователь имеет доступ к событию через категорию
            Event? @event = await _context.Event
                .Include(e => e.Category) // Включаем связанную категорию
                .FirstOrDefaultAsync(e => e.Id == id && e.Category.UserId == userId);

            if (@event == null)
            {
                return false; // Событие не найдено или пользователь не имеет доступа
            }

            _context.Event.Remove(@event);
            await _context.SaveChangesAsync();

            return true;
        }

        public bool EventExists(int id)
        {
            return (_context.Event?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
