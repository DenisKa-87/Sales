namespace Api.DTO
{
    /// <summary>
    /// An entinty to pass user data to the client
    /// </summary>
    public class UserDto
    {
        public string Username { get; set; }
        public string Token { get; set; } //authorization token
    }
}
