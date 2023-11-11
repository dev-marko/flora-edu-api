using FloraEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloraEdu.Persistence.Configurations;

public class PlantConfiguration : IEntityTypeConfiguration<Plant>
{
    public void Configure(EntityTypeBuilder<Plant> builder)
    {
        builder.ToTable("Plants");
        
        builder.Property(plant => plant.Id).HasDefaultValueSql("uuid_generate_v4()");
        
        builder
            .Property(plant => plant.CreatedAt)
            .HasDefaultValueSql("now() at time zone 'utc'")
            .ValueGeneratedOnAdd();
        
        builder
            .Property(plant => plant.LastModified)
            .HasDefaultValueSql("now() at time zone 'utc'")
            .ValueGeneratedOnAddOrUpdate();
        
        builder.Property(plant => plant.IsDeleted).HasDefaultValue(false);
    }
}
