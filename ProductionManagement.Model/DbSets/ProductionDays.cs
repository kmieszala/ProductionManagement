using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductionManagement.Model.DbSets
{
    public class ProductionDays
    {
        [Key]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public bool DayOff { get; set; }

        [ForeignKey("ProductionLine")]
        public int ProductionLineId { get; set; }

        public virtual ProductionLine? ProductionLine { get; set; }
    }
}