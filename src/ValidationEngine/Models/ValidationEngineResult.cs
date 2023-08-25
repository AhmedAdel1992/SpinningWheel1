namespace ValidationEngine.Models;

public class ValidationEngineResult
{
    public ValidationEngineResult()
    {
        IsValid = true;
        Errors = new List<ErrorResult>();
    }
    

    public bool IsValid { get; set; }
    public List<ErrorResult> Errors { get; set; }
    public string Message { get; set; }
}

public class ErrorResult
{
    public ErrorResult()
    {
        
    }
    public ErrorResult(string name, string error)
    {
        Name = name;
        Error = error;
    }
    public string  Name { get; set; }
    public string Error { get; set; }
}