using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantPalace.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addmultipleimageforProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageOne",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageThree",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageTwo",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ImageOne", "ImageThree", "ImageTwo", "ImageUrl" },
                values: new object[] { "https://www.plantsguru.com/image/cache/Aquatic/plantsguru-adenium-light-of-sun-and-moon-400x400.jpg", "https://www.plantsguru.com/image/cache/catalog/Desertrose/pg-adenium-phet-mong-kon-400x400.jpg", "https://www.plantsguru.com/image/cache/catalog/Desertrose/pg-adenium-phet-mong-kon-400x400.jpg", "https://www.plantsguru.com/image/cache/pg-flowering-plants-175x175.jpg" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ImageOne", "ImageThree", "ImageTwo", "ImageUrl" },
                values: new object[] { "https://www.plantsguru.com/image/cache/Aquatic/plantsguru-adenium-light-of-sun-and-moon-400x400.jpg", "https://www.plantsguru.com/image/cache/catalog/Desertrose/pg-adenium-phet-mong-kon-400x400.jpg", "https://www.plantsguru.com/image/cache/catalog/Desertrose/pg-adenium-phet-mong-kon-400x400.jpg", "https://www.plantsguru.com/image/cache/pg-flowering-plants-175x175.jpg" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ImageOne", "ImageThree", "ImageTwo", "ImageUrl" },
                values: new object[] { "https://www.plantsguru.com/image/cache/Aquatic/plantsguru-adenium-light-of-sun-and-moon-400x400.jpg", "https://www.plantsguru.com/image/cache/catalog/Desertrose/pg-adenium-phet-mong-kon-400x400.jpg", "https://www.plantsguru.com/image/cache/catalog/Desertrose/pg-adenium-phet-mong-kon-400x400.jpg", "https://www.plantsguru.com/image/cache/pg-flowering-plants-175x175.jpg" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ImageOne", "ImageThree", "ImageTwo", "ImageUrl" },
                values: new object[] { "https://www.plantsguru.com/image/cache/Aquatic/plantsguru-adenium-light-of-sun-and-moon-400x400.jpg", "https://www.plantsguru.com/image/cache/catalog/Desertrose/pg-adenium-phet-mong-kon-400x400.jpg", "https://www.plantsguru.com/image/cache/catalog/Desertrose/pg-adenium-phet-mong-kon-400x400.jpg", "https://www.plantsguru.com/image/cache/pg-flowering-plants-175x175.jpg" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ImageOne", "ImageThree", "ImageTwo", "ImageUrl" },
                values: new object[] { "https://www.plantsguru.com/image/cache/Aquatic/plantsguru-adenium-light-of-sun-and-moon-400x400.jpg", "https://www.plantsguru.com/image/cache/catalog/Desertrose/pg-adenium-phet-mong-kon-400x400.jpg", "https://www.plantsguru.com/image/cache/catalog/Desertrose/pg-adenium-phet-mong-kon-400x400.jpg", "https://www.plantsguru.com/image/cache/pg-flowering-plants-175x175.jpg" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ImageOne", "ImageThree", "ImageTwo" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "ImageOne", "ImageThree", "ImageTwo", "ImageUrl" },
                values: new object[] { "https://www.plantsguru.com/image/cache/Aquatic/plantsguru-adenium-light-of-sun-and-moon-400x400.jpg", "https://www.plantsguru.com/image/cache/catalog/Desertrose/pg-adenium-phet-mong-kon-400x400.jpg", "https://www.plantsguru.com/image/cache/catalog/Desertrose/pg-adenium-phet-mong-kon-400x400.jpg", "https://www.plantsguru.com/image/cache/pg-flowering-plants-175x175.jpg" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "ImageOne", "ImageThree", "ImageTwo", "ImageUrl" },
                values: new object[] { "https://www.plantsguru.com/image/cache/Aquatic/plantsguru-adenium-light-of-sun-and-moon-400x400.jpg", "https://www.plantsguru.com/image/cache/catalog/Desertrose/pg-adenium-phet-mong-kon-400x400.jpg", "https://www.plantsguru.com/image/cache/catalog/Desertrose/pg-adenium-phet-mong-kon-400x400.jpg", "https://www.plantsguru.com/image/cache/pg-flowering-plants-175x175.jpg" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "ImageOne", "ImageThree", "ImageTwo", "ImageUrl" },
                values: new object[] { "https://www.plantsguru.com/image/cache/Aquatic/plantsguru-adenium-light-of-sun-and-moon-400x400.jpg", "https://www.plantsguru.com/image/cache/catalog/Desertrose/pg-adenium-phet-mong-kon-400x400.jpg", "https://www.plantsguru.com/image/cache/catalog/Desertrose/pg-adenium-phet-mong-kon-400x400.jpg", "https://www.plantsguru.com/image/cache/pg-flowering-plants-175x175.jpg" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "ImageOne", "ImageThree", "ImageTwo", "ImageUrl" },
                values: new object[] { "https://www.plantsguru.com/image/cache/Aquatic/plantsguru-adenium-light-of-sun-and-moon-400x400.jpg", "https://www.plantsguru.com/image/cache/catalog/Desertrose/pg-adenium-phet-mong-kon-400x400.jpg", "https://www.plantsguru.com/image/cache/catalog/Desertrose/pg-adenium-phet-mong-kon-400x400.jpg", "https://www.plantsguru.com/image/cache/pg-flowering-plants-175x175.jpg" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageOne",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ImageThree",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ImageTwo",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImageUrl",
                value: "");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "ImageUrl",
                value: "");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                column: "ImageUrl",
                value: "");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                column: "ImageUrl",
                value: "");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                column: "ImageUrl",
                value: "");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                column: "ImageUrl",
                value: "");
        }
    }
}
