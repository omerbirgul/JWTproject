using AuthServer.Core.Dtos;
using AuthServer.Core.Dtos.ResponseDtos;
using AuthServer.Core.Dtos.UserDtos;

namespace AuthServer.Core.Services.Abstract;

public interface IUserService
{
    Task<ResponseDto<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto);
    Task<ResponseDto<NoDataDto>> CreateRoleAsync(Guid userId);
    Task<ResponseDto<UserAppDto>> GetUserByName(string userName);
}