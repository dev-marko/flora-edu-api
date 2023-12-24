namespace FloraEdu.Domain.DataTransferObjects.Article;

public class ArticleEditDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public string HeaderImageUrl { get; set; }
    public string ShortDescription { get; set; }
    public string Content { get; set; }
    public bool IsNew { get; set; }
}
