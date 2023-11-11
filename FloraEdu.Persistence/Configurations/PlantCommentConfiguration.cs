using FloraEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloraEdu.Persistence.Configurations;

public class PlantCommentConfiguration : IEntityTypeConfiguration<PlantComment>
{
    public void Configure(EntityTypeBuilder<PlantComment> builder)
    {
        builder.ToTable("PlantComments");
        
        builder.Property(plantComment => plantComment.Id).HasDefaultValueSql("uuid_generate_v4()");
        
        builder
            .Property(plantComment => plantComment.CreatedAt)
            .HasDefaultValueSql("now() at time zone 'utc'")
            .ValueGeneratedOnAdd();

        builder
            .Property(plantComment => plantComment.LastModified)
            .HasDefaultValueSql("now() at time zone 'utc'")
            .ValueGeneratedOnAddOrUpdate();

        builder.Property(plantComment => plantComment.IsDeleted).HasDefaultValue(false);
    }
}
