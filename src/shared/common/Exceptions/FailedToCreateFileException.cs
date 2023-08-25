namespace Common.Exceptions;

public class FailedToCreateFileException:Exception
{
    public override string Message { get; }

    public FailedToCreateFileException()
    {
        this.Message = "Failed to create file";
    }
    public FailedToCreateFileException(string message)
    {
        Message = message;
    }
    
}