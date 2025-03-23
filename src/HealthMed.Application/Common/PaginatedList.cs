namespace HealthMed.Application.Common;

public class PaginatedList<T> : List<T>
{
    private int CurrentPage { get; set; }
    private int TotalPages { get; set; }
    public int PageSize { get; private set; }
    public int TotalCount { get; private set; }

    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;
    public int GetCurrentPage => CurrentPage;
    public int GetTotalPages => TotalPages;
    
    private PaginatedList(List<T> items, int count, int pageNumber, int pageSize)
    {
        TotalCount = count;
        PageSize = pageSize;
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);

        AddRange(items);
    }

    public static PaginatedList<T> Create(IEnumerable<T> source, int pageNumber, int pageSize,
        CancellationToken cancellationToken = default)
    {
        var enumerable = source.ToList();
        var count = enumerable.Count;
        var items = enumerable.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        return new PaginatedList<T>(items, count, pageNumber, pageSize);
    }
}