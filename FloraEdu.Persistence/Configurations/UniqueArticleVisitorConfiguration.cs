using FloraEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloraEdu.Persistence.Configurations;

public class UniqueArticleVisitorConfiguration : BaseEntityConfiguration<UniqueArticleVisitor>
{
    public override void Configure(EntityTypeBuilder<UniqueArticleVisitor> builder)
    {
        base.Configure(builder);
        builder.ToTable("UniqueArticleVisitors");
    }
}
