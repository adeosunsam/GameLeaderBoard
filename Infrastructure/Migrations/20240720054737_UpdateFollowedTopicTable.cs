using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class UpdateFollowedTopicTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FollowedTopics_AppUsers_AppUserId",
                table: "FollowedTopics");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "FollowedTopics",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_FollowedTopics_AppUserId",
                table: "FollowedTopics",
                newName: "IX_FollowedTopics_UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "FollowedTopics",
                newName: "AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_FollowedTopics_UserId",
                table: "FollowedTopics",
                newName: "IX_FollowedTopics_AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FollowedTopics_AppUsers_AppUserId",
                table: "FollowedTopics",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
