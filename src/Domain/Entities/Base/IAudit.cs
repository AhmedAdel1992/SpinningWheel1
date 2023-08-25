using Common.Entities.Interfaces;

namespace Domain.Entities.Base;

public interface IAudit:ICreate,IUpdate
{
    DateTime? ModifiedDate { get; set; }
    string ModifiedBy { get; set; }
}