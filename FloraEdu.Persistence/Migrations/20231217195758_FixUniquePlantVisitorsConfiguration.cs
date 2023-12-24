using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FloraEdu.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixUniquePlantVisitorsConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UniquePlantVisitors_Articles_ArticleId",
                table: "UniquePlantVisitors");

            migrationBuilder.DropTable(
                name: "UniquePlantVisitor");

            migrationBuilder.RenameColumn(
                name: "ArticleId",
                table: "UniquePlantVisitors",
                newName: "PlantId");

            migrationBuilder.RenameIndex(
                name: "IX_UniquePlantVisitors_ArticleId",
                table: "UniquePlantVisitors",
                newName: "IX_UniquePlantVisitors_PlantId");

            migrationBuilder.CreateTable(
                name: "UniqueArticleVisitors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    UUAID = table.Column<Guid>(type: "uuid", nullable: false),
                    ArticleId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    IsAnonymous = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UniqueArticleVisitors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UniqueArticleVisitors_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UniqueArticleVisitors_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UniqueArticleVisitors_ArticleId",
                table: "UniqueArticleVisitors",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_UniqueArticleVisitors_UserId",
                table: "UniqueArticleVisitors",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UniquePlantVisitors_Plants_PlantId",
                table: "UniquePlantVisitors",
                column: "PlantId",
                principalTable: "Plants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UniquePlantVisitors_Plants_PlantId",
                table: "UniquePlantVisitors");

            migrationBuilder.DropTable(
                name: "UniqueArticleVisitors");

            migrationBuilder.RenameColumn(
                name: "PlantId",
                table: "UniquePlantVisitors",
                newName: "ArticleId");

            migrationBuilder.RenameIndex(
                name: "IX_UniquePlantVisitors_PlantId",
                table: "UniquePlantVisitors",
                newName: "IX_UniquePlantVisitors_ArticleId");

            migrationBuilder.CreateTable(
                name: "UniquePlantVisitor",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlantId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsAnonymous = table.Column<bool>(type: "boolean", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UUAID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UniquePlantVisitor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UniquePlantVisitor_Plants_PlantId",
                        column: x => x.PlantId,
                        principalTable: "Plants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UniquePlantVisitor_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UniquePlantVisitor_PlantId",
                table: "UniquePlantVisitor",
                column: "PlantId");

            migrationBuilder.CreateIndex(
                name: "IX_UniquePlantVisitor_UserId",
                table: "UniquePlantVisitor",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UniquePlantVisitors_Articles_ArticleId",
                table: "UniquePlantVisitors",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
