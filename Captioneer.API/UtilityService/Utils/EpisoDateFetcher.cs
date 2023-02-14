using System.Net;
using System.Text.Json;
using UtilityService.Models;

namespace UtilityService.Utils
{
    public static class EpisoDateFetcher
    {
        private static readonly string apiURL = "https://www.episodate.com/api/";

        private static readonly HttpClient httpClient = new HttpClient();

        public static async Task<EpisoDateModel?> Fetch(string searchQuery)
        {
            // EpisoDate expects search queries to be in lower-case with spaces replaced with hyphens (e.g. game-of-thrones)
            searchQuery = searchQuery.ToLower();
            searchQuery = searchQuery.Replace(' ', '-');
            var url = $"{apiURL}show-details?q={searchQuery}";

            try
            {
                var result = await httpClient.GetAsync(url);

                if (result.StatusCode == HttpStatusCode.NoContent)
                {
                    LoggerManager.GetInstance().LogError($"Failed to fetch {searchQuery} from EpisoDate with status code {result.StatusCode}");
                    return default;
                }

                // First reads the get result as a stream and then deserializes the JSON into an EpisoDateModel
                var body = await result.Content.ReadAsStreamAsync();
                var asObj = await JsonSerializer.DeserializeAsync<EpisoDateModel>(body);

                return asObj;
            }
            catch (Exception e)
            {
                LoggerManager.GetInstance().LogError(e.Message);
            }

            return default;
        }
    }
}
