using AuthServer.API.Extensions;
using AuthServer.Core.Configuration;
using AuthServer.Core.Configuration.TokenConfiguration;
using AuthServer.Service.Extensions;

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

app.UseAuthorization();

app.MapControllers();

app.Run();