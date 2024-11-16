namespace Auth.Contracts.Extensions;

internal static class ValidateExtensions
{
    public static string Error(this Dictionary<Func<bool>, string> conditions)
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
