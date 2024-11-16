using AuthServer.Core.Configuration;
using AuthServer.Core.Dtos;

namespace AuthServer.Core.Services.Abstract;

public interface IAuthenticationService
{
    Task<ResponseDto<TokenDto>> CreateTokenAsync(LoginDto loginDto);
    Task<ResponseDto<TokenDto>> CreateTokenByRefreshToken(string refreshToken);
    Task<ResponseDto<NoDataDto>> RevokeRefreshTokenAsync(string refreshToken);
    Task<ResponseDto<ClientTokenDto>> CreateTokenByClient(ClientLoginDto clientLoginDto);
}