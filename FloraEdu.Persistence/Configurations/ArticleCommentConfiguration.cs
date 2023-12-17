using FloraEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloraEdu.Persistence.Configurations;

public class ArticleCommentConfiguration : BaseEntityConfiguration<ArticleComment>
{
    public override void Configure(EntityTypeBuilder<ArticleComment> builder)
    {
        base.Configure(builder);

        builder.ToTable("ArticleComments");

        builder
            .HasOne(articleComment => articleComment.User);

        builder
            .HasMany(articleComment => articleComment.Likes)
            .WithMany(user => user.LikedArticleComments)
            .UsingEntity(join => join.ToTable("ArticleCommentLikes"));
    }
}
