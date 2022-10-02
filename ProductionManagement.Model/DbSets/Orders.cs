using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductionManagement.Model.DbSets
{
    public class Orders
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(30)]
        [Required]
        public string OrderName { get; set; }

        [MaxLength(500)]
        [Required]
        public string Description { get; set; }

        [MaxLength(20)]
        [Required]
        public string Color { get; set; }

        [ForeignKey("Tank")]
        public int TankId { get; set; }

        public virtual Tanks Tank { get; set; }
    }
}