using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Captioneer.API.Migrations
{
    public partial class AddedisBannedPropertyInUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isBanned",
                table: "Users",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isBanned",
                table: "Users");
        }
    }
}
