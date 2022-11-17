using System.Globalization;
using Captioneer.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Captioneer.API.Data.OMDb
{
    public static class OMDbCacher
    {
        /// <summary>
        /// Takes an OMDb model and caches its JSON information to the database, also performing caching for the genre, actors, creators and shooting places
        /// </summary>
        /// <param name="movie">OMDb model containing the JSON information of the movie</param>
        /// <param name="context">The database context</param>
        /// <returns>The Movie object that has been cached to the database</returns>
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

            // Returns 0 if the input JSON cannot be parsed into double and int
            // Rating is expected to be in decimal points and votes will have thousand markers
            double.TryParse(movie.ImdbRating, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out imdbRating);
            int.TryParse(movie.ImdbVotes, NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out imdbVotes);

            // Retrieves the numeric part of the runtime JSON property
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

            // Performs the other cachings necessary
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

        /// <summary>
        /// Takes an OMDb model and caches its JSON information to the database, also performing caching for the genre, actors, creators and shooting places
        /// </summary>
        /// <param name="show">OMDb model containing the JSON information of the TV show</param>
        /// <param name="context"></param>
        /// <returns>The TVShow object that has been cached to the database</returns>
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
                SeasonCount = 0,
                IMDBRatingCount = imdbVotes,
                IMDBRatingValue = imdbRating,
                RottenTomatoesValue = rottenTomatoes,
                MetacriticValue = metacritic,
                CoverArt = poster,
                EpisodeCount = 0
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

        /// <summary>
        /// Caches any genres that are not already present in the database
        /// </summary>
        /// <param name="genres">String containing the genres, delimited with ", "</param>
        /// <param name="context">The database context</param>
        /// <param name="movie">The Movie object which contains the genres</param>
        /// <returns></returns>
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

                if (newDbGenre == null)
                    newDbGenre = dbGenre.First();

                var dbGenreMovie = await context.GenresMovie.Where(gm => gm.Movie.Title == movie.Title && gm.Movie.Year == movie.Year && gm.Genre.Name == newDbGenre.Name).ToListAsync();

                if (dbGenreMovie.Count == 0)
                    await context.GenresMovie.AddAsync(new GenreMovie(){Genre = newDbGenre, Movie = movie});
            }
        }

        /// <summary>
        /// Caches any genres that are not already present in the database
        /// </summary>
        /// <param name="genres">String containing the genres, delimited with ", "</param>
        /// <param name="context">The database context</param>
        /// <param name="show">The TVShow object which contains the genres</param>
        /// <returns></returns>
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

        /// <summary>
        /// Caches any actors that are not already present in the database
        /// </summary>
        /// <param name="actors">String containing the actors, delimited with ", "</param>
        /// <param name="context">The database context</param>
        /// <param name="movie">The Movie object which contains the actors</param>
        /// <returns></returns>
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

        /// <summary>
        /// Caches any actors that are not already present in the database
        /// </summary>
        /// <param name="actors">String containing the actors, delimited with ", "</param>
        /// <param name="context">The database context</param>
        /// <param name="show">The TVShow object which contains the actors</param>
        /// <returns></returns>
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

        /// <summary>
        /// Caches any creators that are not already present in the database
        /// </summary>
        /// <param name="writers">String containing the writers, delimited with ", "</param>
        /// <param name="directors">String containing the directors, delimited with ", "</param>
        /// <param name="context">The database context</param>
        /// <param name="movie">The Movie object which contains the creators</param>
        /// <returns></returns>
        private static async Task CacheCreators(string? writers, string? directors, CaptioneerDBContext context, Movie movie)
        {
            if (writers == "N/A" || writers == null)
                writers = "";
            if (directors == "N/A" || directors == null)
                directors = "";

            var writersSplit = new List<string>(writers.Split(", "));
            var directorsSplit = new List<string>(directors.Split(", "));
            // Unify the creators into one list so they can be iterated on all at the same time
            var creators = new List<string>();
            creators.AddRange(writersSplit);
            creators.AddRange(directorsSplit);

            // Only grab the distinct ones to prevent adding duplicates
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

                // If newCreator is null, then we can grab the creator from the database
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

        /// <summary>
        /// Caches any creators that are not already present in the database
        /// </summary>
        /// <param name="writers">String containing the writers, delimited with ", "</param>
        /// <param name="directors">String containing the directors, delimited with ", "</param>
        /// <param name="context">The database context</param>
        /// <param name="show">The TVShow object which contains the creators</param>
        /// <returns></returns>
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

        /// <summary>
        /// Caches any shooting places that are not already present in the database
        /// </summary>
        /// <param name="country">String containing the filming country</param>
        /// <param name="context">The database context</param>
        /// <param name="movie">The Movie object containing the shooting places</param>
        /// <returns></returns>
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

        /// <summary>
        /// Caches any shooting places that are not already present in the database
        /// </summary>
        /// <param name="country">String containing the filming country</param>
        /// <param name="context">The database context</param>
        /// <param name="show">The TVShow object containing the shooting places</param>
        /// <returns></returns>
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
