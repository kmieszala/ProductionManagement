using System.ComponentModel.DataAnnotations;

namespace ProductionManagement.Model.DbSets
{
    public class Parts
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(30)]
        [Required]
        public string Name { get; set; }

        [MaxLength(500)]
        [Required]
        public string Description { get; set; }
    }
}
