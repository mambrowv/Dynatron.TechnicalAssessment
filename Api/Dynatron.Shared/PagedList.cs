namespace Dynatron.Shared
{
    public class PagedList<T>
    {
        public PagedList(IEnumerable<T> items, int page, int pageSize, int totalRows)
        {
            Items = new List<T>(items);
            Page = page;
            PageSize = pageSize;
            TotalRows = totalRows;
        }

        public IEnumerable<T> Items { get; }
        public int PageSize { get; }
        public int Page { get; }
        public int TotalRows { get; }
        public int TotalPages => TotalRows / Math.Max(PageSize, 1);
    }
}
