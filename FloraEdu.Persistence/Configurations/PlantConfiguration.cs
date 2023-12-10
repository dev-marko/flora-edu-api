using FloraEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloraEdu.Persistence.Configurations;

public class PlantConfiguration : BaseEntityConfiguration<Plant>
{
    public override void Configure(EntityTypeBuilder<Plant> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("Plants");

        builder
            .HasOne(p => p.Author)
            .WithMany(a => a.AuthoredPlants)
            .HasForeignKey("AuthorId");
        
        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}
