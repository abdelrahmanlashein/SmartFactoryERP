namespace SmartFactoryERP.Application.Features.Identity.Models
{
    /// <summary>
    /// ÑÏ íÍÊæí Úáì Access Token æ Refresh Token
    /// </summary>
    public class TokenResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}