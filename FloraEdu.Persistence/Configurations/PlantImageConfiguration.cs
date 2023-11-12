using FloraEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloraEdu.Persistence.Configurations;

public class PlantImageConfiguration : BaseEntityConfiguration<PlantImage>
{
    public override void Configure(EntityTypeBuilder<PlantImage> builder)
    {
        base.Configure(builder);
        builder.ToTable("PlantImageUrls");
    }
}
