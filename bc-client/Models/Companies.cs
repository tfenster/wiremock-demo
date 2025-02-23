using System.Text.Json.Serialization;

namespace bc_client.Models
{
    public class CompaniesResponse
    {
        [JsonPropertyName("@odata.context")]
        public required string ODataContext { get; init; }

        [JsonPropertyName("value")]
        public required Company[] Companies { get; init; }
    }

    public class Company
    {
        [JsonPropertyName("id")]
        public required string Id { get; init; }

        [JsonPropertyName("systemVersion")]
        public required string SystemVersion { get; init; }

        [JsonPropertyName("timestamp")]
        public int Timestamp { get; init; }

        [JsonPropertyName("name")]
        public required string Name { get; init; }

        [JsonPropertyName("displayName")]
        public required string DisplayName { get; init; }

        [JsonPropertyName("businessProfileId")]
        public required string BusinessProfileId { get; init; }

        [JsonPropertyName("systemCreatedAt")]
        public DateTime SystemCreatedAt { get; init; }

        [JsonPropertyName("systemCreatedBy")]
        public required string SystemCreatedBy { get; init; }

        [JsonPropertyName("systemModifiedAt")]
        public DateTime SystemModifiedAt { get; init; }

        [JsonPropertyName("systemModifiedBy")]
        public required string SystemModifiedBy { get; init; }
    }
}