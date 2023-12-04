using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FloraEdu.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddLikesAndBookmarksToArticleEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShortDescription",
                table: "Articles",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "UserArticleBookmarks",
                columns: table => new
                {
                    BookmarkedArticlesId = table.Column<Guid>(type: "uuid", nullable: false),
                    BookmarksId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserArticleBookmarks", x => new { x.BookmarkedArticlesId, x.BookmarksId });
                    table.ForeignKey(
                        name: "FK_UserArticleBookmarks_Articles_BookmarkedArticlesId",
                        column: x => x.BookmarkedArticlesId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserArticleBookmarks_Users_BookmarksId",
                        column: x => x.BookmarksId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserArticleLikes",
                columns: table => new
                {
                    LikedArticlesId = table.Column<Guid>(type: "uuid", nullable: false),
                    LikesId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserArticleLikes", x => new { x.LikedArticlesId, x.LikesId });
                    table.ForeignKey(
                        name: "FK_UserArticleLikes_Articles_LikedArticlesId",
                        column: x => x.LikedArticlesId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserArticleLikes_Users_LikesId",
                        column: x => x.LikesId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserArticleBookmarks_BookmarksId",
                table: "UserArticleBookmarks",
                column: "BookmarksId");

            migrationBuilder.CreateIndex(
                name: "IX_UserArticleLikes_LikesId",
                table: "UserArticleLikes",
                column: "LikesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserArticleBookmarks");

            migrationBuilder.DropTable(
                name: "UserArticleLikes");

            migrationBuilder.DropColumn(
                name: "ShortDescription",
                table: "Articles");
        }
    }
}
