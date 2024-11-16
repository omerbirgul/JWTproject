using AuthServer.Core.Dtos;

namespace AuthServer.Core.Entities;

public class UserRefreshToken
{
    public string UserId { get; set; }
    public string Code { get; set; }
    public DateTime ExpirationDate { get; set; }
}