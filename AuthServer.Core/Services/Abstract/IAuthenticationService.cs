using AuthServer.Core.Dtos;

namespace AuthServer.Core.Services.Abstract;

public interface IAuthenticationService
{
    void CreateToken(LoginDto loginDto);
}