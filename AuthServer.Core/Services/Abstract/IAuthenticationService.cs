using AuthServer.Core.Configuration;
using AuthServer.Core.Dtos;
using AuthServer.Core.Dtos.LoginDtos;
using AuthServer.Core.Dtos.ResponseDtos;

namespace AuthServer.Core.Services.Abstract;


// Direkt olarak API'ye göndereceğimiz token'lar olduğu için
// response dto gönderiyoruz.
public interface IAuthenticationService
{
    Task<ResponseDto<TokenDto>> CreateTokenAsync(LoginDto loginDto);
    Task<ResponseDto<TokenDto>> CreateTokenByRefreshToken(string refreshToken);
    Task<ResponseDto<NoDataDto>> RevokeRefreshTokenAsync(string refreshToken);
    ResponseDto<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto);
}