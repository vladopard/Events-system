using Events_system.BusinessServices.BusinessInterfaces;
using Events_system.Entities;
using Newtonsoft.Json.Linq;

namespace Events_system.BusinessServices
{
    public class TicketmasterService : ITickermasterService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public TicketmasterService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task<List<Event>> FetchEventsAsync()
        {
            var apiKey = _configuration["Ticketmaster:ApiKey"];
            var url = $"https://app.ticketmaster.com/discovery/v2/events.json?apikey={apiKey}&size=20";

            //pozivamo api, dobijamo nazad HttpResponseMessage
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            //citamo telo http odgovora kao JSON u vidu stringa
            var json = await response.Content.ReadAsStringAsync();
            //pretvara to dalje u dinamicki JSON objekat
            var data = JObject.Parse(json);

            var events = new List<Event>();

            var items = data["_embedded"]?["events"];
            if (items == null) return events;

            foreach (var item in items)
            {
                var evt = new Event
                {
                    Name = item["name"]?.ToString() ?? "No name",
                    Description = item["info"]?.ToString(),
                    ImageUrl = item["images"]?.FirstOrDefault()?["url"]?.ToString(),
                    Venue = item["_embedded"]?["venues"]?.FirstOrDefault()?["name"]?.ToString() ?? "Unknown",
                    City = item["_embedded"]?["venues"]?.FirstOrDefault()?["city"]?["name"]?.ToString() ?? "Unknown",
                    StartDate = DateTime.SpecifyKind(
                    DateTime.Parse(item["dates"]?["start"]?["dateTime"]?.ToString() ?? DateTime.UtcNow.ToString()),
                    DateTimeKind.Utc)
                };

                events.Add(evt);
            }

            return events;
        }
    }
}
