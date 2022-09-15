using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductionManagement.Model.DbSets
{
    public class TankParts
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Number of parts.
        /// </summary>
        public int PartsNumber { get; set; }

        [ForeignKey("Tank")]
        public int TankId { get; set; }

        public virtual Tanks Tank { get; set; }

        [ForeignKey("Parts")]
        public int PartsId { get; set; }

        public virtual Parts Parts { get; set; }
    }
}
