using UsersAPI.Models;

namespace UsersAPI.Services
{
    public interface ITokenExchangeService
    {
        Task<AuthenticationData> AuthenticateUser(AuthenticationModel authenticationRequest);
    }
}