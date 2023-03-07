using System.Net;
using System.Text.Json;
using UtilityService.Models;

namespace UtilityService.Utils
{
    public static class OMDbFetcher
    {
        private static readonly string apiURL = "http://www.omdbapi.com/?";

        private static readonly HttpClient httpClient = new HttpClient();

        /// <summary>
        /// Makes a HTTP request to the OMDb API and fetches information on a movie/show
        /// </summary>
        /// <param name="searchQuery">Movie/show to search for</param>
        /// <param name="type">Whether to search for a movie or TV show (valid: movie, series, episode)</param>
        /// /// <param name="apiKey">Api key from OMDb</param>
        /// <returns>OMDbModel object containing all of the information</returns>
        public static async Task<OMDbModel?> Fetch(string searchQuery, string type, string apiKey)
        {
            searchQuery = searchQuery.Replace(' ', '+');
            var url = $"{apiURL}t={searchQuery}&type={type}&apiKey={apiKey}";

            // Switches the URL to use the OMDb API endpoint that expects IMDBid queries
            if (searchQuery.StartsWith("tt"))
                url = $"{apiURL}i={searchQuery}&type={type}&apiKey={apiKey}";

            try
            {
                var result = await httpClient.GetAsync(url);

                if (result.StatusCode != HttpStatusCode.OK)
                {
                    LoggerManager.GetInstance().LogError($"Failed to fetch {searchQuery} from OMDb with status code {result.StatusCode}");
                    return default;
                }

                // Reads the resulting body as a stream and then deserializes the JSON as a OMDb model object
                var body = await result.Content.ReadAsStreamAsync();
                var asObj = await JsonSerializer.DeserializeAsync<OMDbModel>(body);

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
