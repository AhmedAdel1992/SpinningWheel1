namespace Common.Entities.Interfaces;

public interface ICreate
{
    public DateTime CreatedDate { get; set; }
    public string CreatedBy { get; set; }
}
