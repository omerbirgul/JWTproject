using AuthServer.Core.Configuration;
using AuthServer.Core.Dtos;
using AuthServer.Core.Entities;
using AuthServer.Core.Repositories;
using AuthServer.Core.Services.Abstract;
using AuthServer.Core.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AuthServer.Service.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly List<Client> _clients;
    private readonly ITokenService _tokenService;
    private readonly UserManager<UserApp> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<UserRefreshToken> _userRefreshTokenRepository;

    public AuthenticationService(IOptions<List<Client>> optionsClient, ITokenService tokenService,
        UserManager<UserApp> userManager, IUnitOfWork unitOfWork,
        IGenericRepository<UserRefreshToken> userRefreshTokenRepository)
    {
        _clients = optionsClient.Value;
        _tokenService = tokenService;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _userRefreshTokenRepository = userRefreshTokenRepository;
    }

    
    public async Task<ResponseDto<TokenDto>> CreateTokenAsync(LoginDto loginDto)
    {
        if (loginDto is null) throw new ArgumentException(nameof(loginDto));

        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user is null) return ResponseDto<TokenDto>.Fail("Email or Password wrong!", 400, true);

        bool isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);
        if(!isPasswordCorrect) return ResponseDto<TokenDto>.Fail("Email or Password wrong!", 400, true);

        var token = _tokenService.CreateToken(user);
        var userRefreshToken = await _userRefreshTokenRepository
            .Where(x => x.UserId == user.Id).SingleOrDefaultAsync();

        if (userRefreshToken is null)
        {
            await _userRefreshTokenRepository.AddAsync(new UserRefreshToken()
            {
                UserId = user.Id,
                Code = token.RefreshToken,
                ExpirationDate = token.RefreshTokenExpiration

            });
        }
        else
        {
            userRefreshToken.Code = token.RefreshToken;
            userRefreshToken.ExpirationDate = token.RefreshTokenExpiration;
        }

        await _unitOfWork.SaveChangesAsync();
        return ResponseDto<TokenDto>.Success(token, 200);
    }
    
    
    public ResponseDto<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto)
    {
        var client = _clients
            .SingleOrDefault(x => x.Id == clientLoginDto.ClientId && x.Secret == clientLoginDto.ClientSecret);

        if (client == null) return ResponseDto<ClientTokenDto>
            .Fail("ClientId or ClientSecret not found!", 404, true);

        var token = _tokenService.CreateTokenByClient(client);
        return ResponseDto<ClientTokenDto>.Success(token, 200);
    }

    public async Task<ResponseDto<TokenDto>> CreateTokenByRefreshToken(string refreshToken)
    {
        var existRefreshToken = await _userRefreshTokenRepository
            .Where(x => x.Code == refreshToken).SingleOrDefaultAsync();

        if (existRefreshToken is null) ResponseDto<TokenDto>.Fail("Refresh token not found!", 404, true);

        var user = await _userManager.FindByIdAsync(existRefreshToken.UserId);
        if (user is null) return ResponseDto<TokenDto>.Fail("UserId not found!", 404, true);

        var tokenDto = _tokenService.CreateToken(user);
        existRefreshToken.Code = tokenDto.RefreshToken;
        existRefreshToken.ExpirationDate = tokenDto.RefreshTokenExpiration;

        await _unitOfWork.SaveChangesAsync();
        return ResponseDto<TokenDto>.Success(tokenDto, 200);
    }

    public async Task<ResponseDto<NoDataDto>> RevokeRefreshTokenAsync(string refreshToken)
    {
        var existRefreshToken = await _userRefreshTokenRepository
            .Where(x => x.Code == refreshToken).SingleOrDefaultAsync();

        if (existRefreshToken is null) 
            return ResponseDto<NoDataDto>.Fail("Refresh token not found", 404, true);
        
        _userRefreshTokenRepository.Remove(existRefreshToken);
        await _unitOfWork.SaveChangesAsync();
        return ResponseDto<NoDataDto>.Success(200);
    }
}