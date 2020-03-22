namespace App.Models.Core
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using App.Models.Interfaces;

    public class BaseEntity : IId, IGuid, IRowVersion
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Guid { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } = null!;
    }
}
