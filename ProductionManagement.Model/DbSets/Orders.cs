using ProductionManagement.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductionManagement.Model.DbSets
{
    public class Orders
    {
        public Orders()
        {
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(30)]
        [Required]
        public string OrderName { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [MaxLength(20)]
        [Required]
        public string Color { get; set; }

        public int Sequence { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? StopDate { get; set; }

        public OrderStatusEnum Status { get; set; }

        [ForeignKey("Tank")]
        public int TankId { get; set; }

        [ForeignKey("ProductionLine")]
        public int? ProductionLineId { get; set; }

        public virtual Tanks Tank { get; set; }

        public virtual ProductionLine? ProductionLine { get; set; }
    }
}