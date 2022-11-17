using System.Net;
using System.Text.Json;

namespace Captioneer.API.Data.EpisoDate
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
                    return default(EpisoDateModel?);

                // First reads the get result as a stream and then deserializes the JSON into an EpisoDateModel
                var body = await result.Content.ReadAsStreamAsync();
                var asObj = await JsonSerializer.DeserializeAsync<EpisoDateModel>(body);

                return asObj;
            }
            catch (HttpRequestException)
            {
                Console.WriteLine("An error occured when requesting the information");
            }
            catch (NotSupportedException)
            {
                Console.WriteLine("The content type is not supported");
            }
            catch (JsonException)
            {
                Console.WriteLine("Invalid JSON");
            }

            return default(EpisoDateModel?);
        }
    }
}
