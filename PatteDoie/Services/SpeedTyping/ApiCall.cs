namespace PatteDoie.Services.SpeedTyping
{
    public class ApiCall
    {
        private static IHttpClientFactory httpClientFactory;
        public static async Task<String> GetAsync(String url)
        {
            using HttpClient client = httpClientFactory.CreateClient();
            using HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return jsonResponse;
        }
    }
}
