using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Common.Extensions;
using Microsoft.IdentityModel.Tokens;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Wallet.Integration.Tests.Servers;

public class ExternalApi : IDisposable
{
    private WireMockServer _server;
    public string Url => _server.Url!;

    public void Start()
    {
        _server = WireMockServer.Start();
        
    }

    public void Dispose()
    {
        _server.Stop();
        _server.Dispose();
    }
    
    public string GenerateJwtToken(string secretKey="test-seceret-key", string issuer="test-issure", string audience="test-audience", int expirySeconds=3, Claim[] claims=default)
    {
        // Create symmetric security key from the secret key
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        // Create signing credentials using the security key and algorithm
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // Create JWT token descriptor with issuer, audience, claims, and expiration
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = issuer,
            Audience = audience,
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddSeconds(expirySeconds),
            SigningCredentials = signingCredentials
        };

        // Create JWT token handler
        var tokenHandler = new JwtSecurityTokenHandler();

        // Generate JWT token
        var token = tokenHandler.CreateToken(tokenDescriptor);

        // Write the token as a string
        var tokenString = tokenHandler.WriteToken(token);

        return tokenString;
    }
}