namespace Authentication.Options;
public class JwtOptions
{
    public const string SectionKey = "JWT";

    public string SecretKey { get; set; }

    public string ValidAudience { get; set; }

    public string ValidIssuer { get; set; } 

    public string ExpirationMinutes { get; set; }
}
