namespace FloraEdu.Domain.Entities;

public class UniqueArticleVisitor : BaseEntity
{
    public required Guid UUAID { get; set; } // Unique User Agent ID
    public required Guid ArticleId { get; set; }
    public Article Article { get; set; }
    public string? UserId { get; set; }
    public User? User { get; set; }
    public bool? IsAnonymous { get; set; }
}
