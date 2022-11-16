using System.Globalization;
using Captioneer.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Captioneer.API.Data.OMDb
{
    public static class OMDbCacher
    {
        public static async Task<Movie?> CacheMovie(OMDbModel? movie, CaptioneerDBContext context)
        {
            if (movie == null)
            {
                Console.WriteLine("Movie passed for caching was null!");
                return null;
            }

            int imdbVotes;
            double imdbRating;
            int runtime = 0;
            string rottenTomatoes = "";
            string metacritic = "";
            string poster = "";

            foreach (var rating in movie.Ratings)
            {
                if (rating.Source == "Rotten Tomatoes")
                    rottenTomatoes = rating.Value;
                if (rating.Source == "Metacritic")
                    metacritic = rating.Value;
            }

            double.TryParse(movie.ImdbRating, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out imdbRating);
            int.TryParse(movie.ImdbVotes, NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out imdbVotes);

            if (movie.Runtime != null)
            {
                var index = movie.Runtime.LastIndexOfAny(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' });
                if (index != -1)
                    runtime = int.Parse(movie.Runtime.Substring(0, index + 1));
            }

            if (movie.Poster != null)
                poster = movie.Poster.ToString();

            var newMovie = new Movie()
            {
                Title = movie.Title == null || movie.Title == "N/A"? string.Empty : movie.Title,
                IMDBId = movie.ImdbId == null || movie.ImdbId == "N/A"? string.Empty : movie.ImdbId,
                IMDBRatingCount = imdbVotes,
                IMDBRatingValue = imdbRating,
                RottenTomatoesValue = rottenTomatoes == ""? null : rottenTomatoes,
                MetacriticValue = metacritic == ""? null : metacritic,
                Synopsis = movie.Plot == null || movie.Plot == "N/A" ? string.Empty : movie.Plot,
                Runtime = runtime,
                Year = movie.Year == null? "" : movie.Year,
                CoverArt = poster
            };

            await context.Movies.AddAsync(newMovie);

            if (movie.Genre != null)
                await CacheGenres(movie.Genre, context, newMovie);
            if (movie.Actors != null)
                await CacheActors(movie.Actors, context, newMovie);

            await CacheCreators(movie.Writer, movie.Director, context, newMovie);

            if (movie.Country != null)
                await CacheShootingPlace(movie.Country, context, newMovie);

            await context.SaveChangesAsync();

            return await context.Movies.FindAsync(newMovie.ID);
        }

        public static async Task<TVShow?> CacheShow(OMDbModel? show, CaptioneerDBContext context)
        {
            if (show == null)
            {
                Console.WriteLine("TVShow passed for caching was null!");
                return null;
            }

            int imdbVotes = 0;
            double imdbRating = 0;
            int runtime = 0;
            string rottenTomatoes = "";
            string metacritic = "";
            int seasonCount = 0;
            string poster = "";

            foreach (var rating in show.Ratings)
            {
                if (rating.Source == "Rotten Tomatoes")
                    rottenTomatoes = rating.Value;
                if (rating.Source == "Metacritic")
                    metacritic = rating.Value;
            }

            double.TryParse(show.ImdbRating, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out imdbRating);
            int.TryParse(show.ImdbVotes, NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out imdbVotes);
            int.TryParse(show.TotalSeasons, out seasonCount);

            if (show.Runtime != null)
            {
                var index = show.Runtime.LastIndexOfAny(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' });
                if (index != -1)
                    runtime = int.Parse(show.Runtime.Substring(0, index + 1));
            }

            if (show.Poster != null)
                poster = show.Poster.ToString();

            var newShow = new TVShow()
            {
                Title = show.Title == null? "" : show.Title,
                IMDBId = show.ImdbId == null? "" : show.ImdbId,
                Synopsis = show.Plot == null? "" : show.Plot,
                Year = show.Year == null? "" : show.Year,
                SeasonCount = seasonCount,
                IMDBRatingCount = imdbVotes,
                IMDBRatingValue = imdbRating,
                RottenTomatoesValue = rottenTomatoes,
                MetacriticValue = metacritic,
                CoverArt = poster
            };

            if (show.Genre != null)
                await CacheGenres(show.Genre, context, newShow);
            if (show.Actors != null)
                await CacheActors(show.Actors, context, newShow);
            await CacheCreators(show.Writer, show.Director, context, newShow);
            if (show.Country != null)
                await CacheShootingPlace(show.Country, context, newShow);

            await context.SaveChangesAsync();

            return await context.TVShows.FindAsync(newShow.ID);
        }

        private static async Task CacheGenres(string genres, CaptioneerDBContext context, Movie movie)
        {
            var genresSplit = genres.Split(", ");

            foreach(var genre in genresSplit)
            {
                var dbGenre = await context.Genres.Where(g => g.Name == genre).ToListAsync();
                Genre newDbGenre = null;

                if (dbGenre.Count == 0)
                {
                    newDbGenre = new Genre() { Name = genre };
                    await context.Genres.AddAsync(newDbGenre);
                }

                if (newDbGenre == null) // Exception
                    newDbGenre = dbGenre.First();

                var dbGenreMovie = await context.GenresMovie.Where(gm => gm.Movie.Title == movie.Title && gm.Movie.Year == movie.Year && gm.Genre.Name == newDbGenre.Name).ToListAsync();

                if (dbGenreMovie.Count == 0)
                    await context.GenresMovie.AddAsync(new GenreMovie(){Genre = newDbGenre, Movie = movie});
            }
        }

        private static async Task CacheGenres(string genres, CaptioneerDBContext context, TVShow show)
        {
            var genresSplit = genres.Split(", ");

            foreach(var genre in genresSplit)
            {
                var dbGenre = await context.Genres.Where(g => g.Name == genre).ToListAsync();
                Genre newDbGenre = null;

                if (dbGenre.Count <= 0)
                {
                    newDbGenre = new Genre() { Name = genre };
                    await context.Genres.AddAsync(newDbGenre);
                }

                if (newDbGenre == null)
                    newDbGenre = dbGenre.First();

                var dbGenreTVShow = await context.GenresTVShows.Where(gtv => gtv.Genre.Name == newDbGenre.Name && gtv.TVShow.Title == show.Title && gtv.TVShow.Year == show.Year).ToListAsync();

                if (dbGenreTVShow.Count == 0)
                    await context.GenresTVShows.AddAsync(new GenreTVShow(){Genre = newDbGenre, TVShow = show});
            }
        }

        private static async Task CacheActors(string actors, CaptioneerDBContext context, Movie movie)
        {
            var actorsSplit = actors.Split(", ");

            foreach (var actor in actorsSplit)
            {
                var names = actor.Split(' ');
                var dbActors = await context.Actors.Where(a => a.FirstName == names[0] && a.Surname == names[1]).ToListAsync();
                Actor newActor = null;

                if (dbActors.Count == 0)
                {
                    newActor = new Actor() { FirstName = names[0], Surname = names[1] };
                    await context.Actors.AddAsync(newActor);
                }

                if (newActor == null)
                    newActor = dbActors.First();

                var dbActorMovie = await context.ActorMovies.Where(
                    am => am.Actor.FirstName == names[0] && am.Actor.Surname == names[1] && am.Movie.Title == movie.Title && am.Movie.Year == movie.Year).ToListAsync();

                if (dbActorMovie.Count == 0)
                    await context.ActorMovies.AddAsync(new ActorMovie() { Actor = newActor, Movie = movie });
            }
        }

        private static async Task CacheActors(string actors, CaptioneerDBContext context, TVShow show)
        {
            var actorsSplit = actors.Split(", ");

            foreach (var actor in actorsSplit)
            {
                var names = actor.Split(' ');
                var dbActors = await context.Actors.Where(a => a.FirstName == names[0] && a.Surname == names[1]).ToListAsync();
                Actor newActor = null;

                if (dbActors.Count == 0)
                {
                    newActor = new Actor() { FirstName = names[0], Surname = names[1] };
                    await context.Actors.AddAsync(newActor);
                }

                if (newActor == null)
                    newActor = dbActors.First();

                var dbActorTVShow = await context.ActorTVShows.Where(
                    am => am.Actor.FirstName == names[0] && am.Actor.Surname == names[1] && am.TVShow.Title == show.Title && am.TVShow.Year == show.Year).ToListAsync();

                if (dbActorTVShow.Count == 0)
                    await context.ActorTVShows.AddAsync(new ActorTVShow() { Actor = newActor, TVShow = show });
            }
        }

        private static async Task CacheCreators(string? writers, string? directors, CaptioneerDBContext context, Movie movie)
        {
            if (writers == "N/A" || writers == null)
                writers = "";
            if (directors == "N/A" || directors == null)
                directors = "";

            var writersSplit = new List<string>(writers.Split(", "));
            var directorsSplit = new List<string>(directors.Split(", "));
            var creators = new List<string>();
            creators.AddRange(writersSplit);
            creators.AddRange(directorsSplit);

            foreach (var creator in creators.Distinct().ToList())
            {
                if (creator == "")
                    continue;

                var names = creator.Split(' ');
                var dbCreator = await context.Creators.Where(c => c.FirstName == names[0] && c.Surname == names[1]).ToListAsync();
                Creator newCreator = null;

                if (dbCreator.Count == 0)
                {
                    newCreator = new Creator() { FirstName = names[0], Surname = names[1] };
                    await context.Creators.AddAsync(newCreator);
                }

                if (newCreator == null)
                    newCreator = dbCreator.First();

                if (writersSplit.Contains(creator))
                {
                    var dbCreatorMovie = await context.CreatorsMovie.Where(
                        cm => cm.Creator.FirstName == newCreator.FirstName && cm.Creator.Surname == newCreator.Surname 
                        && cm.Movie.Title == movie.Title && cm.Movie.Year == movie.Year && cm.Position == "Writer").ToListAsync();

                    if (dbCreatorMovie.Count == 0)
                    {
                        var newCreatorMovie = new CreatorMovie() { Creator = newCreator, Movie = movie, Position = "Writer" };
                        await context.CreatorsMovie.AddAsync(newCreatorMovie);
                    }
                }

                if (directorsSplit.Contains(creator))
                {
                    var dbCreatorMovie = await context.CreatorsMovie.Where(
                        cm => cm.Creator.FirstName == newCreator.FirstName && cm.Creator.Surname == newCreator.Surname 
                        && cm.Movie.Title == movie.Title && cm.Movie.Year == movie.Year && cm.Position == "Director").ToListAsync();

                    if (dbCreatorMovie.Count == 0)
                    {
                        var newCreatorMovie = new CreatorMovie() { Creator = newCreator, Movie = movie, Position = "Director" };
                        await context.CreatorsMovie.AddAsync(newCreatorMovie);
                    }
                }
            }
        }

        private static async Task CacheCreators(string? writers, string? directors, CaptioneerDBContext context, TVShow show)
        {
            if (writers == "N/A" || writers == null)
                writers = "";
            if (directors == "N/A" || directors == null)
                directors = "";

            var writersSplit = new List<string>(writers.Split(", "));
            var directorsSplit = new List<string>(directors.Split(", "));
            var creators = new List<string>();
            creators.AddRange(writersSplit);
            creators.AddRange(directorsSplit);

            foreach (var creator in creators.Distinct().ToList())
            {
                if (creator == "")
                    continue;

                var names = creator.Split(' ');
                var dbCreator = await context.Creators.Where(c => c.FirstName == names[0] && c.Surname == names[1]).ToListAsync();
                Creator newCreator = null;

                if (dbCreator.Count == 0)
                {
                    newCreator = new Creator() { FirstName = names[0], Surname = names[1] };
                    await context.Creators.AddAsync(newCreator);
                }

                if (newCreator == null)
                    newCreator = dbCreator.First();

                if (writersSplit.Contains(creator))
                {
                    var dbCreatorTVShow = await context.CreatorsTVShows.Where(
                        ctv => ctv.TVShow.Title == show.Title && ctv.TVShow.Year == show.Year
                        && ctv.Creator.FirstName == newCreator.FirstName && ctv.Creator.Surname == newCreator.Surname && ctv.Position == "Writer").ToListAsync();

                    if (dbCreatorTVShow.Count == 0)
                    {
                        var newCreatorTVShow = new CreatorTVShow() { Creator = newCreator, TVShow = show, Position = "Writer" };
                        await context.CreatorsTVShows.AddAsync(newCreatorTVShow);
                    }
                }

                if (directorsSplit.Contains(creator))
                {
                    var dbCreatorTVShow = await context.CreatorsTVShows.Where(
                        ctv => ctv.TVShow.Title == show.Title && ctv.TVShow.Year == show.Year
                        && ctv.Creator.FirstName == newCreator.FirstName && ctv.Creator.Surname == newCreator.Surname && ctv.Position == "Director").ToListAsync();

                    if (dbCreatorTVShow.Count == 0)
                    {
                        var newCreatorTVShow = new CreatorTVShow() { Creator = newCreator, TVShow = show, Position = "Director" };
                        await context.CreatorsTVShows.AddAsync(newCreatorTVShow);
                    }
                }
            }
        }

        private static async Task CacheShootingPlace(string country, CaptioneerDBContext context, Movie movie)
        {
            var countriesSplit = country.Split(", ");

            foreach(var countrySplit in countriesSplit)
            {
                var places = await context.ShootingPlaces.Where(sp => sp.Country == countrySplit).ToListAsync();
                ShootingPlace newPlace = null;

                if (places.Count == 0)
                {
                    newPlace = new ShootingPlace() { Country = countrySplit };
                    await context.ShootingPlaces.AddAsync(newPlace);
                }

                if (newPlace == null)
                    newPlace = places.First();

                var placeMovie = await context.ShootingPlacesMovie.Where(
                    spm => spm.ShootingPlace.Country == newPlace.Country && spm.Movie.Title == movie.Title && spm.Movie.Year == movie.Year).ToListAsync();

                if (placeMovie.Count == 0)
                    await context.ShootingPlacesMovie.AddAsync(new ShootingPlaceMovie() { Movie = movie, ShootingPlace = newPlace });
            }
        }

        private static async Task CacheShootingPlace(string country, CaptioneerDBContext context, TVShow show)
        {
            var countriesSplit = country.Split(", ");

            foreach(var countrySplit in countriesSplit)
            {
                var places = await context.ShootingPlaces.Where(sp => sp.Country == countrySplit).ToListAsync();
                ShootingPlace newPlace = null;

                if (places.Count == 0)
                {
                    newPlace = new ShootingPlace() { Country = countrySplit };
                    await context.ShootingPlaces.AddAsync(newPlace);
                }

                if (newPlace == null)
                    newPlace = places.First();

                var placeTVShow = await context.ShootingPlacesTVShows.Where(
                    spm => spm.ShootingPlace.Country == newPlace.Country && spm.TVShow.Title == show.Title && spm.TVShow.Year == show.Year).ToListAsync();

                if (placeTVShow.Count == 0)
                    await context.ShootingPlacesTVShows.AddAsync(new ShootingPlaceTVShow() { TVShow = show, ShootingPlace = newPlace });
            }
        }
    }
}
