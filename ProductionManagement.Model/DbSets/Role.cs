namespace ProductionManagement.Model.DbSets
{
    using System.ComponentModel.DataAnnotations;

    public class Role
    {
        public Role()
        {
            UserRoles = new HashSet<UserRoles>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        [MaxLength(300)]
        [Required]
        public string Description { get; set; }

        public virtual ICollection<UserRoles> UserRoles { get; set; }
    }
}
