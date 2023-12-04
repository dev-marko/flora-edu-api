using FloraEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloraEdu.Persistence.Configurations;

public class ArticleConfiguration : BaseEntityConfiguration<Article>
{
    public override void Configure(EntityTypeBuilder<Article> builder)
    {
        base.Configure(builder);

        builder.ToTable("Articles");

        builder
            .HasOne(a => a.Author)
            .WithMany(a => a.AuthoredArticles)
            .HasForeignKey("AuthorId");
    }
}
