using AuthServer.Core.Dtos;
using AuthServer.Core.Dtos.UserDtos;
using AuthServer.Core.Entities;
using AuthServer.Core.Services.Abstract;
using AuthServer.Service.GeneralMapping;
using AutoMapper.Internal.Mappers;
using Microsoft.AspNetCore.Identity;

namespace AuthServer.Service.Services;

public class UserService : IUserService
{
    private readonly UserManager<UserApp> _userManager;

    public UserService(UserManager<UserApp> userManager)
    {
        _userManager = userManager;
    }

     public async Task<ResponseDto<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto)
    {
        var user = new UserApp()
        {
            Email = createUserDto.Email,
            UserName = createUserDto.UserName
        };

        var result = await _userManager.CreateAsync(user, createUserDto.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(x => x.Description).ToList();
            return ResponseDto<UserAppDto>.Fail(new ErrorDto(errors, true), 400);
        }

        var userDto = ObjectMapper.Mapper.Map<UserAppDto>(user);
        return ResponseDto<UserAppDto>.Success(userDto, 200);
    }

    public async Task<ResponseDto<UserAppDto>> GetUserByName(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user is null)
            return ResponseDto<UserAppDto>.Fail("Username not found!", 404, true);

        var userDto = ObjectMapper.Mapper.Map<UserAppDto>(user);
        return ResponseDto<UserAppDto>.Success(userDto, 200);
    }
}