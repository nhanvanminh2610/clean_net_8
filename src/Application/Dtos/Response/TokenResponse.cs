using System.Text.Json.Serialization;

namespace Application.Dtos.Response
{
    public class TokenResponse : BaseResponse
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int UserId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? UserName { get; set; }

        public string Role { get; set; }
        public long ExpirationTime { get; set; }
    }
}
