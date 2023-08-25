namespace Common.Entities.Interfaces;

public interface IDelete
{
    public bool IsDeleted { get; set; }
    public DateTime DeletedDate { get; set; }
    public string DeletedBy { get; set; }
}