namespace Application.Dtos.Request
{
    public class RefreshTokenRequest
    {
        public int UserId { get; set; }
        public string RefreshToken { get; set; }
    }
}
