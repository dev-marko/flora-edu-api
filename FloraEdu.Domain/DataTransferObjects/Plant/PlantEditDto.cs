namespace FloraEdu.Domain.DataTransferObjects.Plant;

public class PlantEditDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
    public string Predispositions { get; set; }
    public string Planting { get; set; }
    public string Maintenance { get; set; }
    public DateTime CreatedAt { get; set; }
}
