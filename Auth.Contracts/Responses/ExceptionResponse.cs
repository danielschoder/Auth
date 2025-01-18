namespace Auth.Contracts.Responses;

public class ExceptionResponse(string message, List<string> logs = default)
{
    public ExceptionResponse() : this(default, default) { }

    public string Exception { get; set; } = message;

    public List<string> Logs { get; set; } = logs;
}
