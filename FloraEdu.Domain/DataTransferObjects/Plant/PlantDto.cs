namespace FloraEdu.Domain.DataTransferObjects.Plant;

public class PlantDto : BaseDto
{
    public string? Name { get; set; }
    public string? Type { get; set; }
    public string? ThumbnailImageUrl { get; set; }
    public List<string> HeaderImageUrls { get; set; } = new();
    public string? Description { get; set; }
    public string? Predispositions { get; set; }
    public string? Planting { get; set; }
    public string? Maintenance { get; set; }
    public AuthorDto Author { get; set; }
    public int LikeCount { get; set; }
    public bool IsLiked { get; set; }
    public int BookmarkCount { get; set; }
    public bool IsBookmarked { get; set; }
    public List<PlantCommentDto> Comments { get; set; } = new();
    public int CommentCount { get; set; }
    public int VisitorsCount { get; set; }
}
