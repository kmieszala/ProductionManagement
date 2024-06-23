namespace ProductionManagement.Model.DbSets
{
    using System.ComponentModel.DataAnnotations;
    using ProductionManagement.Common.Enums;

    public class LogCodeDict
    {
        public LogCodeDict()
        {
            Log = new HashSet<Log>();
        }

        [Key]
        public LogCodeEnum Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        public bool Active { get; set; }

        public virtual ICollection<Log> Log { get; set; }
    }
}