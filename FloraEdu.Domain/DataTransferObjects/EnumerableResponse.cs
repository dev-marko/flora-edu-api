namespace FloraEdu.Domain.DataTransferObjects;

public class EnumerableResponse<T>
{
    // TODO: Use in the future for pagination
    public IEnumerable<T> Items { get; set; }
    public int Count { get; set; }
    public int? Limit { get; set; }
    public int? Offset { get; set; }

    public EnumerableResponse()
    {
        Items = new List<T>();
        Count = 0;
    }

    public EnumerableResponse(IEnumerable<T> items, int count)
    {
        Items = items;
        Count = count;
    }
}
