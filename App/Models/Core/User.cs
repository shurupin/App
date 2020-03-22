namespace App.Models.Core
{
    using System.ComponentModel.DataAnnotations;
    using Interfaces;

    public class User : BaseEntity, IName, IEmail
    {
        [Required(AllowEmptyStrings = false), StringLength(maximumLength: 300, MinimumLength = 1)]
        public string Name { get; set; } = null!;

        [EmailAddress, Required(AllowEmptyStrings = true), StringLength(maximumLength: 300, MinimumLength = 1)]
        public string? Email { get; set; }
    }
}
