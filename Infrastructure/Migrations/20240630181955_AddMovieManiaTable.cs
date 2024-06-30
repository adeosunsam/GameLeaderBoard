using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddMovieManiaTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeaderBoards");

            migrationBuilder.CreateTable(
                name: "MovieManiaLeaderBoards",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    PlayerId = table.Column<string>(type: "text", nullable: false),
                    PlayerName = table.Column<string>(type: "text", nullable: false),
                    PlayerScore = table.Column<long>(type: "bigint", nullable: false),
                    MovieId = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieManiaLeaderBoards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RampageArenaLeaderBoards",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    PlayerName = table.Column<string>(type: "text", nullable: false),
                    PlayerScore = table.Column<long>(type: "bigint", nullable: false),
                    Rank = table.Column<int>(type: "integer", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RampageArenaLeaderBoards", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovieManiaLeaderBoards_MovieId",
                table: "MovieManiaLeaderBoards",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieManiaLeaderBoards_PlayerId",
                table: "MovieManiaLeaderBoards",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_RampageArenaLeaderBoards_PlayerName",
                table: "RampageArenaLeaderBoards",
                column: "PlayerName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovieManiaLeaderBoards");

            migrationBuilder.DropTable(
                name: "RampageArenaLeaderBoards");

            migrationBuilder.CreateTable(
                name: "LeaderBoards",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PlayerName = table.Column<string>(type: "text", nullable: false),
                    PlayerScore = table.Column<long>(type: "bigint", nullable: false),
                    Rank = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaderBoards", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeaderBoards_PlayerName",
                table: "LeaderBoards",
                column: "PlayerName",
                unique: true);
        }
    }
}
