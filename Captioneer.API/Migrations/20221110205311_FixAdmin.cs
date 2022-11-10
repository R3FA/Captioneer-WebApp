using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Captioneer.API.Migrations
{
    public partial class FixAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_AdminID",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_SubtitleMovies_Users_AdminID",
                table: "SubtitleMovies");

            migrationBuilder.DropForeignKey(
                name: "FK_SubtitleTVShows_Users_AdminID",
                table: "SubtitleTVShows");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_AdminID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Admins_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Admins_UserID",
                table: "Admins",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Admins_AdminID",
                table: "Comments",
                column: "AdminID",
                principalTable: "Admins",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_SubtitleMovies_Admins_AdminID",
                table: "SubtitleMovies",
                column: "AdminID",
                principalTable: "Admins",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_SubtitleTVShows_Admins_AdminID",
                table: "SubtitleTVShows",
                column: "AdminID",
                principalTable: "Admins",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Admins_AdminID",
                table: "Users",
                column: "AdminID",
                principalTable: "Admins",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Admins_AdminID",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_SubtitleMovies_Admins_AdminID",
                table: "SubtitleMovies");

            migrationBuilder.DropForeignKey(
                name: "FK_SubtitleTVShows_Admins_AdminID",
                table: "SubtitleTVShows");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Admins_AdminID",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Users",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_AdminID",
                table: "Comments",
                column: "AdminID",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_SubtitleMovies_Users_AdminID",
                table: "SubtitleMovies",
                column: "AdminID",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_SubtitleTVShows_Users_AdminID",
                table: "SubtitleTVShows",
                column: "AdminID",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_AdminID",
                table: "Users",
                column: "AdminID",
                principalTable: "Users",
                principalColumn: "ID");
        }
    }
}
