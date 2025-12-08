namespace SmartFactoryERP.Application.Features.Identity.Models
{
    /// <summary>
    /// ØáÈ ÊÌÏíÏ ÇáÜ Token
    /// </summary>
    public class RefreshTokenRequest
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}