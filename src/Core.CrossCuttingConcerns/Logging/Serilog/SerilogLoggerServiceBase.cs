using Core.CrossCuttingConcerns.Logging.Abstraction;
using PackageSerilog = Serilog;

namespace Core.CrossCuttingConcerns.Logging.Serilog;

public abstract class SerilogLoggerServiceBase(PackageSerilog.ILogger logger) : ILogger
{
    public void Critical(string message)
    {
        logger?.Fatal(message);
    }

    public void Debug(string message)
    {
        logger?.Debug(message);
    }

    public void Error(string message)
    {
        logger?.Error(message);
    }

    public void Information(string message)
    {
        logger?.Information(message);
    }

    public void Trace(string message)
    {
        logger?.Verbose(message);
    }

    public void Warning(string message)
    {
        logger?.Warning(message);
    }
}
