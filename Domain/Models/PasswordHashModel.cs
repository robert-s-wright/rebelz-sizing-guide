namespace Domain.Models
{
    public class PasswordHashModel
    {
        public string UserId { get; set; }
        public string Password { get; set; }

        public string Hash { get; set; }
    }
}
