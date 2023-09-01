using Microsoft.Extensions.Logging;

internal static class ConsoleLogger
{
    internal static ILogger Logger => LogFactory.CreateLogger<object>();

    internal static ILoggerFactory LogFactory => s_loggerFactory.Value;

    private static readonly Lazy<ILoggerFactory> s_loggerFactory = new(LogBuilder);

    private static ILoggerFactory LogBuilder()
    {
        return LoggerFactory.Create(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Trace);
            builder.AddConsole();
        });
    }
}