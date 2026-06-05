namespace BusesControl.Api.Utils
{
    public class JwtCookie
    {
        public static CookieOptions GetOptions(bool isDevelopment) 
        {
            return new CookieOptions
            {
                HttpOnly = true,
                SameSite = isDevelopment ? SameSiteMode.None : SameSiteMode.Strict,
                Secure = true,
                Path = "/api",
            };
        }
    }
}
