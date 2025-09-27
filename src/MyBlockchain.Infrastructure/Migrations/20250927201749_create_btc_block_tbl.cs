﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBlockchain.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class create_btc_block_tbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BtcBlocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Height = table.Column<int>(type: "INTEGER", nullable: false),
                    Hash = table.Column<string>(type: "TEXT", nullable: false),
                    Time = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LatestUrl = table.Column<string>(type: "TEXT", nullable: false),
                    PreviousHash = table.Column<string>(type: "TEXT", nullable: false),
                    PreviousUrl = table.Column<string>(type: "TEXT", nullable: false),
                    PeerCount = table.Column<int>(type: "INTEGER", nullable: false),
                    UnconfirmedCount = table.Column<int>(type: "INTEGER", nullable: false),
                    HighFeePerKb = table.Column<int>(type: "INTEGER", nullable: false),
                    MediumFeePerKb = table.Column<int>(type: "INTEGER", nullable: false),
                    LowFeePerKb = table.Column<int>(type: "INTEGER", nullable: false),
                    LastForkHeight = table.Column<int>(type: "INTEGER", nullable: false),
                    LastForkHash = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BtcBlocks", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BtcBlocks");
        }
    }
}
