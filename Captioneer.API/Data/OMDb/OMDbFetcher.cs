using System.Net;
using System.Text.Json;

namespace Captioneer.API.Data.OMDb
{
    public static class OMDbFetcher
    {
        private static readonly string apiURL = "http://www.omdbapi.com/?";
        private static readonly string apiKey = "bac09921";

        private static readonly HttpClient httpClient = new HttpClient();

        /// <summary>
        /// Makes a HTTP request to the OMDb API and fetches information on a movie/show
        /// </summary>
        /// <param name="searchQuery">Movie/show to search for</param>
        /// <param name="type">Whether to search for a movie or TV show (valid: movie, series, episode)</param>
        /// <returns>OMDbModel object containing all of the information</returns>
        public static async Task<OMDbModel?> Fetch(string searchQuery, string type)
        {
            searchQuery = searchQuery.Replace(' ', '+');
            var url = $"{apiURL}t={searchQuery}&type={type}&apiKey={apiKey}";

            if (searchQuery.StartsWith("tt"))
                url = $"{apiURL}i={searchQuery}&type={type}&apiKey={apiKey}";

            try
            {
                var result = await httpClient.GetAsync(url);

                if (result.StatusCode == HttpStatusCode.NotFound || result.StatusCode == HttpStatusCode.NoContent)
                    return default(OMDbModel?);

                var body = await result.Content.ReadAsStreamAsync();
                var asObj = await JsonSerializer.DeserializeAsync<OMDbModel>(body);

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

            return default(OMDbModel?);
        }
    }
}
