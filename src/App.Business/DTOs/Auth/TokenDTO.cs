namespace App.Business.DTOs.Auth
{
    public class TokenDTO
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
        public UserInfoDTO User { get; set; }
    }
}
