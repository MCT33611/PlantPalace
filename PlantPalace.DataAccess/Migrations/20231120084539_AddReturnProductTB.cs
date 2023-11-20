using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantPalace.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddReturnProductTB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductReturn",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderDetailId = table.Column<int>(type: "int", nullable: false),
                    ReturnStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReturnReason = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductReturn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductReturn_OrderDetail_OrderDetailId",
                        column: x => x.OrderDetailId,
                        principalTable: "OrderDetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductReturn_OrderDetailId",
                table: "ProductReturn",
                column: "OrderDetailId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductReturn");
        }
    }
}
