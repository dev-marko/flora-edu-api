using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FloraEdu.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPlantImageEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:plant_type", "unknown,fruit,vegetable,tree,flower,herbs,other");

            migrationBuilder.CreateTable(
                name: "PlantImageUrls",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlantId = table.Column<Guid>(type: "uuid", nullable: false),
                    ThumbnailImageUrl = table.Column<string>(type: "text", nullable: true),
                    HeaderImageUrls = table.Column<List<string>>(type: "text[]", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlantImageUrls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlantImageUrls_Plants_PlantId",
                        column: x => x.PlantId,
                        principalTable: "Plants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlantImageUrls_PlantId",
                table: "PlantImageUrls",
                column: "PlantId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlantImageUrls");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:Enum:plant_type", "unknown,fruit,vegetable,tree,flower,herbs,other");
        }
    }
}
