using System.Security.Claims;

namespace Infrastructure.ExternalServices.Authentication;

public interface IJwtProvider
{
    /// <summary>
    /// generate access token 
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<string> GenerateAccessToken(string email);
}