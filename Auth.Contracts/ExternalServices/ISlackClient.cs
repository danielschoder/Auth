namespace Auth.Contracts.ExternalServices;

public interface ISlackClient
{
    void SendMessage(string message);
}
