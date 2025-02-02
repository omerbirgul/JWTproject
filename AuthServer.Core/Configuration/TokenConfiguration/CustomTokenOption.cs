namespace AuthServer.Core.Configuration.TokenConfiguration;

public class CustomTokenOption
{
    public List<string> Audience { get; }
    public string Issuer { get; }
    public int AccessTokenExpiration { get; }
    public int RefreshTokenExpiration { get; }
    public string SecurityKey { get; }
}