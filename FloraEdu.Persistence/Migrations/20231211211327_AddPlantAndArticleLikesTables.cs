using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FloraEdu.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPlantAndArticleLikesTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArticleCommentLikes",
                columns: table => new
                {
                    LikedArticleCommentsId = table.Column<Guid>(type: "uuid", nullable: false),
                    LikesId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleCommentLikes", x => new { x.LikedArticleCommentsId, x.LikesId });
                    table.ForeignKey(
                        name: "FK_ArticleCommentLikes_ArticleComments_LikedArticleCommentsId",
                        column: x => x.LikedArticleCommentsId,
                        principalTable: "ArticleComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleCommentLikes_Users_LikesId",
                        column: x => x.LikesId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlantCommentLikes",
                columns: table => new
                {
                    LikedPlantCommentsId = table.Column<Guid>(type: "uuid", nullable: false),
                    LikesId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlantCommentLikes", x => new { x.LikedPlantCommentsId, x.LikesId });
                    table.ForeignKey(
                        name: "FK_PlantCommentLikes_PlantComments_LikedPlantCommentsId",
                        column: x => x.LikedPlantCommentsId,
                        principalTable: "PlantComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlantCommentLikes_Users_LikesId",
                        column: x => x.LikesId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleCommentLikes_LikesId",
                table: "ArticleCommentLikes",
                column: "LikesId");

            migrationBuilder.CreateIndex(
                name: "IX_PlantCommentLikes_LikesId",
                table: "PlantCommentLikes",
                column: "LikesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleCommentLikes");

            migrationBuilder.DropTable(
                name: "PlantCommentLikes");
        }
    }
}
