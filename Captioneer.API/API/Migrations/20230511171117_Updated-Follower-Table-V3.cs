using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Captioneer.API.Migrations
{
    public partial class UpdatedFollowerTableV3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Followers_Users_UserFollowerId",
                table: "Followers");

            migrationBuilder.DropIndex(
                name: "IX_Followers_UserFollowerId",
                table: "Followers");

            migrationBuilder.DropColumn(
                name: "UserFollowerId",
                table: "Followers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserFollowerId",
                table: "Followers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Followers_UserFollowerId",
                table: "Followers",
                column: "UserFollowerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Followers_Users_UserFollowerId",
                table: "Followers",
                column: "UserFollowerId",
                principalTable: "Users",
                principalColumn: "ID");
        }
    }
}
