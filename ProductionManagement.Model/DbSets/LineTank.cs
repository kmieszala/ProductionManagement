using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductionManagement.Model.DbSets
{
    public class LineTank
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Tank")]
        public int TankId { get; set; }

        public virtual Tanks Tank { get; set; }

        [ForeignKey("ProductionLine")]
        public int ProductionLineId { get; set; }

        public virtual ProductionLine ProductionLine { get; set; }
    }
}
