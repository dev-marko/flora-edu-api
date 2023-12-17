using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FloraEdu.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddNewUniquePlantVisitorEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UniqueArticleVisitor_Articles_ArticleId",
                table: "UniqueArticleVisitor");

            migrationBuilder.DropForeignKey(
                name: "FK_UniqueArticleVisitor_Users_UserId",
                table: "UniqueArticleVisitor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UniqueArticleVisitor",
                table: "UniqueArticleVisitor");

            migrationBuilder.RenameTable(
                name: "UniqueArticleVisitor",
                newName: "UniquePlantVisitors");

            migrationBuilder.RenameIndex(
                name: "IX_UniqueArticleVisitor_UserId",
                table: "UniquePlantVisitors",
                newName: "IX_UniquePlantVisitors_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UniqueArticleVisitor_ArticleId",
                table: "UniquePlantVisitors",
                newName: "IX_UniquePlantVisitors_ArticleId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastModified",
                table: "UniquePlantVisitors",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UniquePlantVisitors",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "UniquePlantVisitors",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "UniquePlantVisitors",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuid_generate_v4()",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UniquePlantVisitors",
                table: "UniquePlantVisitors",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UniquePlantVisitor",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UUAID = table.Column<Guid>(type: "uuid", nullable: false),
                    PlantId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    IsAnonymous = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
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

            migrationBuilder.AddForeignKey(
                name: "FK_UniquePlantVisitors_Users_UserId",
                table: "UniquePlantVisitors",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UniquePlantVisitors_Articles_ArticleId",
                table: "UniquePlantVisitors");

            migrationBuilder.DropForeignKey(
                name: "FK_UniquePlantVisitors_Users_UserId",
                table: "UniquePlantVisitors");

            migrationBuilder.DropTable(
                name: "UniquePlantVisitor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UniquePlantVisitors",
                table: "UniquePlantVisitors");

            migrationBuilder.RenameTable(
                name: "UniquePlantVisitors",
                newName: "UniqueArticleVisitor");

            migrationBuilder.RenameIndex(
                name: "IX_UniquePlantVisitors_UserId",
                table: "UniqueArticleVisitor",
                newName: "IX_UniqueArticleVisitor_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UniquePlantVisitors_ArticleId",
                table: "UniqueArticleVisitor",
                newName: "IX_UniqueArticleVisitor_ArticleId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastModified",
                table: "UniqueArticleVisitor",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "now() at time zone 'utc'");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UniqueArticleVisitor",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "UniqueArticleVisitor",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "now() at time zone 'utc'");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "UniqueArticleVisitor",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "uuid_generate_v4()");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UniqueArticleVisitor",
                table: "UniqueArticleVisitor",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UniqueArticleVisitor_Articles_ArticleId",
                table: "UniqueArticleVisitor",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UniqueArticleVisitor_Users_UserId",
                table: "UniqueArticleVisitor",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
