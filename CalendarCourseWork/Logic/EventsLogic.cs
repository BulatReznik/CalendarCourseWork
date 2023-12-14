namespace CalendarCourseWork.DataBase.Models
{
    public class EventsLogic
    {
        private readonly EventsStorage _eventsStorage;

        public EventsLogic(EventsStorage eventsStorage)
        {
            _eventsStorage = eventsStorage;
        }

        public async Task<List<Event>> GetEventsAsync()
        {
            return await _eventsStorage.GetEventsAsync();
        }

        public async Task<Event> GetEventByIdAsync(int id)
        {
            return await _eventsStorage.GetEventByIdAsync(id);
        }

        public async Task<bool> UpdateEventAsync(int id, Event @event)
        {
            return await _eventsStorage.UpdateEventAsync(id, @event);
        }

        public async Task<Event> CreateEventAsync(Event @event)
        {
            return await _eventsStorage.CreateEventAsync(@event);
        }

        public async Task<bool> DeleteEventAsync(int id)
        {
            return await _eventsStorage.DeleteEventAsync(id);
        }

        public bool EventExists(int id)
        {
            return _eventsStorage.EventExists(id);
        }
    }
}
