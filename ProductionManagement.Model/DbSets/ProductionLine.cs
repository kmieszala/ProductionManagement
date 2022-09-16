using System.ComponentModel.DataAnnotations;

namespace ProductionManagement.Model.DbSets
{
    public class ProductionLine
    {
        public ProductionLine()
        {
            LineTank = new HashSet<LineTank>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(30)]
        [Required]
        public string Name { get; set; }

        public bool Active { get; set; }

        public virtual ICollection<LineTank> LineTank { get; set; }
    }
}
