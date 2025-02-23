using System.Text.Json;
using bc_client.Models;
using Xunit;

namespace bc_client.Tests;

public class BCClientTests
{
    private readonly string _companyName = "CRONUS AG";
    private readonly string _companyId = "9ee48135-93e1-ef11-9344-6045bde9ca09";

    private readonly string _customerDisplayName = "Spotsmeyer's Furnishings";
    private readonly string _customerNumber = "01121212";
    private readonly string _customerId = "37fe3458-93e1-ef11-9344-6045bde9ca09";

    [Fact]
    public async Task GetCompaniesAsync_ReturnsCompanies_WhenApiCallSucceeds()
    {
        // Arrange
        var bcIntegration = new BCIntegration();

        // Act
        var companies = await bcIntegration.GetCompaniesAsync();

        // Assert
        Assert.NotNull(companies);
        Assert.NotEmpty(companies);

        var company = companies[0];
        Assert.NotNull(company.Id);
        Assert.Equal(_companyName, company.Name);
        Assert.Equal(_companyId, company.Id);
        Assert.NotNull(company.SystemVersion);
        Assert.True(company.SystemCreatedAt > DateTime.MinValue);
    }

    [Fact]
    public async Task GetCustomersAsync_ReturnsCustomers_WhenApiCallSucceeds()
    {
        // Arrange
        var bcIntegration = new BCIntegration();

        // Act 
        var customers = await bcIntegration.GetCustomersAsync(_companyId);

        // Assert
        Assert.NotNull(customers);
        Assert.NotEmpty(customers);

        Assert.Contains(customers, c => c.DisplayName == _customerDisplayName);
        var customer = customers.First(c => c.DisplayName == _customerDisplayName);
        Assert.Equal(_customerNumber, customer.Number);
        Assert.NotNull(customer.Email);
        Assert.True(customer.LastModifiedDateTime > DateTime.MinValue);
    }

    [Fact]
    public async Task GetCustomerAsync_ReturnsCustomer_WhenApiCallSucceeds()
    {
        // Arrange
        var bcIntegration = new BCIntegration();

        // Act
        var customer = await bcIntegration.GetCustomerAsync(_companyId, _customerId);

        // Assert
        Assert.NotNull(customer);
        Assert.Equal(_customerDisplayName, customer.DisplayName);
        Assert.Equal(_customerNumber, customer.Number);
        Assert.NotNull(customer.Email);
        Assert.True(customer.LastModifiedDateTime > DateTime.MinValue);
    }

    [Fact]
    public async Task CreateCustomerAsync_ReturnsNewCustomer_WhenApiCallSucceeds()
    {
        // Arrange
        var bcIntegration = new BCIntegration();
        var newCustomer = new CustomerRequest
        {
            DisplayName = "Ulm Falcons",
            Type = "Company"
        };

        // Act
        var createdCustomer = await bcIntegration.CreateCustomerAsync(_companyId, newCustomer);
        var retrieveCreatedCustomer = await bcIntegration.GetCustomerAsync(_companyId, createdCustomer.Id);

        // Assert 
        Assert.NotNull(createdCustomer);
        Assert.Equal(newCustomer.DisplayName, createdCustomer.DisplayName);
        Assert.NotNull(createdCustomer.Id);
        Assert.True(createdCustomer.LastModifiedDateTime > DateTime.MinValue);

        Assert.NotNull(retrieveCreatedCustomer);
        Assert.Equal(newCustomer.DisplayName, retrieveCreatedCustomer.DisplayName);
        Assert.Equal(createdCustomer.Id, retrieveCreatedCustomer.Id);
        Assert.True(retrieveCreatedCustomer.LastModifiedDateTime > DateTime.MinValue);
    }

    [Fact]
    public async Task UpdateCustomerAsync_ReturnsUpdatedCustomer_WhenApiCallSucceeds()
    {
        // Arrange
        var bcIntegration = new BCIntegration();
        var updateRequest = new CustomerRequest
        {
            DisplayName = "TSG Söflingen",
            Type = "Company"
        };
        var newCustomer = new CustomerRequest
        {
            DisplayName = "Ulm Falcons",
            Type = "Company"
        };
        var createdCustomer = await bcIntegration.CreateCustomerAsync(_companyId, newCustomer);
        var retrieveCreatedCustomer = await bcIntegration.GetCustomerAsync(_companyId, createdCustomer.Id);

        // Act
        var updatedCustomer = await bcIntegration.UpdateCustomerAsync(_companyId, createdCustomer.Id, updateRequest, retrieveCreatedCustomer.ETag);
        var retrieveUpdatedCusteomer = await bcIntegration.GetCustomerAsync(_companyId, createdCustomer.Id);

        // Assert
        Assert.NotNull(updatedCustomer);
        Assert.Equal(updateRequest.DisplayName, updatedCustomer.DisplayName);
        Assert.Equal(createdCustomer.Id, updatedCustomer.Id);
        Assert.True(updatedCustomer.LastModifiedDateTime > DateTime.MinValue);

        Assert.NotNull(retrieveUpdatedCusteomer);
        Assert.Equal(updateRequest.DisplayName, retrieveUpdatedCusteomer.DisplayName);
        Assert.Equal(createdCustomer.Id, retrieveUpdatedCusteomer.Id);
        Assert.True(retrieveUpdatedCusteomer.LastModifiedDateTime > DateTime.MinValue);
    }

    [Fact]
    public async Task DeleteCustomerAsync_RemovesCustomer_WhenApiCallSucceeds()
    {
        // Arrange
        var bcIntegration = new BCIntegration();
        var newCustomer = new CustomerRequest
        {
            DisplayName = "Ulm Falcons",
            Type = "Company"
        };
        var createdCustomer = await bcIntegration.CreateCustomerAsync(_companyId, newCustomer);

        // Act
        await bcIntegration.DeleteCustomerAsync(_companyId, createdCustomer.Id);

        // Assert
        // Verify customer no longer exists
        var exception = await Assert.ThrowsAsync<ApplicationException>(() =>
            bcIntegration.GetCustomerAsync(_companyId, createdCustomer.Id));
        Assert.Contains("Failed to fetch backend content", exception.Message);
    }
}