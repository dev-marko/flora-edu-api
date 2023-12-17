using Microsoft.AspNetCore.Identity;

namespace FloraEdu.Domain.Entities;

public class User : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? AvatarImageUrl { get; set; }
    public bool IsDeleted { get; set; }
    public string? AuthorBiography { get; set; }
    public string? ProfileBiography { get; set; }
    public List<Plant> AuthoredPlants { get; set; } = new();
    public List<Plant> LikedPlants { get; set; } = new();
    public List<Plant> BookmarkedPlants { get; set; } = new();
    public List<Article> AuthoredArticles { get; set; } = new();
    public List<Article> LikedArticles { get; set; } = new();
    public List<Article> BookmarkedArticles { get; set; } = new();
    public List<PlantComment> LikedPlantComments { get; set; } = new();
    public List<ArticleComment> LikedArticleComments { get; set; } = new();
}
