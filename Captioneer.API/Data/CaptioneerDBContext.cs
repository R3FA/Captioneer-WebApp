using Captioneer.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Captioneer.API.Data
{
    public class CaptioneerDBContext : DbContext
    {
        public CaptioneerDBContext(DbContextOptions<CaptioneerDBContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActorMovie>().HasKey(table => new
            {
                table.ActorID, table.MovieID
            });

            modelBuilder.Entity<ActorTVShow>().HasKey(table => new
            {
                table.ActorID, table.TVShowID
            });

            modelBuilder.Entity<CreatorMovie>().HasKey(table => new
            {
                table.CreatorID, table.MovieID, table.Position
            });

            modelBuilder.Entity<CreatorTVShow>().HasKey(table => new
            {
                table.CreatorID, table.TVShowID, table.Position
            });

            modelBuilder.Entity<GenreMovie>().HasKey(table => new
            {
                table.GenreID, table.MovieID
            });

            modelBuilder.Entity<GenreTVShow>().HasKey(table => new
            {
                table.GenreID, table.TVShowID
            });

            modelBuilder.Entity<ShootingPlaceMovie>().HasKey(table => new
            {
                table.ShootingPlaceID, table.MovieID
            });

            modelBuilder.Entity<ShootingPlaceTVShow>().HasKey(table => new
            {
                table.ShootingPlaceID, table.TVShowID
            });
        }

        public DbSet<Actor> Actors { get; set; }
        public DbSet<ActorMovie> ActorMovies { get; set; }
        public DbSet<ActorTVShow> ActorTVShows { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Creator> Creators { get; set; }
        public DbSet<CreatorMovie> CreatorsMovie { get; set; }
        public DbSet<CreatorTVShow> CreatorsTVShows { get; set;}
        public DbSet<DirectMessage> DirectMessages { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<GenreMovie> GenresMovie { get; set; }
        public DbSet<GenreTVShow> GenresTVShows { get; set;}
        public DbSet<Language> Languages { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<ShootingPlace> ShootingPlaces { get; set; }
        public DbSet<ShootingPlaceMovie> ShootingPlacesMovie { get; set;}
        public DbSet<ShootingPlaceTVShow> ShootingPlacesTVShows { get; set; }
        public DbSet<SubtitleMovie> SubtitleMovies { get; set; }
        public DbSet<SubtitleTVShow> SubtitleTVShows { get; set; }
        public DbSet<SubtitleUser> SubtitleUsers { get; set; }
        public DbSet<Translation> Translations { get; set; }
        public DbSet<TVShow> TVShows { get; set; }
        public DbSet<User> Users { get; set; }
    }

}
