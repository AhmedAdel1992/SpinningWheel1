using ValidationEngine.Models;

namespace ValidationEngine.Services;

public class RuleService:IRuleService
{
    private readonly List<Rule> _rules = new();
    public  List<Rule> GetExampleRules()
    {
        return new List<Rule>()
        {
            new Rule() { FieldName = "RefundAmount", FieldType = "decimal", Operator = "greaterthan", Value = 90M,ErrorMessage = "Refund amount should be greater than {value}"},
            new Rule() { FieldName = "PassengerName", FieldType = "string", Operator = "notequals", Value = "Mohamed" ,ErrorMessage = "Passenger name should not be {value}"},
            new Rule() { FieldName = "PassengerName", FieldType = "string", Operator = "notcontains", Value = "sex" ,ErrorMessage = "Passenger name should not contains {value}"},
            new Rule() { FieldName = "From", FieldType = "string", Operator = "equals", Value = "RUH",ErrorMessage = "From should be {value}"},
        };
    }
    public RuleService()
    {
        Initialize();
    }
    private  void Initialize()
    {
        _rules.AddRange(GetExampleRules());
    }
    public List<Rule> GetRules()
    {
        return _rules;
    }
    public List<Rule> GetRules(string entity)
    {
        return _rules.Where(x => x.Entity == entity).ToList();
    }
}

public interface IRuleService
{
    List<Rule> GetExampleRules();
}