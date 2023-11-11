namespace FloraEdu.Domain.Entities;

public class PlantImage : BaseEntity
{
    public Guid PlantId { get; set; }
    public Plant? Plant { get; set; }
    public string? ThumbnailImageUrl { get; set; }
    public List<string> HeaderImageUrls { get; set; } = new();
}
