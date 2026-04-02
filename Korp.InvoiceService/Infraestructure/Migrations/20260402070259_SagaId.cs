using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Korp.InvoiceService.Migrations
{
    /// <inheritdoc />
    public partial class SagaId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SagaId",
                table: "Invoices",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SagaId",
                table: "Invoices");
        }
    }
}
