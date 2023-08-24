using System.Diagnostics.CodeAnalysis;

using UsersAPI.Helper.Interfaces;

namespace UsersAPI.Helper
{
    [ExcludeFromCodeCoverage]
    public class CookieManager : ICookieManager
    {
        #region Constants
        private static readonly string X_ACCESS_TOKEN = "x-access-token";
        private static readonly string X_REFRESH_TOKEN = "x-refresh-token";
        #endregion

        #region Constructor
        public CookieManager() { }
        #endregion

        #region Public Methods
        /// <summary>
        /// Method to Get Cookie value from Request.
        /// </summary>
        /// <param name="keyName">Cookie name</param>
        /// <param name="context">Http Context</param>
        /// <returns></returns>
        public string GetCookie(string keyName, IHttpContextAccessor context)
        {
            string[] separatingStrings = { ",", ";" };

            var cookieResult = new Dictionary<string, string>();

            if (context.HttpContext.Request.Headers.TryGetValue("Cookie", out var outCookie))
            {
                var outCookieValue = outCookie.ToString();
                var cookieList = outCookieValue.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries).ToList();
                foreach (var item in cookieList)
                {
                    var cookieArray = item.Split("=");
                    cookieResult.Add(cookieArray[0].Trim(), cookieArray[1].Trim());
                }

            }
            cookieResult.TryGetValue(keyName, out string cookie);
            return cookie;

            //string cookie = null;
            //if (!string.IsNullOrEmpty(keyName) && context != null && context.HttpContext.Request.Cookies.ContainsKey(keyName))
            //{
            //    cookie = context.HttpContext.Request.Cookies[keyName];
            //}

            //return cookie;
        }

        /// <summary>
        /// Set Cookie Values to response
        /// </summary>
        /// <param name="accessToken">Access Token</param>
        /// <param name="refreshToken">Refresh Token</param>
        /// <param name="context">Http Context</param>
        public void SetCookies(string accessToken, string refreshToken, IHttpContextAccessor context)
        {
            CookieOptions cookieOptions = new()
            {
                HttpOnly = true,
                Secure = true
            };

            context.HttpContext.Response.Cookies.Append(X_ACCESS_TOKEN, accessToken, cookieOptions);
            context.HttpContext.Response.Cookies.Append(X_REFRESH_TOKEN, refreshToken, cookieOptions);
        }
        #endregion

    }
}
