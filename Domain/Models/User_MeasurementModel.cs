namespace Domain.Models
{
    public class User_MeasurementModel : ICloneable
    {

        public int? Id { get; set; }

        public DateTime? Date { get; set; }
        public int? UserId { get; set; }

        public int? Height { get; set; }
        public int? Weight { get; set; }

        public int? Arm { get; set; }

        public int? Shoulders { get; set; }
        public int? Chest { get; set; }
        public int? Belly { get; set; }

        public int? Waist { get; set; }
        public int? Seat { get; set; }
        public int? Thigh { get; set; }

        public object Clone()
        {
            return new User_MeasurementModel
            {
                Id = null,

                Date = null,
                UserId = this.UserId,

                Height = this.Height,
                Weight = this.Weight,

                Arm = this.Arm,

                Shoulders = this.Shoulders,
                Chest = this.Chest,
                Belly = this.Belly,

                Waist = this.Waist,
                Seat = this.Seat,
                Thigh = this.Thigh
            };

        }
    }
}
