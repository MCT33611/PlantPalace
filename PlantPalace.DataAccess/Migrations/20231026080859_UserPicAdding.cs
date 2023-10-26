using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantPalace.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UserPicAdding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Pic",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pic",
                table: "AspNetUsers");
        }
    }
}
