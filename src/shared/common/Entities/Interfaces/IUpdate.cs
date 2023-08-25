namespace Common.Entities.Interfaces;

public interface IUpdate
{
    DateTime? ModifiedDate { get; set; }
    string ModifiedBy { get; set; }
}