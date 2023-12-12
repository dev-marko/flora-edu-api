﻿namespace FloraEdu.Domain.DataTransferObjects.Plant;

public class PlantCommentDto : BaseDto
{
    public Guid PlantId { get; set; }
    public required CommentUserInfoDto User { get; set; }
    public required string Content { get; set; }
    public required bool IsLiked { get; set; }
    public int LikeCount { get; set; }
}
