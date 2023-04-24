namespace Domain.Models
{
    public class SecureUserModel
    {
        public int? Id { get; set; }


        public string? Name { get; set; }

        private readonly string? Password;


        public string? Email { get; set; }



        public List<User_MeasurementModel> UserMeasurements { get; set; }

        public SecureUserModel()
        {
            Password = null;
        }
    }
}
