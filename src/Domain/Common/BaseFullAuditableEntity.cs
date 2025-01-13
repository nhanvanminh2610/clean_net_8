namespace Domain.Common
{
    public abstract class BaseFullAuditableEntity : BaseEntity, ICreatedAudited, IUpdatedAudited, IDeletedAudited
    {
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }

        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
    }
}
