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
    }
}
