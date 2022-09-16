using System.Collections.Generic;

namespace ProductionManagement.WebUI.Areas.ProductionLine.ViewModels.Request
{
    public class ProductionLineVM
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Active { get; set; }

        public List<LineTankVM> Tanks { get; set; }
    }
}
