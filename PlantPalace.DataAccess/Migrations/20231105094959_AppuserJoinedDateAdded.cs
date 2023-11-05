using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantPalace.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AppuserJoinedDateAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "joinDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "joinDate",
                table: "AspNetUsers");
        }
    }
}
