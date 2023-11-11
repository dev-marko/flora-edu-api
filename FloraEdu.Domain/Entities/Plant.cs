using FloraEdu.Domain.Enumerations;

namespace FloraEdu.Domain.Entities;

public class Plant : BaseEntity
{
    public required string Name { get; set; }
    public required PlantType Type { get; set; }
    public string? Description { get; set; }
    public string? Predispositions { get; set; }
    public string? Planting { get; set; }
    public string? Maintenance { get; set; }
    public PlantImage? PlantImage { get; set; }
    public List<PlantComment> Comments { get; set; } = new();
}
