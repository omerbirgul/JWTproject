using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using AuthServer.Core.Configuration;
using AuthServer.Core.Configuration.TokenConfiguration;
using AuthServer.Core.Dtos;
using AuthServer.Core.Entities;
using AuthServer.Core.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace AuthServer.Service.Services;

public class TokenService : ITokenService
{
    private readonly CustomTokenOption _customTokenOption;
    private readonly UserManager<UserApp> _userManager;

    public TokenService(IOptions<CustomTokenOption> options, UserManager<UserApp> userManager)
    {
        _userManager = userManager;
        _customTokenOption = options.Value;
    }

    private string CreateRefreshToken()
    {
        var numberBytes = new Byte[32];
        using var random = RandomNumberGenerator.Create();
        random.GetBytes(numberBytes);
        return Convert.ToBase64String(numberBytes);
    }

        // JWT payload içindeki bilgiler buradan toplanır.
    private async Task<IEnumerable<Claim>> GetClaims(UserApp userApp, List<string> audiences)
    {
        var userRoles = await _userManager.GetRolesAsync(userApp);
        var userClaimList = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, userApp.Id),
            new Claim(JwtRegisteredClaimNames.Email, userApp.Email),
            new Claim(ClaimTypes.Name, userApp.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        var value = audiences
            .Select(x => new Claim(JwtRegisteredClaimNames.Aud, x));
        // her bir audience değeri üzerinden geçilir ve her bir öğe için yeni bir Claim oluşturulur ve kullanıcın hedef kitlesi yeni claime atanır.

        var roleValues = userRoles.Select(x => new Claim(ClaimTypes.Role, x));
        
        userClaimList.AddRange(roleValues);
        userClaimList.AddRange(value);
        return userClaimList; 
    }

    private IEnumerable<Claim> GetClaimsByClient(Client client)
    {
        var claims = new List<Claim>();
        claims.AddRange(client.Audiences
            .Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));

        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, client.Id.ToString()));

        return claims;
    }

    
    public TokenDto CreateToken(UserApp userApp)
    {
        var accessTokenExpiration = DateTime.Now
            .AddMinutes(_customTokenOption.AccessTokenExpiration);
        // Access Token expire olma süresi

        var refreshTokenExpiration = DateTime.Now
            .AddMinutes(_customTokenOption.RefreshTokenExpiration);
        // Refresh Token expire olma süresi.

        var securityKey = SignService.GetSymmetricSecurityKey(_customTokenOption.SecurityKey);
        // Token'ı imzalayacak olan key

        SigningCredentials signingCredentials =
            new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
            issuer: _customTokenOption.Issuer,
            expires: accessTokenExpiration,
            notBefore: DateTime.Now,
            claims: GetClaims(userApp, _customTokenOption.Audience).Result,
            signingCredentials: signingCredentials);

        var handler = new JwtSecurityTokenHandler(); 
        // Token'ı oluşturacak.

        var token = handler.WriteToken(jwtSecurityToken);
        var tokenDto = new TokenDto()
        {
            AccessToken = token,
            RefreshToken = CreateRefreshToken(),
            AccessTokenExpiration = accessTokenExpiration,
            RefreshTokenExpiration = refreshTokenExpiration
        };

        return tokenDto;
    }

    public ClientTokenDto CreateTokenByClient(Client client)
    {
        var accessTokenExpiration = DateTime.Now
            .AddMinutes(_customTokenOption.AccessTokenExpiration);

        var securityKey = SignService.GetSymmetricSecurityKey(_customTokenOption.SecurityKey);

        SigningCredentials signingCredentials =
            new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
            issuer: _customTokenOption.Issuer,
            expires: accessTokenExpiration,
            notBefore: DateTime.Now,
            claims: GetClaimsByClient(client),
            signingCredentials: signingCredentials
        );

        var handler = new JwtSecurityTokenHandler();
        var token = handler.WriteToken(jwtSecurityToken);
        var tokenDto = new ClientTokenDto()
        {
            AccessToken = token,
            AccessTokenExpiration = accessTokenExpiration
        };

        return tokenDto;
    }
}