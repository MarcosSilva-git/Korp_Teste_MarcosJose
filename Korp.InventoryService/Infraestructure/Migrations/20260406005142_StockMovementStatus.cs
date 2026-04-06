using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Korp.InventoryService.Migrations
{
    /// <inheritdoc />
    public partial class StockMovementStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "ReservedProducts");

            migrationBuilder.DropColumn(
                name: "RolledbackAt",
                table: "ReservedProducts");

            migrationBuilder.AddColumn<string>(
                name: "StockMovementStatus",
                table: "ReservedProducts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ReservedProducts_ProductId",
                table: "ReservedProducts",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservedProducts_Products_ProductId",
                table: "ReservedProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservedProducts_Products_ProductId",
                table: "ReservedProducts");

            migrationBuilder.DropIndex(
                name: "IX_ReservedProducts_ProductId",
                table: "ReservedProducts");

            migrationBuilder.DropColumn(
                name: "StockMovementStatus",
                table: "ReservedProducts");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "ReservedProducts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RolledbackAt",
                table: "ReservedProducts",
                type: "TEXT",
                nullable: true);
        }
    }
}
