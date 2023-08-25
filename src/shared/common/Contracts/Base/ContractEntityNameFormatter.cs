using MassTransit;

namespace Common.Contracts.Base;

public class ContractEntityNameFormatter : IEntityNameFormatter
{
    private readonly string _env;

    public ContractEntityNameFormatter(string env=default)
    {
        _env = $"{env}";
    }
    public string FormatEntityName<T>()
    {
        var t = typeof(T);
        var envName=!string.IsNullOrEmpty(_env)? $"{_env}-" : string.Empty;
        return t switch
        {
            // _ when t.IsAssignableFrom(typeof(FlightChangedContract)) => $"{envName}flight-changed-contract",
            // _ when t.IsAssignableFrom(typeof(FlightDetailAssignPassengerContract)) => $"{envName}flight-assign-passengers-contract",
            // _ when t.IsAssignableFrom(typeof(SendingApprovalEmailContract)) => $"{envName}sending-approval-email-contract",
            // _ when t.IsAssignableFrom(typeof(NotifyPassengersForFlightContract)) => $"{envName}notify-passengers-for-flight-contract",
            _ => t.Name.ToLower()
        };
    }
}