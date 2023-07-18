using FloraEdu.Domain.Enumerations;

namespace FloraEdu.Domain.Entities;

public class Plant : BaseEntity
{
    public string Name { get; set; } = "";
    public PlantType Type { get; set; }
    public string Description { get; set; } = "";
    public string Predispositions { get; set; } = "";
    public string Maintenance { get; set; } = "";
}
