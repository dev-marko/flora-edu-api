namespace FloraEdu.Domain.DataTransferObjects.Analytics;

public class LikesDataDto
{
    public Guid EntityId { get; set; }
    public string Name { get; set; }
    public int LikesCount { get; set; }
}
