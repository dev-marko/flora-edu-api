using FloraEdu.Domain.Enumerations;

namespace FloraEdu.Domain.Entities;

public class Plant : BaseEntity
{
    public required string Name { get; set; }
    public required PlantType Type { get; set; }
    public required string Description { get; set; }
    public required string Predispositions { get; set; }
    public required string Planting { get; set; }
    public required string Maintenance { get; set; }
    public PlantImage? PlantImage { get; set; }
    public required User Author { get; set; }
    public List<PlantComment> Comments { get; set; } = new();
    public List<User> Likes { get; set; } = new();
    public List<User> Bookmarks { get; set; } = new();
    public List<UniquePlantVisitor> UniqueVisitors { get; set; } = new();
}
