using FloraEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloraEdu.Persistence.Configurations;

public class PlantImageConfiguration : IEntityTypeConfiguration<PlantImage>
{
    public void Configure(EntityTypeBuilder<PlantImage> builder)
    {
        builder.ToTable("PlantImageUrls");
        
        builder.Property(plantImageObj => plantImageObj.Id).HasDefaultValueSql("uuid_generate_v4()");
        
        builder
            .Property(plantImageObj => plantImageObj.CreatedAt)
            .HasDefaultValueSql("now() at time zone 'utc'")
            .ValueGeneratedOnAdd();
        
        builder
            .Property(plantImageObj => plantImageObj.LastModified)
            .HasDefaultValueSql("now() at time zone 'utc'")
            .ValueGeneratedOnAddOrUpdate();
        
        builder.Property(plantImageObj => plantImageObj.IsDeleted).HasDefaultValue(false);
    }
}
