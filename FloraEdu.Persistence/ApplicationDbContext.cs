using FloraEdu.Domain.Entities;
using FloraEdu.Domain.Enumerations;
using FloraEdu.Persistence.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FloraEdu.Persistence;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasPostgresExtension("uuid-ossp");
        builder.HasPostgresEnum<PlantType>();
        
        builder.ApplyConfiguration(new UserConfiguration());
        builder.ApplyConfiguration(new PlantConfiguration());
        builder.ApplyConfiguration(new PlantCommentConfiguration());
        builder.ApplyConfiguration(new PlantImageConfiguration());
    }
}
