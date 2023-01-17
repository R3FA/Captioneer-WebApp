using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Captioneer.API.Migrations
{
    public partial class SubtitleMovie : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FrameRate",
                table: "SubtitleMovies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Release",
                table: "SubtitleMovies",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FrameRate",
                table: "SubtitleMovies");

            migrationBuilder.DropColumn(
                name: "Release",
                table: "SubtitleMovies");
        }
    }
}
