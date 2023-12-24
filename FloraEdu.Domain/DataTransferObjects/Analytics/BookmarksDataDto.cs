namespace FloraEdu.Domain.DataTransferObjects.Analytics;

public class BookmarksDataDto
{
    public Guid EntityId { get; set; }
    public string Name { get; set; }
    public int BookmarksCount { get; set; }
}
