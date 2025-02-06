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
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserService(UserManager<UserApp> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

     public async Task<ResponseDto<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto)
    {
        var user = new UserApp()
        {
            Email = createUserDto.Email,
            UserName = createUserDto.UserName
        };

        var identityResult = await _userManager.CreateAsync(user, createUserDto.Password);
        if (identityResult is null) return ResponseDto<UserAppDto>.Fail("user cannot added");
        if (!identityResult.Succeeded)
        {
            var errors = identityResult.Errors.Select(x => x.Description).ToList();
            return ResponseDto<UserAppDto>.Fail(new ErrorDto(errors));
        }

        var userDto = ObjectMapper.Mapper.Map<UserAppDto>(user);
        return ResponseDto<UserAppDto>.Success(userDto);
    }

    public async Task<ResponseDto<NoDataDto>> CreateRoleAsync(Guid userId)
    {
        var isRoleExist = await _roleManager.RoleExistsAsync("admin");
        if (!isRoleExist)
        {
            await _roleManager.CreateAsync(new IdentityRole() { Name = "admin" });
            await _roleManager.CreateAsync(new IdentityRole() { Name = "manager" });
        }

        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null) return ResponseDto<NoDataDto>.Fail("user not found");

        await _userManager.AddToRoleAsync(user, "admin");
        await _userManager.AddToRoleAsync(user, "manager");
        return ResponseDto<NoDataDto>.Success();
    }

    public async Task<ResponseDto<UserAppDto>> GetUserByName(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user is null) return ResponseDto<UserAppDto>.Fail("Username not found!");

        var userDto = ObjectMapper.Mapper.Map<UserAppDto>(user);
        return ResponseDto<UserAppDto>.Success(userDto);
    }
}