namespace Dynatron.Shared
{
    public record PaginationCriteria(int Page, int PageSize)
    {
        public int RowOffset => (Page - 1) * PageSize;
    }
}
