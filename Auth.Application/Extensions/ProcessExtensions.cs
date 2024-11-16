using MediatR;

namespace Auth.Application.Extensions;

public static class ProcessExtensions
{
    public static async Task<T2> Error<T, T2>(this IRequest<T> commmand, List<(Func<Task<bool>>, T2)> processors)
    {
        foreach (var (function, ErrorResponse) in processors)
        {
            if (!await function()) { return ErrorResponse; }
        }
        return default;
    }
}
