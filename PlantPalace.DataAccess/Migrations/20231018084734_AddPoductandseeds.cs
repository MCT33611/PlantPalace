using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PlantPalace.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddPoductandseeds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categorries",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ListPrice = table.Column<double>(type: "float", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Price50 = table.Column<double>(type: "float", nullable: false),
                    Price100 = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "ListPrice", "Name", "Price", "Price100", "Price50" },
                values: new object[,]
                {
                    { 1, 100.0, "Product 1", 95.0, 85.0, 90.75 },
                    { 2, 100.0, "Product 2", 95.0, 85.0, 90.75 },
                    { 3, 100.0, "Product 3", 95.0, 85.0, 90.75 },
                    { 4, 100.0, "Product 4", 95.0, 85.0, 90.75 },
                    { 5, 100.0, "Product 5", 95.0, 85.0, 90.75 },
                    { 6, 100.0, "Product 6", 95.0, 85.0, 90.75 },
                    { 7, 100.0, "Product 7", 95.0, 85.0, 90.75 },
                    { 8, 100.0, "Product 8", 95.0, 85.0, 90.75 },
                    { 9, 100.0, "Product 9", 95.0, 85.0, 90.75 },
                    { 10, 100.0, "Product 10", 95.0, 85.0, 90.75 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categorries",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);
        }
    }
}
