namespace SmartFactoryERP.Domain.Entities.Identity
{
    /// <summary>
    /// ÃœÊ· · Œ“Ì‰ «·‹ Refresh Tokens
    /// </summary>
    public class RefreshToken
    {
        public int Id { get; set; }
        
        /// <summary>
        /// «·‹ Token ‰›”Â (ÌÃ» √‰ ÌﬂÊ‰ ›—Ìœ)
        /// </summary>
        public string Token { get; set; }
        
        /// <summary>
        /// «·‹ JWT Token «·–Ì  „ ≈’œ«—Â „⁄Â
        /// </summary>
        public string JwtId { get; set; }
        
        /// <summary>
        /// Â·  „ «” Œœ«„ Â–« «·‹ Tokenø
        /// </summary>
        public bool IsUsed { get; set; }
        
        /// <summary>
        /// Â·  „ ≈»ÿ«· Â–« «·‹ Tokenø (⁄‰œ Logout „À·«)
        /// </summary>
        public bool IsRevoked { get; set; }
        
        /// <summary>
        ///  «—ÌŒ «·≈‰‘«¡
        /// </summary>
        public DateTime CreatedDate { get; set; }
        
        /// <summary>
        ///  «—ÌŒ «‰ Â«¡ «·’·«ÕÌ…
        /// </summary>
        public DateTime ExpiryDate { get; set; }
        
        /// <summary>
        /// «·„” Œœ„ «·„«·ﬂ ··‹ Token
        /// </summary>
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}