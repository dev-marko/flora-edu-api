using FloraEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloraEdu.Persistence.Configurations;

public class PlantCommentConfiguration : IEntityTypeConfiguration<PlantComment>
{
    public void Configure(EntityTypeBuilder<PlantComment> builder)
    {
        builder.ToTable("PlantComments");
        builder.Property(plantComment => plantComment.Id).ValueGeneratedOnAdd();
    }
}
