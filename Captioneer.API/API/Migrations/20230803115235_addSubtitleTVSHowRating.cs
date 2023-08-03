using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Captioneer.API.Migrations
{
    public partial class addSubtitleTVSHowRating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<int>(
                name: "FrameRate",
                table: "subtitletvshows",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Release",
                table: "subtitletvshows",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
