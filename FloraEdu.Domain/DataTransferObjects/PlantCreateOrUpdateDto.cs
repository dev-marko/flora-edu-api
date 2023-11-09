using FloraEdu.Domain.Enumerations;

namespace FloraEdu.Domain.DataTransferObjects;

public class PlantCreateOrUpdateDto
{
    public required string Name { get; set; }
    public required PlantType Type { get; init; } = 0;
    public string? Description { get; set; }
    public string? Predispositions { get; set; }
    public string? Planting { get; set; }
    public string? Maintenance { get; set; }
}
