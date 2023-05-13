﻿// <auto-generated />
using System;
using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Captioneer.API.Migrations
{
    [DbContext(typeof(CaptioneerDBContext))]
    partial class CaptioneerDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("API.Entities.Actor", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("varchar(25)");

                    b.HasKey("ID");

                    b.ToTable("Actors");
                });

            modelBuilder.Entity("API.Entities.ActorMovie", b =>
                {
                    b.Property<int>("ActorID")
                        .HasColumnType("int");

                    b.Property<int>("MovieID")
                        .HasColumnType("int");

                    b.HasKey("ActorID", "MovieID");

                    b.HasIndex("MovieID");

                    b.ToTable("ActorMovies");
                });

            modelBuilder.Entity("API.Entities.ActorTVShow", b =>
                {
                    b.Property<int>("ActorID")
                        .HasColumnType("int");

                    b.Property<int>("TVShowID")
                        .HasColumnType("int");

                    b.HasKey("ActorID", "TVShowID");

                    b.HasIndex("TVShowID");

                    b.ToTable("ActorTVShows");
                });

            modelBuilder.Entity("API.Entities.Admin", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("API.Entities.Comment", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("AdminID")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<int?>("SubtitleMovieID")
                        .HasColumnType("int");

                    b.Property<int?>("SubtitleTVShowID")
                        .HasColumnType("int");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("AdminID");

                    b.HasIndex("SubtitleMovieID");

                    b.HasIndex("SubtitleTVShowID");

                    b.HasIndex("UserID");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("API.Entities.Creator", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("varchar(25)");

                    b.HasKey("ID");

                    b.ToTable("Creators");
                });

            modelBuilder.Entity("API.Entities.CreatorMovie", b =>
                {
                    b.Property<int>("CreatorID")
                        .HasColumnType("int");

                    b.Property<int>("MovieID")
                        .HasColumnType("int");

                    b.Property<string>("Position")
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.HasKey("CreatorID", "MovieID", "Position");

                    b.HasIndex("MovieID");

                    b.ToTable("CreatorsMovie");
                });

            modelBuilder.Entity("API.Entities.CreatorTVShow", b =>
                {
                    b.Property<int>("CreatorID")
                        .HasColumnType("int");

                    b.Property<int>("TVShowID")
                        .HasColumnType("int");

                    b.Property<string>("Position")
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.HasKey("CreatorID", "TVShowID", "Position");

                    b.HasIndex("TVShowID");

                    b.ToTable("CreatorsTVShows");
                });

            modelBuilder.Entity("API.Entities.DirectMessage", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<int>("RecipientUserID")
                        .HasColumnType("int");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("RecipientUserID");

                    b.HasIndex("UserID");

                    b.ToTable("DirectMessages");
                });

            modelBuilder.Entity("API.Entities.Episode", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("EpisodeNumber")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("SeasonID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("SeasonID");

                    b.ToTable("Episodes");
                });

            modelBuilder.Entity("API.Entities.Follower", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("FollowingCreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("UserFollowingId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserFollowingId");

                    b.HasIndex("UserId");

                    b.ToTable("Followers");
                });

            modelBuilder.Entity("API.Entities.Genre", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.HasKey("ID");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("API.Entities.GenreMovie", b =>
                {
                    b.Property<int>("GenreID")
                        .HasColumnType("int");

                    b.Property<int>("MovieID")
                        .HasColumnType("int");

                    b.HasKey("GenreID", "MovieID");

                    b.HasIndex("MovieID");

                    b.ToTable("GenresMovie");
                });

            modelBuilder.Entity("API.Entities.GenreTVShow", b =>
                {
                    b.Property<int>("GenreID")
                        .HasColumnType("int");

                    b.Property<int>("TVShowID")
                        .HasColumnType("int");

                    b.HasKey("GenreID", "TVShowID");

                    b.HasIndex("TVShowID");

                    b.ToTable("GenresTVShows");
                });

            modelBuilder.Entity("API.Entities.Language", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("EnglishName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Flag")
                        .HasColumnType("longtext");

                    b.Property<string>("LanguageCode")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.ToTable("Languages");
                });

            modelBuilder.Entity("API.Entities.Movie", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CoverArt")
                        .HasColumnType("longtext");

                    b.Property<string>("IMDBId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("IMDBRatingCount")
                        .HasColumnType("int");

                    b.Property<double>("IMDBRatingValue")
                        .HasColumnType("double");

                    b.Property<string>("MetacriticValue")
                        .HasColumnType("longtext");

                    b.Property<string>("RottenTomatoesValue")
                        .HasColumnType("longtext");

                    b.Property<int>("Runtime")
                        .HasColumnType("int");

                    b.Property<string>("Synopsis")
                        .IsRequired()
                        .HasMaxLength(600)
                        .HasColumnType("varchar(600)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Year")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.HasKey("ID");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("API.Entities.Season", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CoverArt")
                        .HasColumnType("longtext");

                    b.Property<int?>("EpisodeCount")
                        .HasColumnType("int");

                    b.Property<int>("SeasonNumber")
                        .HasColumnType("int");

                    b.Property<int>("TVShowID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("TVShowID");

                    b.ToTable("Seasons");
                });

            modelBuilder.Entity("API.Entities.ShootingPlace", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("City")
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("MapsCoordinates")
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.ToTable("ShootingPlaces");
                });

            modelBuilder.Entity("API.Entities.ShootingPlaceMovie", b =>
                {
                    b.Property<int>("ShootingPlaceID")
                        .HasColumnType("int");

                    b.Property<int>("MovieID")
                        .HasColumnType("int");

                    b.HasKey("ShootingPlaceID", "MovieID");

                    b.HasIndex("MovieID");

                    b.ToTable("ShootingPlacesMovie");
                });

            modelBuilder.Entity("API.Entities.ShootingPlaceTVShow", b =>
                {
                    b.Property<int>("ShootingPlaceID")
                        .HasColumnType("int");

                    b.Property<int>("TVShowID")
                        .HasColumnType("int");

                    b.HasKey("ShootingPlaceID", "TVShowID");

                    b.HasIndex("TVShowID");

                    b.ToTable("ShootingPlacesTVShows");
                });

            modelBuilder.Entity("API.Entities.SubtitleMovie", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("AdminID")
                        .HasColumnType("int");

                    b.Property<int>("DownloadCount")
                        .HasColumnType("int");

                    b.Property<int?>("FrameRate")
                        .HasColumnType("int");

                    b.Property<int>("LanguageID")
                        .HasColumnType("int");

                    b.Property<int>("MovieID")
                        .HasColumnType("int");

                    b.Property<int>("RatingCount")
                        .HasColumnType("int");

                    b.Property<double>("RatingValue")
                        .HasColumnType("double");

                    b.Property<string>("Release")
                        .HasColumnType("longtext");

                    b.Property<string>("SubtitlePath")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.HasIndex("AdminID");

                    b.HasIndex("LanguageID");

                    b.HasIndex("MovieID");

                    b.ToTable("SubtitleMovies");
                });

            modelBuilder.Entity("API.Entities.SubtitleTVShow", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("AdminID")
                        .HasColumnType("int");

                    b.Property<int>("DownloadCount")
                        .HasColumnType("int");

                    b.Property<int>("EpisodeID")
                        .HasColumnType("int");

                    b.Property<int>("LanguageID")
                        .HasColumnType("int");

                    b.Property<int>("RatingCount")
                        .HasColumnType("int");

                    b.Property<double>("RatingValue")
                        .HasColumnType("double");

                    b.Property<string>("SubtitlePath")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.HasIndex("AdminID");

                    b.HasIndex("EpisodeID");

                    b.HasIndex("LanguageID");

                    b.ToTable("SubtitleTVShows");
                });

            modelBuilder.Entity("API.Entities.SubtitleUser", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("RatingCount")
                        .HasColumnType("int");

                    b.Property<double>("RatingValue")
                        .HasColumnType("double");

                    b.Property<int?>("SubtitleMovieID")
                        .HasColumnType("int");

                    b.Property<int?>("SubtitleTVShowID")
                        .HasColumnType("int");

                    b.Property<int?>("TranslationID")
                        .HasColumnType("int");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("SubtitleMovieID");

                    b.HasIndex("SubtitleTVShowID");

                    b.HasIndex("TranslationID");

                    b.HasIndex("UserID");

                    b.ToTable("SubtitleUsers");
                });

            modelBuilder.Entity("API.Entities.Translation", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("LanguageID")
                        .HasColumnType("int");

                    b.Property<string>("Release")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("SubtitlePath")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.HasIndex("LanguageID");

                    b.ToTable("Translations");
                });

            modelBuilder.Entity("API.Entities.TVShow", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CoverArt")
                        .HasColumnType("longtext");

                    b.Property<int?>("EpisodeCount")
                        .HasColumnType("int");

                    b.Property<string>("IMDBId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("IMDBRatingCount")
                        .HasColumnType("int");

                    b.Property<double>("IMDBRatingValue")
                        .HasColumnType("double");

                    b.Property<string>("MetacriticValue")
                        .HasColumnType("longtext");

                    b.Property<string>("RottenTomatoesValue")
                        .HasColumnType("longtext");

                    b.Property<int>("SeasonCount")
                        .HasColumnType("int");

                    b.Property<string>("Synopsis")
                        .IsRequired()
                        .HasMaxLength(600)
                        .HasColumnType("varchar(600)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Year")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.HasKey("ID");

                    b.ToTable("TVShows");
                });

            modelBuilder.Entity("API.Entities.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("AdminID")
                        .HasColumnType("int");

                    b.Property<string>("Designation")
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ProfileImage")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("SubtitleDownload")
                        .HasColumnType("int");

                    b.Property<int>("SubtitleUpload")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("funFact")
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.HasIndex("AdminID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("API.Entities.UserLanguage", b =>
                {
                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<int>("LanguageID")
                        .HasColumnType("int");

                    b.HasKey("UserID", "LanguageID");

                    b.HasIndex("LanguageID");

                    b.ToTable("UsersLanguages");
                });

            modelBuilder.Entity("API.Entities.UserMovies", b =>
                {
                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<int>("MovieID")
                        .HasColumnType("int");

                    b.HasKey("UserID", "MovieID");

                    b.HasIndex("MovieID");

                    b.ToTable("UsersMovies");
                });

            modelBuilder.Entity("API.Entities.UserTVShows", b =>
                {
                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<int>("TVShowID")
                        .HasColumnType("int");

                    b.HasKey("UserID", "TVShowID");

                    b.HasIndex("TVShowID");

                    b.ToTable("UsersTVShows");
                });

            modelBuilder.Entity("API.Entities.ActorMovie", b =>
                {
                    b.HasOne("API.Entities.Actor", "Actor")
                        .WithMany()
                        .HasForeignKey("ActorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.Movie", "Movie")
                        .WithMany()
                        .HasForeignKey("MovieID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Actor");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("API.Entities.ActorTVShow", b =>
                {
                    b.HasOne("API.Entities.Actor", "Actor")
                        .WithMany()
                        .HasForeignKey("ActorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.TVShow", "TVShow")
                        .WithMany()
                        .HasForeignKey("TVShowID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Actor");

                    b.Navigation("TVShow");
                });

            modelBuilder.Entity("API.Entities.Admin", b =>
                {
                    b.HasOne("API.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("API.Entities.Comment", b =>
                {
                    b.HasOne("API.Entities.Admin", null)
                        .WithMany("RemovedComments")
                        .HasForeignKey("AdminID");

                    b.HasOne("API.Entities.SubtitleMovie", "SubtitleMovie")
                        .WithMany()
                        .HasForeignKey("SubtitleMovieID");

                    b.HasOne("API.Entities.SubtitleTVShow", "SubtitleTVShow")
                        .WithMany()
                        .HasForeignKey("SubtitleTVShowID");

                    b.HasOne("API.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SubtitleMovie");

                    b.Navigation("SubtitleTVShow");

                    b.Navigation("User");
                });

            modelBuilder.Entity("API.Entities.CreatorMovie", b =>
                {
                    b.HasOne("API.Entities.Creator", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.Movie", "Movie")
                        .WithMany()
                        .HasForeignKey("MovieID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("API.Entities.CreatorTVShow", b =>
                {
                    b.HasOne("API.Entities.Creator", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.TVShow", "TVShow")
                        .WithMany()
                        .HasForeignKey("TVShowID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");

                    b.Navigation("TVShow");
                });

            modelBuilder.Entity("API.Entities.DirectMessage", b =>
                {
                    b.HasOne("API.Entities.User", "RecipientUser")
                        .WithMany()
                        .HasForeignKey("RecipientUserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RecipientUser");

                    b.Navigation("User");
                });

            modelBuilder.Entity("API.Entities.Episode", b =>
                {
                    b.HasOne("API.Entities.Season", "Season")
                        .WithMany()
                        .HasForeignKey("SeasonID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Season");
                });

            modelBuilder.Entity("API.Entities.Follower", b =>
                {
                    b.HasOne("API.Entities.User", "UserFollowing")
                        .WithMany()
                        .HasForeignKey("UserFollowingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("UserFollowing");
                });

            modelBuilder.Entity("API.Entities.GenreMovie", b =>
                {
                    b.HasOne("API.Entities.Genre", "Genre")
                        .WithMany()
                        .HasForeignKey("GenreID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.Movie", "Movie")
                        .WithMany()
                        .HasForeignKey("MovieID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Genre");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("API.Entities.GenreTVShow", b =>
                {
                    b.HasOne("API.Entities.Genre", "Genre")
                        .WithMany()
                        .HasForeignKey("GenreID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.TVShow", "TVShow")
                        .WithMany()
                        .HasForeignKey("TVShowID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Genre");

                    b.Navigation("TVShow");
                });

            modelBuilder.Entity("API.Entities.Season", b =>
                {
                    b.HasOne("API.Entities.TVShow", "TVShow")
                        .WithMany()
                        .HasForeignKey("TVShowID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TVShow");
                });

            modelBuilder.Entity("API.Entities.ShootingPlaceMovie", b =>
                {
                    b.HasOne("API.Entities.Movie", "Movie")
                        .WithMany()
                        .HasForeignKey("MovieID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.ShootingPlace", "ShootingPlace")
                        .WithMany()
                        .HasForeignKey("ShootingPlaceID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Movie");

                    b.Navigation("ShootingPlace");
                });

            modelBuilder.Entity("API.Entities.ShootingPlaceTVShow", b =>
                {
                    b.HasOne("API.Entities.ShootingPlace", "ShootingPlace")
                        .WithMany()
                        .HasForeignKey("ShootingPlaceID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.TVShow", "TVShow")
                        .WithMany()
                        .HasForeignKey("TVShowID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ShootingPlace");

                    b.Navigation("TVShow");
                });

            modelBuilder.Entity("API.Entities.SubtitleMovie", b =>
                {
                    b.HasOne("API.Entities.Admin", null)
                        .WithMany("RemovedMovieSubtitles")
                        .HasForeignKey("AdminID");

                    b.HasOne("API.Entities.Language", "Language")
                        .WithMany()
                        .HasForeignKey("LanguageID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.Movie", "Movie")
                        .WithMany()
                        .HasForeignKey("MovieID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Language");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("API.Entities.SubtitleTVShow", b =>
                {
                    b.HasOne("API.Entities.Admin", null)
                        .WithMany("RemovedTVShowSubtitles")
                        .HasForeignKey("AdminID");

                    b.HasOne("API.Entities.Episode", "Episode")
                        .WithMany()
                        .HasForeignKey("EpisodeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.Language", "Language")
                        .WithMany()
                        .HasForeignKey("LanguageID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Episode");

                    b.Navigation("Language");
                });

            modelBuilder.Entity("API.Entities.SubtitleUser", b =>
                {
                    b.HasOne("API.Entities.SubtitleMovie", "SubtitleMovie")
                        .WithMany()
                        .HasForeignKey("SubtitleMovieID");

                    b.HasOne("API.Entities.SubtitleTVShow", "SubtitleTVShow")
                        .WithMany()
                        .HasForeignKey("SubtitleTVShowID");

                    b.HasOne("API.Entities.Translation", "Translation")
                        .WithMany()
                        .HasForeignKey("TranslationID");

                    b.HasOne("API.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SubtitleMovie");

                    b.Navigation("SubtitleTVShow");

                    b.Navigation("Translation");

                    b.Navigation("User");
                });

            modelBuilder.Entity("API.Entities.Translation", b =>
                {
                    b.HasOne("API.Entities.Language", "Language")
                        .WithMany()
                        .HasForeignKey("LanguageID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Language");
                });

            modelBuilder.Entity("API.Entities.User", b =>
                {
                    b.HasOne("API.Entities.Admin", null)
                        .WithMany("BannedUsers")
                        .HasForeignKey("AdminID");
                });

            modelBuilder.Entity("API.Entities.UserLanguage", b =>
                {
                    b.HasOne("API.Entities.Language", "Language")
                        .WithMany()
                        .HasForeignKey("LanguageID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Language");

                    b.Navigation("User");
                });

            modelBuilder.Entity("API.Entities.UserMovies", b =>
                {
                    b.HasOne("API.Entities.Movie", "Movie")
                        .WithMany()
                        .HasForeignKey("MovieID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Movie");

                    b.Navigation("User");
                });

            modelBuilder.Entity("API.Entities.UserTVShows", b =>
                {
                    b.HasOne("API.Entities.TVShow", "TVShow")
                        .WithMany()
                        .HasForeignKey("TVShowID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TVShow");

                    b.Navigation("User");
                });

            modelBuilder.Entity("API.Entities.Admin", b =>
                {
                    b.Navigation("BannedUsers");

                    b.Navigation("RemovedComments");

                    b.Navigation("RemovedMovieSubtitles");

                    b.Navigation("RemovedTVShowSubtitles");
                });
#pragma warning restore 612, 618
        }
    }
}
