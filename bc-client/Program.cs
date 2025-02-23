using bc_client;
using bc_client.Models;

var bcIntegration = new BCIntegration();
var companies = await bcIntegration.GetCompaniesAsync();
foreach (var company in companies)
{
    Console.WriteLine($"Company: {company.Name}");
    var customers = await bcIntegration.GetCustomersAsync(company.Id);
    foreach (var customer in customers)
    {
        Console.WriteLine($"  Customer: {customer.DisplayName}");
    }
}

var singlecustomer = await bcIntegration.GetCustomerAsync(companies.First().Id, "37fe3458-93e1-ef11-9344-6045bde9ca09");
Console.WriteLine($"Single customer: {singlecustomer.DisplayName}");

var newCustomer = await bcIntegration.CreateCustomerAsync(companies.First().Id, new CustomerRequest
{
    DisplayName = "Ulm Falcons",
    Type = "Company"
});
Console.WriteLine($"New customer id: {newCustomer.Id}");
var newCustomerFromGet = await bcIntegration.GetCustomerAsync(companies.First().Id, newCustomer.Id);
Console.WriteLine($"New customer from get: {newCustomerFromGet.DisplayName}");

var updatedCustomer = await bcIntegration.UpdateCustomerAsync(companies.First().Id, newCustomer.Id, new CustomerRequest
{
    DisplayName = "TSG Söflingen",
    Type = "Company"
}, newCustomer.ETag);
Console.WriteLine($"Updated customer display name: {updatedCustomer.DisplayName}");
var updatedCustomerFromGet = await bcIntegration.GetCustomerAsync(companies.First().Id, updatedCustomer.Id);
Console.WriteLine($"Updated customer from get: {updatedCustomerFromGet.DisplayName}");

await bcIntegration.DeleteCustomerAsync(companies.First().Id, newCustomer.Id);
Console.WriteLine("Deleted customer");

try
{
    await bcIntegration.GetCustomerAsync(companies.First().Id, newCustomer.Id);
}
catch (ApplicationException e)
{
    Console.WriteLine($"Customer not found as expected: {e.Message}");
}