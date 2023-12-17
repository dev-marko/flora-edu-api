namespace FloraEdu.Domain.Entities;

public class PlantComment : BaseEntity
{
    public Guid PlantId { get; set; }
    public required Plant Plant { get; set; }
    public required string UserId { get; set; }
    public required User User { get; set; }
    public required string Content { get; set; }
    public List<User> Likes { get; set; } = new();
}
