using System.Runtime.CompilerServices;

namespace Auth.Domain.Common.Interfaces;

public interface IScopedLogger
{
    List<string> LogQueueAsList { get; }

    string ToString();

    void Log(
        string message = default,
        [CallerFilePath] string callerFilePath = default,
        [CallerLineNumber] int callerLineNumber = default);

    void LogIf(
        bool condition,
        string message = default,
        [CallerFilePath] string callerFilePath = default,
        [CallerLineNumber] int callerLineNumber = default);

    void WriteLogsToDebugOutput();

    void LogQueueToDebugWriteLines();
}
