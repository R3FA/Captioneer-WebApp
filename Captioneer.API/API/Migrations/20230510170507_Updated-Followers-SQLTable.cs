using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Captioneer.API.Migrations
{
    public partial class UpdatedFollowersSQLTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Followers",
                newName: "FollowingCreatedAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "FollowerCreatedAt",
                table: "Followers",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FollowerCreatedAt",
                table: "Followers");

            migrationBuilder.RenameColumn(
                name: "FollowingCreatedAt",
                table: "Followers",
                newName: "CreatedAt");
        }
    }
}
