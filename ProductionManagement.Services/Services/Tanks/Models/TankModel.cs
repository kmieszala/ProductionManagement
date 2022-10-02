namespace ProductionManagement.Services.Services.Tanks.Models
{
    public class TankModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// The number of days it takes to produce one tank.
        /// </summary>
        public decimal ProductionDays { get; set; }

        /// <summary>
        /// Is still produced.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Parts needed to make the tank.
        /// </summary>
        public List<TankPartsModel> Parts { get; set; }

        public List<ProductionLineTankModel> ProductionLines { get; set; }
    }
}
