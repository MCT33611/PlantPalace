using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantPalace.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ProductTBAddedColOffer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offer_Products_ProductId",
                table: "Offer");

            migrationBuilder.DropIndex(
                name: "IX_Offer_ProductId",
                table: "Offer");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Offer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Offer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Offer_ProductId",
                table: "Offer",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Offer_Products_ProductId",
                table: "Offer",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
