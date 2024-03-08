namespace Api.DTO
{
    public class LoginDto
    {
        /// <summary>
        /// Password and Username are identical, so we pass username only.
        /// </summary>
        public string Username { get; set; }
    }
}
