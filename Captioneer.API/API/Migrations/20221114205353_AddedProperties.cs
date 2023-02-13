using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Captioneer.API.Migrations
{
    public partial class AddedProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RatingValue",
                table: "TVShows",
                newName: "IMDBRatingValue");

            migrationBuilder.RenameColumn(
                name: "RatingCount",
                table: "TVShows",
                newName: "IMDBRatingCount");

            migrationBuilder.RenameColumn(
                name: "RatingValue",
                table: "Movies",
                newName: "IMDBRatingValue");

            migrationBuilder.RenameColumn(
                name: "RatingCount",
                table: "Movies",
                newName: "IMDBRatingCount");

            migrationBuilder.AddColumn<string>(
                name: "MetacriticValue",
                table: "TVShows",
                type: "varchar(7)",
                maxLength: 7,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "RottenTomatoesValue",
                table: "TVShows",
                type: "varchar(3)",
                maxLength: 3,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "MetacriticValue",
                table: "Movies",
                type: "varchar(7)",
                maxLength: 7,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "RottenTomatoesValue",
                table: "Movies",
                type: "varchar(3)",
                maxLength: 3,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MetacriticValue",
                table: "TVShows");

            migrationBuilder.DropColumn(
                name: "RottenTomatoesValue",
                table: "TVShows");

            migrationBuilder.DropColumn(
                name: "MetacriticValue",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "RottenTomatoesValue",
                table: "Movies");

            migrationBuilder.RenameColumn(
                name: "IMDBRatingValue",
                table: "TVShows",
                newName: "RatingValue");

            migrationBuilder.RenameColumn(
                name: "IMDBRatingCount",
                table: "TVShows",
                newName: "RatingCount");

            migrationBuilder.RenameColumn(
                name: "IMDBRatingValue",
                table: "Movies",
                newName: "RatingValue");

            migrationBuilder.RenameColumn(
                name: "IMDBRatingCount",
                table: "Movies",
                newName: "RatingCount");
        }
    }
}
