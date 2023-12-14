using Microsoft.EntityFrameworkCore;

namespace CalendarCourseWork.DataBase.Models
{
    public class EventsStorage
    {
        private readonly CalendarCourseWorkContext _context;

        public EventsStorage(CalendarCourseWorkContext context)
        {
            _context = context;
        }

        public async Task<List<Event>> GetEventsAsync()
        {
            if (_context.Event == null)
            {
                return new List<Event>();
            }

            return await _context.Event.ToListAsync();
        }

        public async Task<Event> GetEventByIdAsync(int id)
        {
            if (_context.Event == null)
            {
                return null;
            }

            return await _context.Event.FindAsync(id);
        }

        public async Task<bool> UpdateEventAsync(int id, Event @event)
        {
            if (id != @event.Id)
            {
                return false;
            }

            _context.Entry(@event).State = EntityState.Modified;
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

        public async Task<bool> DeleteEventAsync(int id)
        {
            if (_context.Event == null)
            {
                return false;
            }

            var @event = await _context.Event.FindAsync(id);
            if (@event == null)
            {
                return false;
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
