namespace FloraEdu.Domain.DataTransferObjects.Plant;

public class PlantCardDto : BaseDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Type { get; set; }
    public bool IsLiked { get; set; }
    public bool IsBookmarked { get; set; }
}
