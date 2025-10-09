using System.Text.Json.Serialization;

namespace FaysalFundsInternal.Infrastructure.DTOs
{
    public class SignatureModel
    {
        [JsonPropertyName("auth_key")]
        public string auth_key { get; set; }

        [JsonPropertyName("ip_address")]
        public string ip_address { get; set; }

        [JsonPropertyName("time_stamp")]
        public string time_stamp { get; set; }

        [JsonPropertyName("url_path")]
        public string url_path { get; set; }

        [JsonPropertyName("data")]
        public RequestData data { get; set; }

        [JsonPropertyName("secret_key")]
        public string secret_key { get; set; }

        [JsonPropertyName("request_method")]
        public string request_method { get; set; }
    }
}
