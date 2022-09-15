namespace ProductionManagement.WebUI.Areas.Tanks.ViewModels.Request
{
    public class TankPartsVM
    {
        public int Id { get; set; }

        public int PartsId { get; set; }

        public int TankId { get; set; }

        public string PartsName { get; set; }

        public int PartsNumber { get; set; }
    }
}
