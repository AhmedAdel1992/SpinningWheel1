using Auditing;
using Common.DependencyInjection.Interfaces;

namespace Infrastructure.Services;

public class AuditService:IBaseAuditService
{
    string IBaseAuditService.GetUserId() => "test";
}