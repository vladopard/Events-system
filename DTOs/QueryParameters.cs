namespace Events_system.DTOs
{
    public abstract class QueryParameters
    {
        const int maxPageSize = 20;
        private int _pageSize = 6;
        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > maxPageSize ? maxPageSize : value;
        }
        public string? SortBy { get; set; }
        public string? SearchTerm { get; set; }
    }

    public class EventQueryParameters : QueryParameters { }
}
