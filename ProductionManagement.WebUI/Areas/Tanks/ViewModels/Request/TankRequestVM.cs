using System.Collections.Generic;

namespace ProductionManagement.WebUI.Areas.Tanks.ViewModels.Request
{
    public class TankRequestVM
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
        public List<TankPartsVM> Parts { get; set; }
    }
}
