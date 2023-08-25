using System.Linq.Expressions;
using System.Reflection;
using ValidationEngine.Interfaces;
using ValidationEngine.Models;

namespace ValidationEngine.Services;

internal class OperatorService:IOperatorService
{
    private readonly List<Operator> _operators = new();
    private readonly List<string> _types = new(){"string","int","decimal","datetime"};
    public OperatorService()
    {
        Initialize();
    }
    private  void Initialize()
    {
        _operators.Add(new Operator("equals", new[] { "string","int","decimal","datetime" }, Expression.Equal));
        _operators.Add(new Operator("notequals", new[] { "string","int","decimal","datetime" }, Expression.NotEqual));
        _operators.Add(new Operator("greaterthan", new[] { "int","decimal","datetime" }, Expression.GreaterThan));
        _operators.Add(new Operator("lessthan", new[] { "int","decimal","datetime" }, Expression.LessThan));
        _operators.Add(new Operator("contains", new[] { "string" }, 
            (left,right)=>Expression.Call(left,"Contains",null,right)));
        _operators.Add(new Operator("notcontains", new[] { "string" }, 
            (left,right)=>Expression.Not(Expression.Call(left,"Contains",null,right))));
    }
    public Expression GetComparisonExpression(Expression left, Expression right, string @operator)
    {
        var op = _operators.FirstOrDefault(x => x.Name == @operator);
        if (op == null)
        {
            throw new NotSupportedException();
        }
        if (!op.Types.Contains(left.Type.Name.ToLower()))
        {
            throw new NotSupportedException();
        }
        return op.Function(left, right);
    }
    public List<string> GetOperators()
    {
        return _operators.Select(s=>s.Name).ToList();
    }
    public List<string> GetOperators(string type)
    {
        return _operators.Where(x => x.Types.Contains(type.ToLower())).Select(s=>s.Name).ToList();
    }
    public List<string> GetTypes()
    {
        return _types;
    }
}

public class EntityService:IEntityService
{
    private readonly Dictionary<string,List<EntityFieldInfo>>_entities=new();

    void AddEntity(string entity)
    {
        if (!_entities.ContainsKey(entity))
        {
            _entities.Add(entity, new());
        }
    }
    public void AddField(string entity,PropertyInfo field)
    {
        AddEntity(entity);
        _entities[entity].Add(new(field.Name,field.PropertyType.Name));
    }

    public void AddFields(string entity, PropertyInfo[] fields)
    {
        foreach (var field in fields)
        {
            AddField(entity,field);
        }
    }

    public IEnumerable<string> GetEntities()
    {
        return _entities.Keys;
    }

    public IEnumerable<EntityFieldInfo> GetFields(string entity)
    {
        return _entities[entity];
    }

}
 public   class EntityFieldInfo
    {
        public EntityFieldInfo(string name, string type)
        {
            Name = name;
            Type = type;
        }
        public string Name { get; set; }
        public string Type { get; set; }
    }

public interface IEntityService
{
    void AddField(string entity, PropertyInfo field);
    void AddFields(string entity, PropertyInfo[] fields);
    IEnumerable<string> GetEntities();
    IEnumerable<EntityFieldInfo> GetFields(string entity);
}