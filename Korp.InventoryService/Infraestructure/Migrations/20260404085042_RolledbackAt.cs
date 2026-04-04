using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Korp.InventoryService.Migrations
{
    /// <inheritdoc />
    public partial class RolledbackAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RolledbackAt",
                table: "ReservedProducts",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RolledbackAt",
                table: "ReservedProducts");
        }
    }
}
