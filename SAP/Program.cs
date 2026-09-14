using System;
using SapIntegrationDemo.Helpers;
using SapIntegrationDemo.Services;
using SapIntegrationDemo.Models;

class Program
{
    static async Task Main(string[] args)
    {
        string clientId = "your-client-id";
        string clientSecret = "your-client-secret";
        string tokenUrl = "https://your-sap-instance/oauth/token";
        string baseUrl = "https://your-sap-instance/sap/c4c/odata/v1";

        var token = await AuthHelper.GetTokenAsync(clientId, clientSecret, tokenUrl);
        var sapService = new SapService(token, baseUrl);

        // Read
        var customers = await sapService.GetCustomersAsync();
        Console.WriteLine(customers);

        // Create
        var newCustomer = new Customer { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };
        var createResponse = await sapService.CreateCustomerAsync(newCustomer);
        Console.WriteLine($"Create Status: {createResponse.StatusCode}");

        // Update
        newCustomer.Email = "updated.email@example.com";
        var updateResponse = await sapService.UpdateCustomerAsync("123", newCustomer);
        Console.WriteLine($"Update Status: {updateResponse.StatusCode}");

        // Delete
        var deleteResponse = await sapService.DeleteCustomerAsync("123");
        Console.WriteLine($"Delete Status: {deleteResponse.StatusCode}");
    }
}
