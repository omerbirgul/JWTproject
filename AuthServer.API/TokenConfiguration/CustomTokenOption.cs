using AuthServer.Core.Configuration.TokenConfiguration;

namespace AuthServer.API.TokenConfiguration;

public class CustomTokenOption : ICustomTokenOption
{
    public List<string> Audience { get; }
    public string Issuer { get; }
    public int AccessTokenExpiration { get; }
    public int RefreshTokenExpiration { get; }
    public string SecurityKey { get; }
}