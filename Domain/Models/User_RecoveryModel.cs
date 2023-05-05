namespace Domain.Models
{
    public class User_RecoveryModel
    {
        public int? Id { get; set; }
        public int UserId { get; set; }
        public string? Hash { get; set; }
    }
}
