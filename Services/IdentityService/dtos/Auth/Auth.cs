namespace IdentityService.dtos.Auth
{
    public class Auth
    {

    }
    public class LoginRequestDto
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }

    public class LoginResponce
    {
        public string Token {  get; set; } = string.Empty;

        public DateTime ExperiedAt { get; set; }

        public string userName { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public int UserId {  get; set; }

        public List<string> Permission {  get; set; }
     }
}
