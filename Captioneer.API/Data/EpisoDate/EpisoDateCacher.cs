using Captioneer.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Captioneer.API.Data.EpisoDate
{
    public static class EpisoDateCacher
    {
        /// <summary>
        /// Takes a list of TV shows retrieved from the database and caches any new episodes/seasons for it from EpisoDate
        /// </summary>
        /// <param name="shows">List of shows to cache seasons/episodes for</param>
        /// <param name="context">The database context</param>
        /// <returns></returns>
        public static async Task Cache(List<TVShow> shows, CaptioneerDBContext context)
        {
            if (shows.Count == 0) 
                return;

            foreach (var show in shows)
            {
                var model = await EpisoDateFetcher.Fetch(show.Title);
                // Just some lists to better track what seasons have been added/updated and what episodes have been added
                var addedSeasons = new List<int>() { -1 };
                var addedEpisodes = new List<int>();
                var updatedSeasons = new List<int>() { -1 };

                if (model == null || model.Show.Episodes == null || model.Show.Episodes.Count == 0)
                    continue;

                var dbSeasons = await context.Seasons.Where(s => s.TVShow.ID == show.ID).ToListAsync();

                // Go through all of the seasons fetched and add them to the database if not already present
                foreach (var episode in model.Show.Episodes)
                {
                    if (episode.Season == null)
                        continue;

                    // Prevent adding seasons if they've already been tracked for addition of if they're already in the database
                    if (!dbSeasons.Any(s => s.SeasonNumber == episode.Season) && !addedSeasons.Contains((int)episode.Season))
                    {
                        var newSeason = new Season()
                        {
                            TVShow = show,
                            SeasonNumber = (int)episode.Season,
                            EpisodeCount = 0
                        };

                        await context.Seasons.AddAsync(newSeason);
                        addedSeasons.Add(newSeason.SeasonNumber);
                    }
                }

                await context.SaveChangesAsync();

                // Go through all of the episodes fetched and add them to the database if not already present
                foreach (var episode in model.Show.Episodes)
                {
                    if (episode.Season == null)
                        continue;

                    var dbSeason = await context.Seasons.Where(s => s.SeasonNumber == episode.Season && s.TVShow.ID == show.ID).FirstAsync();

                    if (!context.Episodes.Any(e => e.Name == episode.Name && e.Season.ID == dbSeason.ID))
                    {
                        var newEpisode = new Episode()
                        {
                            Name = episode.Name == null? $"Episode {episode.EpisodeNumber}" : episode.Name,
                            EpisodeNumber = (int)episode.EpisodeNumber!,
                            Season = dbSeason
                        };

                        await context.Episodes.AddAsync(newEpisode);
                        addedEpisodes.Add(newEpisode.ID);

                        if (!updatedSeasons.Contains(dbSeason.ID))
                            updatedSeasons.Add(dbSeason.ID);
                    }
                }

                await context.SaveChangesAsync();

                // If any seasons have been updated, update the season and episode counts for the show
                if (updatedSeasons.Count > 1)
                {
                    show.SeasonCount = addedSeasons.Count - 1;
                    show.EpisodeCount = addedEpisodes.Count;

                    foreach (var seasonID in updatedSeasons)
                    {
                        var count = await context.Episodes.CountAsync(e => e.Season.ID == seasonID);
                        await context.Seasons.Where(s => s.ID == seasonID).ForEachAsync((season) =>
                        {
                            season.EpisodeCount = count;
                        });
                    }

                    await context.SaveChangesAsync();
                }

            }
        }
    }
}

