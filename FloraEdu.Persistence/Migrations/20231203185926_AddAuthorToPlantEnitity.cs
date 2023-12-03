using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FloraEdu.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthorToPlantEnitity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "Plants",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Plants_AuthorId",
                table: "Plants",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Plants_Users_AuthorId",
                table: "Plants",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plants_Users_AuthorId",
                table: "Plants");

            migrationBuilder.DropIndex(
                name: "IX_Plants_AuthorId",
                table: "Plants");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Plants");
        }
    }
}
