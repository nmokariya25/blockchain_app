using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBlockchain.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class eth_block_tbl_create : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EthBlocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Height = table.Column<long>(type: "INTEGER", nullable: false),
                    Hash = table.Column<string>(type: "TEXT", nullable: false),
                    Time = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LatestUrl = table.Column<string>(type: "TEXT", nullable: false),
                    PreviousHash = table.Column<string>(type: "TEXT", nullable: false),
                    PreviousUrl = table.Column<string>(type: "TEXT", nullable: false),
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
                    LastForkHash = table.Column<string>(type: "TEXT", nullable: false),
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
                name: "EthBlocks");
        }
    }
}
