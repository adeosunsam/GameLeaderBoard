using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class UserGamingNumberUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserGamingNumbers_AppUsers_AppUserId",
                table: "UserGamingNumbers");

            migrationBuilder.DropIndex(
                name: "IX_UserGamingNumbers_AppUserId",
                table: "UserGamingNumbers");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "UserGamingNumbers",
                newName: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGamingNumbers_UserId",
                table: "UserGamingNumbers",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserGamingNumbers_UserId",
                table: "UserGamingNumbers");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserGamingNumbers",
                newName: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGamingNumbers_AppUserId",
                table: "UserGamingNumbers",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserGamingNumbers_AppUsers_AppUserId",
                table: "UserGamingNumbers",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
