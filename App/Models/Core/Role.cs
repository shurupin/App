namespace App.Models.Core
{
    using System.ComponentModel.DataAnnotations;
    using Interfaces;

    public class Role : BaseEntity, IName
    {
        [Required(AllowEmptyStrings = false), StringLength(maximumLength: 300, MinimumLength = 1)]
        public string Name { get; set; } = null!;
    }
}
