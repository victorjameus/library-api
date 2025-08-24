namespace Library.Application.Common.Models
{
    public sealed class Paginated<T>
    {
        public IReadOnlyList<T> Items { get; }
        public int TotalCount { get; }
        public int Page { get; }
        public int PageSize { get; }
        public int TotalPages { get; }
        public bool HasNextPage { get; }
        public bool HasPreviousPage { get; }

        public Paginated(IEnumerable<T> items, int totalCount, int page, int pageSize)
        {
            Items = items.ToList().AsReadOnly();
            TotalCount = totalCount;
            Page = page;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            HasNextPage = page < TotalPages;
            HasPreviousPage = page > 1;
        }

        public static Paginated<T> Empty(int page, int pageSize)
        {
            return new Paginated<T>([], 0, page, pageSize);
        }

        public static Paginated<T> Create(IEnumerable<T> items, int totalCount, int page, int pageSize)
        {
            return new Paginated<T>(items, totalCount, page, pageSize);
        }
    }
}
