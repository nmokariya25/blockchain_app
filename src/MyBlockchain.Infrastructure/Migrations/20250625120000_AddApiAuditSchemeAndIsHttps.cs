using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBlockchain.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddApiAuditSchemeAndIsHttps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHttps",
                table: "ApiAudits",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Scheme",
                table: "ApiAudits",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHttps",
                table: "ApiAudits");

            migrationBuilder.DropColumn(
                name: "Scheme",
                table: "ApiAudits");
        }
    }
}
