namespace ProductionManagement.Services.Services.ProductionLine.Models
{
    public class ProductionLineModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Active { get; set; }

        public DateTime StartDate { get; set; }

        public List<LineTankModel> Tanks { get; set; }
    }
}
