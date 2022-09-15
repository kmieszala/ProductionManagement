namespace ProductionManagement.Services.Services.Tanks.Models
{
    public class TankPartsModel
    {
        public int Id { get; set; }

        public int PartsId { get; set; }

        public int TankId { get; set; }

        public string PartsName { get; set; }

        public int PartsNumber { get; set; }
    }
}
