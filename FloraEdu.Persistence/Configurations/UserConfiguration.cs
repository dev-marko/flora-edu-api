using FloraEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloraEdu.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder
            .HasMany(user => user.LikedPlants)
            .WithMany(plant => plant.Likes)
            .UsingEntity(join => join.ToTable("UserPlantLikes"));

        builder
            .HasMany(user => user.BookmarkedPlants)
            .WithMany(plant => plant.Bookmarks)
            .UsingEntity(join => join.ToTable("UserPlantBookmarks"));

        builder
            .HasMany(user => user.LikedArticles)
            .WithMany(article => article.Likes)
            .UsingEntity(join => join.ToTable("UserArticleLikes"));

        builder
            .HasMany(user => user.BookmarkedArticles)
            .WithMany(article => article.Bookmarks)
            .UsingEntity(join => join.ToTable("UserArticleBookmarks"));

        builder
            .HasMany(user => user.LikedPlantComments)
            .WithMany(plantComment => plantComment.Likes)
            .UsingEntity(join => join.ToTable("PlantCommentLikes"));

        builder
            .HasMany(user => user.LikedArticleComments)
            .WithMany(articleComment => articleComment.Likes)
            .UsingEntity(join => join.ToTable("ArticleCommentLikes"));
    }
}
