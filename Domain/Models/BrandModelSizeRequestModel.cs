namespace Domain.Models
{
    public class BrandModelSizeRequestModel
    {
        public List<BrandModel> Brands { get; set; }
        public List<ModelModel> Models { get; set; }
        public List<SizeModel> Sizes { get; set; }
        public List<Model_SizeModel> Model_Sizes { get; set; }
        public List<MeasurementModel> Measurements { get; set; }


    }
}
