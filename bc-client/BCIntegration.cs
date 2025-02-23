using System.Net.Http.Headers;
using System.Text.Json;
using bc_client.Models;
using Microsoft.Extensions.Configuration;

namespace bc_client
{
    public class BCIntegration
    {
        private readonly string _baseUrl;

        public BCIntegration()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.dev.json", optional: true);

            IConfiguration config = builder.Build();

            _baseUrl = config["backendBaseUrl"] ?? throw new ArgumentNullException("config", "Backend base URL is not configured.");

        }

        public async Task<Company[]> GetCompaniesAsync()
        {
            var companiesResponse = await GetFromBackendAsync<CompaniesResponse>("/companies");
            return companiesResponse?.Companies ?? [];
        }

        public async Task<Customer[]> GetCustomersAsync(string companyId)
        {
            var customersResponse = await GetFromBackendAsync<CustomersResponse>($"/companies({companyId})/customers");
            return customersResponse?.Customers ?? [];
        }

        public async Task<Customer> GetCustomerAsync(string companyId, string customerId)
        {
            var customerResponse = await GetFromBackendAsync<CustomerResponse>($"/companies({companyId})/customers({customerId})");
            return customerResponse;
        }

        public async Task<CustomerResponse> CreateCustomerAsync(string companyId, CustomerRequest customerRequest)
        {
            return await PostToBackendAsync<CustomerResponse, CustomerRequest>($"/companies({companyId})/customers", customerRequest);
        }

        public async Task<CustomerResponse> UpdateCustomerAsync(string companyId, string customerId, CustomerRequest customerRequest, string ifMatch)
        {
            var headers = new Dictionary<string, string>
            {
                { "If-Match", ifMatch }
            };

            return await PatchToBackendAsync<CustomerResponse, CustomerRequest>(
                $"/companies({companyId})/customers({customerId})",
                customerRequest,
                headers);
        }

        public async Task DeleteCustomerAsync(string companyId, string customerId)
        {
            await DeleteFromBackendAsync($"/companies({companyId})/customers({customerId})");
        }

        private async Task<T> GetFromBackendAsync<T>(string specificUrl)
        {
            using var httpClient = new HttpClient();
            try
            {
                var response = await httpClient.GetAsync($"{_baseUrl}{specificUrl}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                try
                {
                    var responseContent = JsonSerializer.Deserialize<T>(content);
                    return responseContent ?? throw new ApplicationException($"Deserialized content is null for {_baseUrl}{specificUrl}. Content: {content}");
                }
                catch (JsonException ex)
                {
                    throw new ApplicationException(
                        $"Failed to deserialize backend response. Content: {content}", ex);
                }
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException(
                    $"Failed to fetch backend content from {_baseUrl}{specificUrl}", ex);
            }
        }

        private async Task<TResponse> PostToBackendAsync<TResponse, TRequest>(string specificUrl, TRequest data, IDictionary<string, string>? customHeaders = null)
        {
            return await PostOrPatchToBackendAsync<TResponse, TRequest>(specificUrl, data, HttpMethod.Post, customHeaders);
        }

        private async Task<TResponse> PatchToBackendAsync<TResponse, TRequest>(string specificUrl, TRequest data, IDictionary<string, string>? customHeaders = null)
        {
            return await PostOrPatchToBackendAsync<TResponse, TRequest>(specificUrl, data, HttpMethod.Patch, customHeaders);
        }

        private async Task<TResponse> PostOrPatchToBackendAsync<TResponse, TRequest>(string specificUrl, TRequest data, HttpMethod method, IDictionary<string, string>? customHeaders)
        {
            using var httpClient = new HttpClient();
            try
            {
                var jsonContent = JsonSerializer.Serialize(data);
                var stringContent = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                var requestMessage = new HttpRequestMessage(method, $"{_baseUrl}{specificUrl}")
                {
                    Content = stringContent
                };

                if (customHeaders != null)
                {
                    foreach (var header in customHeaders)
                    {
                        requestMessage.Headers.Add(header.Key, header.Value);
                    }
                }

                var response = await httpClient.SendAsync(requestMessage);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                try
                {
                    var responseContent = JsonSerializer.Deserialize<TResponse>(content);
                    return responseContent ?? throw new ApplicationException($"Deserialized content is null for {_baseUrl}{specificUrl}. Content: {content}");
                }
                catch (JsonException ex)
                {
                    throw new ApplicationException(
                    $"Failed to deserialize backend response. Content: {content}", ex);
                }
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException(
                    $"Failed to {method} to backend at {_baseUrl}{specificUrl}", ex);
            }
        }

        private async Task DeleteFromBackendAsync(string specificUrl)
        {
            using var httpClient = new HttpClient();
            try
            {
                var response = await httpClient.DeleteAsync($"{_baseUrl}{specificUrl}");
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException(
                    $"Failed to delete from backend at {_baseUrl}{specificUrl}", ex);
            }
        }
    }
}