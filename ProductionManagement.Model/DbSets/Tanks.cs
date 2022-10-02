namespace ProductionManagement.Model.DbSets
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using ProductionManagement.Model.Core;

    public class Tanks //: ITrackable
    {
        public Tanks()
        {
            TankParts = new HashSet<TankParts>();
            LineTank = new HashSet<LineTank>();
            Orders = new HashSet<Orders>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        [MaxLength(300)]
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// The number of days it takes to produce one tank.
        /// </summary>
        public decimal ProductionDays { get; set; }

        /// <summary>
        /// Is still produced.
        /// </summary>
        public bool Active { get; set; }

        public DateTime CreationDate { get; set; }

        //public DateTime ModificationDate { get; set; }

        //[ForeignKey("User")]
        //public int ModificationUserId { get; set; }

        // public virtual Users User { get; set; }

        public virtual ICollection<TankParts> TankParts { get; set; }

        public virtual ICollection<LineTank> LineTank { get; set; }

        public virtual ICollection<Orders> Orders { get; set; }
    }
}
