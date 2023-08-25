using System.Linq.Expressions;

namespace ValidationEngine.Models;

public class Operator
{
    public Operator(string name, string[] types, Func<Expression, Expression, Expression> function)
    {
        Name = name;
        Types = types;
        Function = function;
    }
    public string Name { get;private set; }
    public string[] Types { get;private set; }
    public Func<Expression, Expression,Expression> Function { get;private set; }
}