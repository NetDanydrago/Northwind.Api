namespace Authentication.Dtos;

public class RefreshTokenDto
{
    public string Token { get; set; } 
    public int UserId { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; } 
    public bool IsRevoked { get; set; } 
    public DateTime? RevokedAt { get; set; }
    public string ReplacedByToken { get; set; }
}
