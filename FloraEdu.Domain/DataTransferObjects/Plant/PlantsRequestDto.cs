using FloraEdu.Domain.Enumerations;

namespace FloraEdu.Domain.DataTransferObjects.Plant;

public class PlantsRequestDto
{
    public PlantType Type { get; set; }
    public int Page { get; set; }
    public int Size { get; set; }
}
