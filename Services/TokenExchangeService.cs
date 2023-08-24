using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UsersAPI.Helper;
using UsersAPI.Helper.Interfaces;
using UsersAPI.Models;
using Token = UsersAPI.Models.Token;

namespace UsersAPI.Services
{
    public class TokenExchangeService : ITokenExchangeService
    {
        #region Constants
        private static readonly string USERNAME = "username";
        private static readonly string PASSWORD = "password";
        private static readonly string GRANT_TYPE = "grant_type";
        private static readonly string CLIENT_ID = "client_id";
        private static readonly string CLIENT_SECRET = "client_secret";
        private static readonly string ACCESS_TOKEN = "access_token";
        private static readonly string REFRESH_TOKEN = "refresh_token";
        private static readonly string TOKEN = "token";
        private static readonly string TOKEN_TYPE_HINT = "token_type_hint";

        private static readonly string X_REFRESH_TOKEN = "x-refresh-token";
        private static readonly string X_ACCESS_TOKEN = "x-access-token";

        private static readonly string REFRESH_TOKEN_ERROR_MSG = "refreshToken must contain a value";
        private static readonly string ACCESS_TOKEN_ERROR_MSG = "Cookie does not have access token";
        private static readonly string INVALID_TOKEN_ERROR_MSG = "Invalid token";
        private static readonly string REFRESH_TOKEN_EXPIRED_ERROR_MSG = "RefreshToken has been expired";
        private static readonly string INVALID_CREDENTIALS = "Invalid Credentials";
        #endregion

        #region Configurations
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;
        private readonly ICookieManager _cookieManager;
        private readonly IHttpContextAccessor context;
        private HttpResponseMessage tokenResponse = null;
        private AuthenticationData authenticationData;
        #endregion

        #region Constructor
        public TokenExchangeService(IConfiguration configuration,
                             HttpClient client,
                             HttpResponseMessage tokenResponse,
                             AuthenticationData authenticationData,
                             ICookieManager cookieManager,
                             IHttpContextAccessor context)
        {
            _configuration = configuration;
            _client = client;
            _cookieManager = cookieManager;
            this.context = context;
            this.tokenResponse = tokenResponse;
            this.authenticationData = authenticationData;
        }
        #endregion

        public async Task<AuthenticationData> AuthenticateUser(AuthenticationModel authenticationRequest)
        {
            string tokenUrl = Environment.GetEnvironmentVariable("KEYCLOAK_TOKEN_URL") ?? _configuration.GetValue<string>("KeyCloakSettings:KEYCLOAK_TOKEN_URL");
            string grantType = Environment.GetEnvironmentVariable("GRANT_TYPE") ?? _configuration.GetValue<string>("KeyCloakSettings:GRANT_TYPE");
            string clientId = Environment.GetEnvironmentVariable("CLIENT_ID") ?? _configuration.GetValue<string>("KeyCloakSettings:CLIENT_ID");
            string clientSecret = Environment.GetEnvironmentVariable("CLIENT_SECRET") ?? _configuration.GetValue<string>("KeyCloakSettings:CLIENT_SECRET");

            var form = new Dictionary<string, string> {

                    { USERNAME, authenticationRequest.UserName},
                    { PASSWORD, Helper.Helper.Base64Decode(authenticationRequest.HashedPassword)},
                    { GRANT_TYPE, grantType },
                    { CLIENT_ID, clientId},
                    { CLIENT_SECRET, clientSecret }
                };

            authenticationData = await GetApiResponse(tokenUrl, new FormUrlEncodedContent(form));
            return authenticationData;
        }
        private async Task<AuthenticationData> GetApiResponse(string tokenUrl, FormUrlEncodedContent content)
        {
            tokenResponse = await _client.PostAsync(tokenUrl, content);
            string responseText = tokenResponse.Content.ReadAsStringAsync().Result;

            if (tokenResponse.IsSuccessStatusCode)
            {
                Token token = JsonConvert.DeserializeObject<Token>(responseText);
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token.AccessToken);
                var claims = jwtSecurityToken.Claims;
                authenticationData = GetClaimsData(token.AccessToken, claims, token.RefreshToken);
            }

            else
            {
                throw new Exception(responseText);
            }

            return authenticationData;
        }

        private AuthenticationData GetClaimsData(string token, IEnumerable<Claim> claims, string refreshToken)
        {
            authenticationData = new();

            var userRoles = claims.First(claim => claim.Type == "realm_access").Value.ToString();
            authenticationData.UserId = claims.First(claim => claim.Type == "sub").Value;
            authenticationData.UserName = claims.First(claim => claim.Type == "preferred_username").Value;
            authenticationData.FirstName = claims.First(claim => claim.Type == "given_name").Value;
            authenticationData.LastName = claims.First(claim => claim.Type == "family_name").Value;
            authenticationData.UserRoles = Settings.Roles.Where(role => userRoles.Contains(role)).Select(role => role).ToList();
            authenticationData.AccessToken = token;
            authenticationData.RefreshToken = refreshToken;

            return authenticationData;
        }
    }
}
