using System.Net;
using AuthServer.Core.Dtos;
using AuthServer.Core.Dtos.ResponseDtos;
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

        var identityResult = await _userManager.CreateAsync(user, createUserDto.Password);
        if (!identityResult.Succeeded)
        {
            var errors = identityResult.Errors.Select(x => x.Description).ToList();
            return ResponseDto<UserAppDto>.Fail(new ErrorDto(errors, true));
        }

        var userDto = ObjectMapper.Mapper.Map<UserAppDto>(user);
        return ResponseDto<UserAppDto>.Success(userDto);
    }

    public async Task<ResponseDto<UserAppDto>> GetUserByName(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user is null) return ResponseDto<UserAppDto>.Fail("Username not found!");

        var userDto = ObjectMapper.Mapper.Map<UserAppDto>(user);
        return ResponseDto<UserAppDto>.Success(userDto);
    }
}