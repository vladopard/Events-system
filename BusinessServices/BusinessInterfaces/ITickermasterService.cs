using Events_system.Entities;

namespace Events_system.BusinessServices.BusinessInterfaces
{
    public interface ITickermasterService
    {
        Task<List<Event>> FetchEventsAsync();
    }
}
