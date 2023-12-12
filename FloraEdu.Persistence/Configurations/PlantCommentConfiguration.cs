using FloraEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloraEdu.Persistence.Configurations;

public class PlantCommentConfiguration : BaseEntityConfiguration<PlantComment>
{
    public override void Configure(EntityTypeBuilder<PlantComment> builder)
    {
        base.Configure(builder);

        builder.ToTable("PlantComments");

        builder.HasOne(plantComment => plantComment.User);

        builder
            .HasMany(plantComment => plantComment.Likes)
            .WithMany(user => user.LikedPlantComments)
            .UsingEntity(join => join.ToTable("PlantCommentLikes"));
    }
}
