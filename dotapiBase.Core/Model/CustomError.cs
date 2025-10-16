using System.Text.Json.Serialization;

namespace dotapiBase.Core.Model
{
    public class CustomError
    {
        [JsonPropertyName("code")]
        public string? Code { get; set; }
        [JsonPropertyName("message")]
        public string? Message { get; set; }
    }
}
