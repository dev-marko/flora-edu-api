namespace FloraEdu.Domain.DataTransferObjects.Article;

public class ArticlesRequestDto
{
    public string? SearchTerm { get; set; }
    public int Page { get; set; }
    public int Size { get; set; }
}
