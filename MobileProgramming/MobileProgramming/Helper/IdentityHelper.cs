using MobileProgramming.Business.Models.Constants;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace MobileProgramming.API.Helper;

public static class IdentityHelper
{
    public static string GetUserIdFromToken(this IPrincipal user)
    {
        if (user == null)
            return string.Empty;

        var identity = user.Identity as ClaimsIdentity;
        IEnumerable<Claim> claims = identity!.Claims;
        return claims.FirstOrDefault(s => s.Type == "UserId")?.Value ?? string.Empty;
    }
    public static string GetUserIdFromToken2(HttpContext context)
    {
        string? jwtToken = GetJwtTokenFromHeader(context);
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);

            var userId = token.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
                return string.Empty;

            return userId;
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }

    public static string GetEmailFromToken(this IPrincipal user)
    {
        if (user == null)
            return string.Empty;

        var identity = user.Identity as ClaimsIdentity;
        IEnumerable<Claim> claims = identity!.Claims;
        return claims.FirstOrDefault(s => s.Type.Equals(UserClaimType.Email))?.Value ?? string.Empty;
    }

    private static string? GetJwtTokenFromHeader(HttpContext httpContext)
    {
        if (httpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            // Giả sử token được truyền trong header với tiền tố "Bearer "
            return authHeader.FirstOrDefault()?.Replace("Bearer ", "");
        }

        return string.Empty;
    }
}
