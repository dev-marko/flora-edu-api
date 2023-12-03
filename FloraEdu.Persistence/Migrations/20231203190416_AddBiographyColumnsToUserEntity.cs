using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FloraEdu.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddBiographyColumnsToUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorBiography",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileBiography",
                table: "Users",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorBiography",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ProfileBiography",
                table: "Users");
        }
    }
}
