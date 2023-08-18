using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Captioneer.API.Migrations
{
    public partial class adminclassupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RemovedCommentsNumber",
                table: "admins",
                type: "int",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "BannedUsersNumber",
                table: "admins",
                type: "int",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "RemovedMovieSubtitlesNumber",
                table: "admins",
                type: "int",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "RemovedTVShowSubtitlesNumber",
                table: "admins",
                type: "int",
                nullable: false);
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
