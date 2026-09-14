using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using SapIntegrationDemo.Models;

namespace SapIntegrationDemo.Services
{
    public class SapService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        public SapService(string token, string baseUrl)
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _baseUrl = baseUrl;
        }

        public async Task<string> GetCustomersAsync()
        {
            var response = await _client.GetAsync($"{_baseUrl}/Customers");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<HttpResponseMessage> CreateCustomerAsync(Customer customer)
        {
            var json = JsonSerializer.Serialize(customer);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await _client.PostAsync($"{_baseUrl}/Customers", content);
        }

        public async Task<HttpResponseMessage> UpdateCustomerAsync(string id, Customer customer)
        {
            var json = JsonSerializer.Serialize(customer);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"{_baseUrl}/Customers({id})")
            {
                Content = content
            };
            return await _client.SendAsync(request);
        }

        public async Task<HttpResponseMessage> DeleteCustomerAsync(string id)
        {
            return await _client.DeleteAsync($"{_baseUrl}/Customers({id})");
        }
    }
}
