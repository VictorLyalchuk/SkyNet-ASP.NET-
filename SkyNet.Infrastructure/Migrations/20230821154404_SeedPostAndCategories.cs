using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SkyNet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedPostAndCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Text = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PublishDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Posts_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 1, "Car" },
                    { 2, "Girl" },
                    { 3, "Nature" }
                });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "ID", "CategoryId", "Description", "Image", "PublishDate", "Text" },
                values: new object[,]
                {
                    { 1, 1, "Test", "Car_1", new DateTime(2023, 8, 21, 18, 44, 4, 75, DateTimeKind.Local).AddTicks(5615), "Test" },
                    { 2, 1, "Test", "Car_2", new DateTime(2023, 8, 21, 18, 44, 4, 75, DateTimeKind.Local).AddTicks(5673), "Test" },
                    { 3, 2, "Test", "Girl_1", new DateTime(2023, 8, 21, 18, 44, 4, 75, DateTimeKind.Local).AddTicks(5677), "Test" },
                    { 4, 2, "Test", "Girl_2", new DateTime(2023, 8, 21, 18, 44, 4, 75, DateTimeKind.Local).AddTicks(5679), "Test" },
                    { 5, 3, "Test", "Nature_1", new DateTime(2023, 8, 21, 18, 44, 4, 75, DateTimeKind.Local).AddTicks(5682), "Test" },
                    { 6, 3, "Test", "Nature_2", new DateTime(2023, 8, 21, 18, 44, 4, 75, DateTimeKind.Local).AddTicks(5684), "Test" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CategoryId",
                table: "Posts",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
