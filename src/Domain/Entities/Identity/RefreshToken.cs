using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Tables
{
    [Table("RefreshToken")]
    public partial class RefreshToken : BaseFullAuditableEntity
    {  
        [ForeignKey("User")]
        public int UserFId { get; set; }
        [MaxLength(1000)]
        public string? TokenHash { get; set; }
        [MaxLength(1000)]
        public string? TokenSalt { get; set; }
        public DateTime? TS { get; set; }
        public DateTime? ExpiryDate { get; set; }

        public virtual User User { get; set; }
    }
}
