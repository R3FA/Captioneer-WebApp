using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Captioneer.API.Migrations
{
    public partial class FixActorTVShowFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActorTVShows_TVShows_MovieID",
                table: "ActorTVShows");

            migrationBuilder.DropIndex(
                name: "IX_ActorTVShows_MovieID",
                table: "ActorTVShows");

            migrationBuilder.DropColumn(
                name: "MovieID",
                table: "ActorTVShows");

            migrationBuilder.CreateIndex(
                name: "IX_ActorTVShows_TVShowID",
                table: "ActorTVShows",
                column: "TVShowID");

            migrationBuilder.AddForeignKey(
                name: "FK_ActorTVShows_TVShows_TVShowID",
                table: "ActorTVShows",
                column: "TVShowID",
                principalTable: "TVShows",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActorTVShows_TVShows_TVShowID",
                table: "ActorTVShows");

            migrationBuilder.DropIndex(
                name: "IX_ActorTVShows_TVShowID",
                table: "ActorTVShows");

            migrationBuilder.AddColumn<int>(
                name: "MovieID",
                table: "ActorTVShows",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ActorTVShows_MovieID",
                table: "ActorTVShows",
                column: "MovieID");

            migrationBuilder.AddForeignKey(
                name: "FK_ActorTVShows_TVShows_MovieID",
                table: "ActorTVShows",
                column: "MovieID",
                principalTable: "TVShows",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
