namespace FloraEdu.Domain.Entities;

public class UniquePlantVisitor : BaseEntity
{
    public required Guid UUAID { get; set; } // Unique User Agent ID
    public required Guid PlantId { get; set; }
    public Plant Plant { get; set; }
    public string? UserId { get; set; }
    public User? User { get; set; }
    public bool? IsAnonymous { get; set; }
}
