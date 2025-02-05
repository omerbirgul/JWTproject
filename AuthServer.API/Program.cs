using AuthServer.API.Extensions;
using AuthServer.Core.Configuration;
using AuthServer.Core.Configuration.TokenConfiguration;
using AuthServer.Core.Entities;
using AuthServer.Core.Repositories;
using AuthServer.Core.Services.Abstract;
using AuthServer.Core.UnitOfWork;
using AuthServer.Service.Extensions;
using AuthServer.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.Configure<CustomTokenOption>(builder.Configuration.GetSection("TokenOptions"));
// appSetting.json içindeki veriler ile CustomTokenOption prop'larını eşleştirdik.

builder.Services.Configure<CustomTokenOption>(builder.Configuration.GetSection("TokenOptions"));
builder.Services.Configure<List<Client>>(builder.Configuration.GetSection("Clients"));
// appSetting.json içindeki veriler ile Client prop'larını eşleştirdik.



// DbContext ekleme işlemi
builder.Services.AddDatabase(builder.Configuration);

// Identity ve JWT ekleme işlemi
builder.Services.AddSecurity(builder.Configuration);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
{
    var tokenOptions = builder.Services.BuildServiceProvider()
        .GetRequiredService<IOptions<CustomTokenOption>>().Value;
    opts.TokenValidationParameters = new TokenValidationParameters()
    {
        
        ValidIssuer = tokenOptions.Issuer,
        ValidAudience = tokenOptions.Audience[0],
        IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOptions.SecurityKey),

        ValidateIssuerSigningKey = true,
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// DI Registers
builder.Services.AddServices(builder.Configuration);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();