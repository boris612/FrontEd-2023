namespace Demo3_WebApi.Util;
#nullable disable
public class TokenConfig
{
    public string Secret { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int AccessExpiration { get; set; }
}
