using System.Text.Json.Serialization;

namespace FaysalFunds.Application.DTOs
{
    public class RequestData
    {
        [JsonPropertyName("uuid")]
        public string uuid { get; set; }

        [JsonPropertyName("cnic")]
        public string cnic { get; set; }

        [JsonPropertyName("type")]
        public string type { get; set; }

        [JsonPropertyName("tier")]
        public string tier { get; set; }
    }
}
