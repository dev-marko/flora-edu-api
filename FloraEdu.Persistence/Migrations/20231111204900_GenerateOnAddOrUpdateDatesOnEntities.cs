using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FloraEdu.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class GenerateOnAddOrUpdateDatesOnEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:plant_type", "unknown,fruit,vegetable,nuts,shrubs,tree,flower,herbs,other")
                .OldAnnotation("Npgsql:Enum:plant_type", "unknown,fruit,vegetable,tree,flower,herbs,other");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:plant_type", "unknown,fruit,vegetable,tree,flower,herbs,other")
                .OldAnnotation("Npgsql:Enum:plant_type", "unknown,fruit,vegetable,nuts,shrubs,tree,flower,herbs,other");
        }
    }
}
