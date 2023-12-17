namespace FloraEdu.Domain.DataTransferObjects.Plant;

public class NewPlantCommentDto
{
    public Guid PlantId { get; set; }
    public required string Content { get; set; }
}
