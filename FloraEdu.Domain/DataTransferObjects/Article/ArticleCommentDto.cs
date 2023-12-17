namespace FloraEdu.Domain.DataTransferObjects.Article;

public class ArticleCommentDto : BaseDto
{
    public Guid ArticleId { get; set; }
    public required CommentUserInfoDto User { get; set; }
    public required string Content { get; set; }
    public required bool IsLiked { get; set; }
    public int LikeCount { get; set; }
}
