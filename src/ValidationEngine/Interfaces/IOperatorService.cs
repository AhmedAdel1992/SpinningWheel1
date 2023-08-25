using System.Linq.Expressions;
using ValidationEngine.Models;

namespace ValidationEngine.Interfaces;

public interface IOperatorService
{
    Expression GetComparisonExpression(Expression left, Expression right, string @operator);
    List<string> GetOperators();
    List<string> GetOperators(string type);
    List<string> GetTypes();
}