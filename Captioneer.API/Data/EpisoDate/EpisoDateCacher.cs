using Captioneer.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Captioneer.API.Data.EpisoDate
{
    public static class EpisoDateCacher
    {
        public static async Task Cache(List<TVShow> shows, CaptioneerDBContext context)
        {
            if (shows.Count == 0) 
                return;

            foreach (var show in shows)
            {
                var model = await EpisoDateFetcher.Fetch(show.Title);
                var addedSeasons = new List<int>() { -1 };
                var addedEpisodes = new List<int>();
                var updatedSeasons = new List<int>() { -1 };

                if (model == null || model.Show.Episodes == null || model.Show.Episodes.Count == 0)
                    continue;

                var dbSeasons = await context.Seasons.Where(s => s.TVShow.ID == show.ID).ToListAsync();

                foreach (var episode in model.Show.Episodes)
                {
                    if (episode.Season == null)
                        continue;

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

