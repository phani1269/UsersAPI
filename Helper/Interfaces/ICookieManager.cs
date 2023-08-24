namespace UsersAPI.Helper.Interfaces
{
    public interface ICookieManager
    {
        public void SetCookies(string accessToken, string refreshToken, IHttpContextAccessor context);

        public string GetCookie(string keyName, IHttpContextAccessor context);
    }
}
