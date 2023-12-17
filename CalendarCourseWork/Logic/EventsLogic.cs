namespace CalendarCourseWork.DataBase.Models
{
    public class EventsLogic
    {
        private readonly EventsStorage _eventsStorage;

        public EventsLogic(EventsStorage eventsStorage)
        {
            _eventsStorage = eventsStorage;
        }

        public async Task<List<Event>> GetEventsAsync(int userId, DateTime dateTimeFrom, DateTime dateTimeTo)
        {
            return await _eventsStorage.GetEventsAsync(userId, dateTimeFrom, dateTimeTo);
        }

        public async Task<Event> GetEventByIdAsync(int id, int userId)
        {
            return await _eventsStorage.GetEventByIdAsync(id, userId);
        }

        public async Task<bool> UpdateEventAsync(int id, int userId, Event @event)
        {
            return await _eventsStorage.UpdateEventAsync(id, userId, @event);
        }

        public async Task<Event> CreateEventAsync(Event @event)
        {
            return await _eventsStorage.CreateEventAsync(@event);
        }

        public async Task<bool> DeleteEventAsync(int id, int userId)
        {
            return await _eventsStorage.DeleteEventAsync(id, userId);
        }

        public bool EventExists(int id)
        {
            return _eventsStorage.EventExists(id);
        }
    }
}
