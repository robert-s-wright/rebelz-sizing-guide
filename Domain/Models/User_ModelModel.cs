namespace Domain.Models
{
    public class User_ModelModel
    {
        public int? Id { get; set; }
        public int BrandId { get; set; }

        public int UserId { get; set; }
        public int? UserMeasurementId { get; set; }
        public int ModelId { get; set; }
        public int SizeId { get; set; }
        public string Comments { get; set; }
    }
}
