namespace Events_system.Helpers
{
    public class MetaData
    {
        public int TotalCount { get; set; } //broj item-a
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public bool HasNext => CurrentPage < TotalPages;
        public bool HasPrevious => CurrentPage > 1;
    }

    public class CachedPageDto<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public MetaData MetaData { get; set; } = null!;
    }
}
