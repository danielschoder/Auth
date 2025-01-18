using Auth.Domain.Common.Interfaces;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Auth.Infrastructure.Services;

public class ScopedLogger : IScopedLogger
{
    private readonly Queue<string> _logQueue = new();

    public List<string> LogQueueAsList
        => _logQueue.Select((logEntry, index) => $"{index + 1}. {logEntry}").ToList();

    public override string ToString()
        => string.Join("\r\n", _logQueue.Select((logEntry, index) => $"{index + 1}. {logEntry}"));

    public void Log(string message, string callerFilePath, int callerLineNumber)
    {
        string className = Path.GetFileNameWithoutExtension(callerFilePath);
        string logEntry = $"{className} {callerLineNumber}{(message is null ? string.Empty : $": {message}")}";
        _logQueue.Enqueue(logEntry);
    }

    public void LogIf(
        bool condition,
        string message = null,
        [CallerFilePath] string callerFilePath = null,
        [CallerLineNumber] int callerLineNumber = 0)
    {
        if (condition)
        {
            Log(message, callerFilePath, callerLineNumber);
        }
    }

    public void WriteLogsToDebugOutput()
    {
        Debug.WriteLine(string.Empty);
        Debug.WriteLine("--- Logs ---");
        LogQueueToDebugWriteLines();
        Debug.WriteLine(string.Empty);
    }

    public void LogQueueToDebugWriteLines()
    {
        var lineNumber = 0;
        foreach (var logEntry in _logQueue)
        {
            Debug.WriteLine($"{++lineNumber}. {logEntry}");
        }
    }
}
