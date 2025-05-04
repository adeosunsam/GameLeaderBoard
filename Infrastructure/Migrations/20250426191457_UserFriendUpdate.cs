using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class UserFriendUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFriends_AppUsers_AppUserId",
                table: "UserFriends");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "UserFriends",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserFriends_AppUserId",
                table: "UserFriends",
                newName: "IX_UserFriends_UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserFriends",
                newName: "AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserFriends_UserId",
                table: "UserFriends",
                newName: "IX_UserFriends_AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFriends_AppUsers_AppUserId",
                table: "UserFriends",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
