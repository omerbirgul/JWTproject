using System.Security.Cryptography;
using AuthServer.Core.Configuration;
using AuthServer.Core.Configuration.TokenConfiguration;
using AuthServer.Core.Dtos;
using AuthServer.Core.Entities;
using AuthServer.Core.Services.Abstract;
using Microsoft.AspNetCore.Identity;

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
    
    public TokenDto CreateToken(UserApp userApp)
    {
        throw new NotImplementedException();
    }

    public ClientTokenDto CreateTokenByClient(Client client)
    {
        throw new NotImplementedException();
    }
}