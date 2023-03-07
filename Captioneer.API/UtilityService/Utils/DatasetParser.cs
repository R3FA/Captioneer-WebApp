using UtilityService.Models;

namespace UtilityService.Utils
{
    public static class DatasetParser
    {
        public static async Task<List<OMDbModel>> ParseDataset(Stream datasetStream, string apiKey)
        {
            var models = new List<OMDbModel>();
            using var reader = new StreamReader(datasetStream);

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();

                if (line![0] == 't')
                {
                    var imdbId = line.Split(',')[0];
                    var type = line.Split(',')[1].ToLower();

                    var model = await OMDbFetcher.Fetch(imdbId, type, apiKey);

                    if (model != null)
                        models.Add(model);
                    else
                        LoggerManager.GetInstance().LogError($"Could not fetch {imdbId} when parsing");
                }
            }

            LoggerManager.GetInstance().LogInfo($"Parsed {models.Count} OMDb models from dataset for seeding");
            return models;
        }
    }
}
