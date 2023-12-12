namespace FloraEdu.Domain.DataTransferObjects;

public class CommentUserInfoDto : BaseDto
{
    public required string UserName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? AvatarImageUrl { get; set; }
}
