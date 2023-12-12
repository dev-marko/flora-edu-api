namespace FloraEdu.Domain.DataTransferObjects.Article;

public class NewArticleCommentDto
{
    public Guid ArticleId { get; set; }
    public required string Content { get; set; }
}
