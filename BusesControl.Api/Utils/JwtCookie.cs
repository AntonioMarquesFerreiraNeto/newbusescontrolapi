namespace BusesControl.Api.Utils
{
    public class JwtCookie
    {
        public static CookieOptions GetOptions(bool isDevelopment, long expireHours) 
        {
            return new CookieOptions
            {
                HttpOnly = true,
                MaxAge = TimeSpan.FromHours(expireHours),
                SameSite = SameSiteMode.None,
                Secure = true,
                Path = "/"
            };
        }
    }
}