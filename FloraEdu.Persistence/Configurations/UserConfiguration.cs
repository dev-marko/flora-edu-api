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
    }
}
