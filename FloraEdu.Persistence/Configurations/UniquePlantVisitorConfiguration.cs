using FloraEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloraEdu.Persistence.Configurations;

public class UniquePlantVisitorConfiguration : BaseEntityConfiguration<UniquePlantVisitor>
{
    public override void Configure(EntityTypeBuilder<UniquePlantVisitor> builder)
    {
        base.Configure(builder);
        builder.ToTable("UniquePlantVisitors");
    }
}
