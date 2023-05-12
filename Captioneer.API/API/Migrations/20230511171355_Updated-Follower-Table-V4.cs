using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Captioneer.API.Migrations
{
    public partial class UpdatedFollowerTableV4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Followers_Users_UserFollowingId",
                table: "Followers");

            migrationBuilder.DropColumn(
                name: "FollowerCreatedAt",
                table: "Followers");

            migrationBuilder.AlterColumn<int>(
                name: "UserFollowingId",
                table: "Followers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FollowingCreatedAt",
                table: "Followers",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Followers_Users_UserFollowingId",
                table: "Followers",
                column: "UserFollowingId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Followers_Users_UserFollowingId",
                table: "Followers");

            migrationBuilder.AlterColumn<int>(
                name: "UserFollowingId",
                table: "Followers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FollowingCreatedAt",
                table: "Followers",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AddColumn<DateTime>(
                name: "FollowerCreatedAt",
                table: "Followers",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Followers_Users_UserFollowingId",
                table: "Followers",
                column: "UserFollowingId",
                principalTable: "Users",
                principalColumn: "ID");
        }
    }
}
