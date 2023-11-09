using FloraEdu.Domain.Entities;

namespace FloraEdu.Domain.DataTransferObjects.Plant;

public class PlantCommentDto : BaseDto
{
    public Guid PlantId { get; set; }
    public User? User { get; set; }
    public string? Content { get; set; }
}
