using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBlockchain.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initial_create_block_cypher_db : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApiAudits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HttpMethod = table.Column<string>(type: "TEXT", nullable: false),
                    Path = table.Column<string>(type: "TEXT", nullable: false),
                    QueryString = table.Column<string>(type: "TEXT", nullable: false),
                    RequestBody = table.Column<string>(type: "TEXT", nullable: false),
                    StatusCode = table.Column<int>(type: "INTEGER", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ResponseDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiAudits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EthBlocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Height = table.Column<long>(type: "INTEGER", nullable: false),
                    Hash = table.Column<string>(type: "TEXT", nullable: false),
                    Time = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LatestUrl = table.Column<string>(type: "TEXT", nullable: true),
                    PreviousHash = table.Column<string>(type: "TEXT", nullable: true),
                    PreviousUrl = table.Column<string>(type: "TEXT", nullable: true),
                    PeerCount = table.Column<int>(type: "INTEGER", nullable: false),
                    UnconfirmedCount = table.Column<int>(type: "INTEGER", nullable: false),
                    HighGasPrice = table.Column<long>(type: "INTEGER", nullable: false),
                    MediumGasPrice = table.Column<long>(type: "INTEGER", nullable: false),
                    LowGasPrice = table.Column<long>(type: "INTEGER", nullable: false),
                    HighPriorityFee = table.Column<long>(type: "INTEGER", nullable: false),
                    MediumPriorityFee = table.Column<long>(type: "INTEGER", nullable: false),
                    LowPriorityFee = table.Column<long>(type: "INTEGER", nullable: false),
                    BaseFee = table.Column<long>(type: "INTEGER", nullable: false),
                    LastForkHeight = table.Column<long>(type: "INTEGER", nullable: false),
                    LastForkHash = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EthBlocks", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiAudits");

            migrationBuilder.DropTable(
                name: "EthBlocks");
        }
    }
}
