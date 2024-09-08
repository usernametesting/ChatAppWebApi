using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SignalR.Helpers;

public static class JwtDecryptor
{
    public static string? Decrypt(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        return userIdClaim?.Value;
    }
}
