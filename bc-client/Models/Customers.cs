using System.Text.Json.Serialization;

namespace bc_client.Models
{
    public class CustomersResponse
    {
        [JsonPropertyName("@odata.context")]
        public required string ODataContext { get; init; }

        [JsonPropertyName("value")]
        public required Customer[] Customers { get; init; }
    }

    public class CustomerResponse : Customer
    {
        [JsonPropertyName("@odata.context")]
        public required string ODataContext { get; init; }
    }

    public class Customer
    {
        [JsonPropertyName("@odata.etag")]
        public required string ETag { get; init; }

        [JsonPropertyName("id")]
        public required string Id { get; init; }

        [JsonPropertyName("number")]
        public required string Number { get; init; }

        [JsonPropertyName("displayName")]
        public required string DisplayName { get; init; }

        [JsonPropertyName("type")]
        public required string Type { get; init; }

        [JsonPropertyName("addressLine1")]
        public required string AddressLine1 { get; init; }

        [JsonPropertyName("addressLine2")]
        public required string AddressLine2 { get; init; }

        [JsonPropertyName("city")]
        public required string City { get; init; }

        [JsonPropertyName("state")]
        public required string State { get; init; }

        [JsonPropertyName("country")]
        public required string Country { get; init; }

        [JsonPropertyName("postalCode")]
        public required string PostalCode { get; init; }

        [JsonPropertyName("phoneNumber")]
        public required string PhoneNumber { get; init; }

        [JsonPropertyName("email")]
        public required string Email { get; init; }

        [JsonPropertyName("website")]
        public required string Website { get; init; }

        [JsonPropertyName("salespersonCode")]
        public required string SalespersonCode { get; init; }

        [JsonPropertyName("balanceDue")]
        public decimal BalanceDue { get; init; }

        [JsonPropertyName("creditLimit")]
        public decimal CreditLimit { get; init; }

        [JsonPropertyName("taxLiable")]
        public bool TaxLiable { get; init; }

        [JsonPropertyName("taxAreaId")]
        public required string TaxAreaId { get; init; }

        [JsonPropertyName("taxAreaDisplayName")]
        public required string TaxAreaDisplayName { get; init; }

        [JsonPropertyName("taxRegistrationNumber")]
        public required string TaxRegistrationNumber { get; init; }

        [JsonPropertyName("currencyId")]
        public required string CurrencyId { get; init; }

        [JsonPropertyName("currencyCode")]
        public required string CurrencyCode { get; init; }

        [JsonPropertyName("paymentTermsId")]
        public required string PaymentTermsId { get; init; }

        [JsonPropertyName("shipmentMethodId")]
        public required string ShipmentMethodId { get; init; }

        [JsonPropertyName("paymentMethodId")]
        public required string PaymentMethodId { get; init; }

        [JsonPropertyName("blocked")]
        public required string Blocked { get; init; }

        [JsonPropertyName("lastModifiedDateTime")]
        public DateTime LastModifiedDateTime { get; init; }
    }

    public class CustomerRequest
    {
        [JsonPropertyName("displayName")]
        public required string DisplayName { get; init; }

        [JsonPropertyName("type")]
        public required string Type { get; init; }
    }
}