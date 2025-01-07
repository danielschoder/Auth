namespace Auth.Application.Interfaces;

public interface ITokenProvider
{
    string Create(string userId);
}
