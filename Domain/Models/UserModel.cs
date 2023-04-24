namespace Domain.Models
{
    public class UserModel
    {


        public int? Id { get; set; }


        public string? Name { get; set; }


        public string? Email { get; set; }

        public string? Password { get; set; }

        public List<User_MeasurementModel> UserMeasurements { get; set; }


    }
}
