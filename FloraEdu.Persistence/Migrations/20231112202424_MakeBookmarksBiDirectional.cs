using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FloraEdu.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MakeBookmarksBiDirectional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plants_Users_UserId",
                table: "Plants");

            migrationBuilder.DropIndex(
                name: "IX_Plants_UserId",
                table: "Plants");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Plants");

            migrationBuilder.CreateTable(
                name: "UserPlantBookmarks",
                columns: table => new
                {
                    BookmarkedPlantsId = table.Column<Guid>(type: "uuid", nullable: false),
                    BookmarksId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPlantBookmarks", x => new { x.BookmarkedPlantsId, x.BookmarksId });
                    table.ForeignKey(
                        name: "FK_UserPlantBookmarks_Plants_BookmarkedPlantsId",
                        column: x => x.BookmarkedPlantsId,
                        principalTable: "Plants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPlantBookmarks_Users_BookmarksId",
                        column: x => x.BookmarksId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPlantBookmarks_BookmarksId",
                table: "UserPlantBookmarks",
                column: "BookmarksId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPlantBookmarks");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Plants",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Plants_UserId",
                table: "Plants",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Plants_Users_UserId",
                table: "Plants",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
