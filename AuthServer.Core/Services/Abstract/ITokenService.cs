using AuthServer.Core.Configuration;
using AuthServer.Core.Dtos;
using AuthServer.Core.Entities;

namespace AuthServer.Core.Services.Abstract;

public interface ITokenService
{
    TokenDto CreateToken(UserApp userApp);
    ClientTokenDto CreateTokenByClient(Client client);
}