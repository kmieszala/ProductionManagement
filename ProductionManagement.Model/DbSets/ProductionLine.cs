using System.ComponentModel.DataAnnotations;

namespace ProductionManagement.Model.DbSets
{
    public class ProductionLine
    {
        public ProductionLine()
        {
            LineTank = new HashSet<LineTank>();
            ProductionDays = new HashSet<ProductionDays>();
            Orders = new HashSet<Orders>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(30)]
        [Required]
        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public bool Active { get; set; }

        public virtual ICollection<LineTank> LineTank { get; set; }

        public virtual ICollection<ProductionDays> ProductionDays { get; set; }

        public virtual ICollection<Orders> Orders { get; set; }
    }
}