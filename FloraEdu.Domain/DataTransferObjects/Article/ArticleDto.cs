namespace FloraEdu.Domain.DataTransferObjects.Article;

public class ArticleDto : BaseDto
{
    public string? Title { get; set; }
    public string? Subtitle { get; set; }
    public string? ShortDescription { get; set; }
    public string? HeaderImageUrl { get; set; }
    public string? Content { get; set; }
    public AuthorDto? Author { get; set; }
    public bool IsLiked { get; set; }
    public bool IsBookmarked { get; set; }
}
