namespace Auth.Application.Interfaces;

public interface IExceptionService
{
    Task HandleExceptionAsync(string message);
}
