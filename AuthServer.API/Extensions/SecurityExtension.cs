using AuthServer.Core.Configuration.TokenConfiguration;
using AuthServer.Core.Entities;
using AuthServer.Data;
using AuthServer.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace AuthServer.API.Extensions
{
    public static class SecurityExtension
    {
        public static void AddSecurity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<UserApp, IdentityRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
                opt.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();


            // services.AddAuthentication(options =>
            // {
            //     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            // }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
            // {
            //     var tokenOptions = configuration.GetSection("TokenOptions").Get<CustomTokenOption>();
            //     if (tokenOptions is null) throw new Exception("tokenOption is null");
            //     opts.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
            //     {
            //         ValidIssuer = tokenOptions.Issuer,
            //         ValidAudience = tokenOptions.Audience[0],
            //         IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOptions.SecurityKey),
            //
            //         ValidateIssuerSigningKey = true,
            //         ValidateAudience = true,
            //         ValidateIssuer = true,
            //         ValidateLifetime = true,
            //         ClockSkew = TimeSpan.Zero
            //     };
            // });
        }
    }
}
