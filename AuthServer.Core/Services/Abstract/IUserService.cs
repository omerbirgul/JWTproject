using AuthServer.Core.Dtos;
using AuthServer.Core.Dtos.UserDtos;

namespace AuthServer.Core.Services.Abstract;

public interface IUserService
{
    Task<ResponseDto<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto);
    Task<ResponseDto<UserAppDto>> GetUserByName(string userName);
}