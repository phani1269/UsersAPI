using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace UsersAPI.Models
{
    public class AuthenticationData
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public List<string> UserRoles { get; set; } = new List<string>();
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
    [ExcludeFromCodeCoverage]
    public class Token
    {
        [JsonProperty("access_token", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("refresh_expires_in", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("refresh_expires_in")]
        public int RefreshExpiresIn { get; set; }

        [JsonProperty("refresh_token", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("token_type", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("not-before-policy", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("not-before-policy")]
        public int NotBeforePolicy { get; set; }

        [JsonProperty("session_state", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("session_state")]
        public string SessionState { get; set; }

        [JsonProperty("scope", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("scope")]
        public string Scope { get; set; }
    }
}
