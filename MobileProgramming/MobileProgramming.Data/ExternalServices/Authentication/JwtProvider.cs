
using Infrastructure.ExternalServices.Authentication.Setting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MobileProgramming.Data.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MobileProgramming.Business.Models.Constants;

namespace Infrastructure.ExternalServices.Authentication
{
    public class JwtProvider : IJwtProvider
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IUserRepository _userRepository;
        

        public JwtProvider(IOptions<JwtSettings> jwtSettings,
            IUserRepository userRepository)
        {
            _jwtSettings = jwtSettings.Value;
            _userRepository = userRepository;  
        }

        //generate access token
        public async Task<string> GenerateAccessToken(string email)
        {

            var existUser = await _userRepository.GetUserByUsernameAsync(email);
            if (existUser == null)
            {
                return "Error! Unauthorized.";
            }

            //Define information in the payload
            List<Claim> claims = new List<Claim>
            {
                new Claim(UserClaimType.UserId, existUser.UserId.ToString()),
                new Claim(ClaimTypes.Email, existUser.Email!),
                new Claim(ClaimTypes.Role, existUser.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _jwtSettings.Securitykey ?? throw new InvalidOperationException("Secret not configured")));

            var tokenhandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_jwtSettings.TokenExpirationInMinutes)),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            };
            var securityToken = tokenhandler.CreateToken(tokenDescriptor);
            string finaltoken = tokenhandler.WriteToken(securityToken);

            return finaltoken;

        }



        //Validate the token if the token is decoded with jwt, and then extract the information in the token
        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidAudience = _jwtSettings.Audience,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Securitykey)),
                ValidateLifetime = false //false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("HARD CODE");

            return principal;
        }

    }
}
