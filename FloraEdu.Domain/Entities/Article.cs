﻿namespace FloraEdu.Domain.Entities;

public class Article : BaseEntity
{
    public required string Title { get; set; }
    public required string Subtitle { get; set; }
    public string? HeaderImageUrl { get; set; }
    public required string Content { get; set; }
    public required string AuthorId { get; set; }
    public required User Author { get; set; }
    public List<ArticleComment> Comments { get; set; } = new();
}
