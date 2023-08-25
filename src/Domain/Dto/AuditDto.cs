namespace Domain.Dto
{
    public class AuditDto
    {
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; } = "System";
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
