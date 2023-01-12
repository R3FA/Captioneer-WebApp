using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Captioneer.API.Migrations
{
    public partial class NullableCommentFKs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_SubtitleMovies_SubtitleMovieID",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_SubtitleTVShows_SubtitleTVShowID",
                table: "Comments");

            migrationBuilder.AlterColumn<int>(
                name: "SubtitleTVShowID",
                table: "Comments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "SubtitleMovieID",
                table: "Comments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_SubtitleMovies_SubtitleMovieID",
                table: "Comments",
                column: "SubtitleMovieID",
                principalTable: "SubtitleMovies",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_SubtitleTVShows_SubtitleTVShowID",
                table: "Comments",
                column: "SubtitleTVShowID",
                principalTable: "SubtitleTVShows",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_SubtitleMovies_SubtitleMovieID",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_SubtitleTVShows_SubtitleTVShowID",
                table: "Comments");

            migrationBuilder.AlterColumn<int>(
                name: "SubtitleTVShowID",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SubtitleMovieID",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_SubtitleMovies_SubtitleMovieID",
                table: "Comments",
                column: "SubtitleMovieID",
                principalTable: "SubtitleMovies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_SubtitleTVShows_SubtitleTVShowID",
                table: "Comments",
                column: "SubtitleTVShowID",
                principalTable: "SubtitleTVShows",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
