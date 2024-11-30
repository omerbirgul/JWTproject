using AuthServer.API.TokenConfiguration;
using AuthServer.Core.Configuration;
using AuthServer.Core.Configuration.TokenConfiguration;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<CustomTokenOption>(builder.Configuration.GetSection("TokenOptions"));
// appSetting.json içindeki veriler ile CustomTokenOption prop'larını eşleştirdik.

builder.Services.Configure<List<Client>>(builder.Configuration.GetSection("Clients"));
// appSetting.json içindeki veriler ile Client prop'larını eşleştirdik.

builder.Services.AddSingleton<ICustomTokenOption>(sp =>
    sp.GetRequiredService<IOptions<CustomTokenOption>>().Value);

// Add services to the container.

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

app.UseAuthorization();

app.MapControllers();

app.Run();