namespace Auth.Contracts.Extensions;

public static class ValidateExtensions
{
    public static string Validate(this Dictionary<Func<bool>, string> conditions)
    {
        foreach (var condition in conditions)
        {
            if (condition.Key())
            {
                return condition.Value;
            }
        }
        return null;
    }
}
