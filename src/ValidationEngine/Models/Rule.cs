namespace ValidationEngine.Models;

public class Rule
{
    public string FieldName { get; set; }
    public string FieldType { get; set; }
    public string Operator { get; set; }
    public object Value { get; set; }
    public string Entity { get; set; }
    public string ErrorMessage { get; set; }
}