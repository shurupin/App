namespace App.Models.Core
{
    using System.ComponentModel.DataAnnotations;

    public class UserRole : BaseEntity
    {
        [Required]
        public User User { get; set; } = null!;
        [Required]
        public long UserId { get; set; }

        [Required]
        public Role Role { get; set; } = null!;
        [Required]
        public long RoleId { get; set; }
    }
}
