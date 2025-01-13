namespace Domain.Common
{
    public interface IUpdatedAudited
    {
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
