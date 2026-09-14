using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SapIntegrationDemo.Helpers
{
    public static class AuthHelper
    {
        public static async Task<string> GetTokenAsync(string clientId, string clientSecret, string tokenUrl)
        {
            using var client = new HttpClient();

            var tokenRequest = new Dictionary<string, string>
            {
                { "client_id", clientId },
                { "client_secret", clientSecret },
                { "grant_type", "client_credentials" }
            };

            var response = await client.PostAsync(tokenUrl, new FormUrlEncodedContent(tokenRequest));
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonDocument.Parse(json).RootElement.GetProperty("access_token").GetString();
        }
    }
}
