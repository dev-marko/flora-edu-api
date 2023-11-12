namespace FloraEdu.Domain.Entities;

public class ArticleComment : BaseEntity
{
    public Guid ArticleId { get; set; }
    public required Article Article { get; set; }
    public required string UserId { get; set; }
    public required User User { get; set; }
    public required string Content { get; set; }
}
