namespace ProductionManagement.Model.DbSets
{
    using System.ComponentModel.DataAnnotations;
    using ProductionManagement.Common.Enums;

    public class UserStatusDict
    {
        public UserStatusDict()
        {
            User = new HashSet<Users>();
        }

        [Key]
        public UserStatusEnum Id { get; set; }

        [MaxLength(20)]
        public string Name { get; set; }

        public bool Active { get; set; }

        public virtual ICollection<Users> User { get; set; }
    }
}
