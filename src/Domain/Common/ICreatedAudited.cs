namespace Domain.Common
{
    public interface ICreatedAudited
    {
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
    }
}
