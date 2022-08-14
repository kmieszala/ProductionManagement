namespace ProductionManagement.Model.DbSets
{
    using System.ComponentModel.DataAnnotations;
    using ProductionManagement.Common.Enums;

    public class Role
    {
        public Role()
        {
            UserRoles = new HashSet<UserRoles>();
        }

        [Key]
        public RolesEnum Id { get; set; }

        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        public bool Active { get; set; }

        public virtual ICollection<UserRoles> UserRoles { get; set; }
    }
}
