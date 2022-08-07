namespace ProductionManagement.Model.DbSets
{
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public User()
        {
            UserRoles = new HashSet<UserRoles>();
            Tank = new HashSet<Tank>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        [Required]
        public string FirstName { get; set; }

        [MaxLength(100)]
        [Required]
        public string LastName { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        public virtual ICollection<UserRoles> UserRoles { get; set; }

        public virtual ICollection<Tank> Tank { get; set; }
    }
}
