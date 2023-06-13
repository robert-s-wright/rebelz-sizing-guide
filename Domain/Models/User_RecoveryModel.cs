namespace Domain.Models
{
    public class User_RecoveryModel
    {
        public int? Id { get; set; }
        public int UserId { get; set; }
        public string? Password { get; set; }
        public string? Hash { get; set; }
        public DateTime Created { get; set; }

        public bool? IsExpired { get; set; }


        public User_RecoveryModel()
        {
            IsExpired = DateTime.Compare(this.Created, DateTime.Now.AddMinutes(-30)) > 0;
        }

    }
}
