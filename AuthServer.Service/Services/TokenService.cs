using System.Security.Claims;
using System.Security.Cryptography;
using AuthServer.Core.Configuration;
using AuthServer.Core.Configuration.TokenConfiguration;
using AuthServer.Core.Dtos;
using AuthServer.Core.Entities;
using AuthServer.Core.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;

namespace AuthServer.Service.Services;

public class TokenService : ITokenService
{
    private readonly UserManager<UserApp> _userManager;
    private readonly ICustomTokenOption _customTokenOption;

    public TokenService(UserManager<UserApp> userManager, ICustomTokenOption customTokenOption)
    {
        _userManager = userManager;
        _customTokenOption = customTokenOption;
    }

    private string CreateRefreshToken()
    {
        var numberBytes = new Byte[32];
        using var random = RandomNumberGenerator.Create();
        random.GetBytes(numberBytes);
        return Convert.ToBase64String(numberBytes);
    }

    private IEnumerable<Claim> GetClaim(UserApp userApp, List<string> audiences)
    {
        var userList = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, userApp.Id),
            new Claim(JwtRegisteredClaimNames.Email, userApp.Email),
            new Claim(ClaimTypes.Name, userApp.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        var value = audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x));
        // her bir audience değeri üzerinden geçilir ve her bir öğe için yeni bir Claim oluşturulur ve kullanıcın hedef kitlesi yeni claime atanır.
        
        userList.AddRange(value);
        return userList;
    }

    
    public TokenDto CreateToken(UserApp userApp)
    {
        throw new NotImplementedException();
    }

    public ClientTokenDto CreateTokenByClient(Client client)
    {
        throw new NotImplementedException();
    }
}