using System.Linq.Expressions;
using ValidationEngine.Interfaces;
using ValidationEngine.Models;

namespace ValidationEngine.Services;

internal class ValidationBuilder: IValidationBuilder
{
    private readonly List<Rule> _rules;
    private readonly IOperatorService _operatorService;

    public ValidationBuilder(IRuleService rules, IOperatorService operatorService)
    {
        _rules = rules.GetExampleRules();
        _operatorService = operatorService;
    }

    public ValidationEngineResult Validate<T>(T request)where T : ISetting
    {
        var typeParams = Expression.Parameter(request.GetType(), "p");
        var validationResult = new ValidationEngineResult();
        foreach (var rule in from rule in _rules 
                 let left = Expression.PropertyOrField(typeParams, rule.FieldName) 
                 let right = Expression.Constant(rule.Value) 
                 let binaryExpression =_operatorService.GetComparisonExpression(left,right,rule.Operator)
                 let lambda = Expression.Lambda<Func<T, bool>>(binaryExpression, typeParams) 
                 let compiled = lambda.Compile() 
                 let result = compiled(request) 
                 where !result select rule)
        {
            validationResult.Message = "Validation failed";
            validationResult.IsValid = false;
            var error=string.IsNullOrEmpty(rule.ErrorMessage)? $"{rule.FieldName} should be {rule.Operator} {rule.Value}." :
                rule.ErrorMessage.Replace("{value}", rule.Value.ToString());
            validationResult.Errors.Add( new(rule.FieldName,error));
        }
        if (validationResult.IsValid)
        {
            validationResult.Message= "Validation passed";
        }
        return validationResult;
    }

}