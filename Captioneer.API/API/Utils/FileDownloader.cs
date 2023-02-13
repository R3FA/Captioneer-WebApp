namespace Captioneer.API.Utils
{
    public static class FileDownloader
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public static async Task<bool> Download(string url, string savePath)
        {
            try
            {
                await using var stream = await httpClient.GetStreamAsync(url);
                await using var fileStream = new FileStream(savePath, FileMode.Create);

                await stream.CopyToAsync(fileStream);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;
        }
    }
}
