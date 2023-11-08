using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PlantPalace.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class BannersTabel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BannerUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BannerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banners", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Banners",
                columns: new[] { "Id", "BannerName", "BannerUrl", "Description" },
                values: new object[,]
                {
                    { 1, "Banner 1", "/images/banners/Up to 70% off .jpg", "Description for Banner 1" },
                    { 2, "Banner 2", "/images/banners/Zephyranthes Bulbs.jpg", "Description for Banner 2" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Banners");
        }
    }
}
