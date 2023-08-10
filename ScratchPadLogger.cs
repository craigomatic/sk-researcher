using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Logging;

internal class ScratchPadLogger : ILogger
{
    public ScratchPad ScratchPad { get; set; } = new();

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return default;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (formatter is not null)
        {
            var message = formatter(state, exception);

            if (!string.IsNullOrEmpty(message))
            {
                var currentThought = _ExtractItem(message, "[THOUGHT]");
                var currentAction = _ExtractItem(message, "[ACTION]");

                if (!string.IsNullOrWhiteSpace(currentThought))
                {
                    if (!this.ScratchPad.Thoughts.Contains(currentThought))
                    {
                        this.ScratchPad.Thoughts.Add(currentThought);

                        Console.WriteLine($"Thought: {this.ScratchPad.Thoughts.Last()}");
                        Console.WriteLine();
                    }
                }

                if (!string.IsNullOrWhiteSpace(currentAction))
                {
                    this.ScratchPad.References.Add(currentAction);

                    Console.WriteLine($"References: {this.ScratchPad.References.Last()}");
                    Console.WriteLine();
                }
            }
        }
    }

    private string? _ExtractItem(string message, string item)
    {
        var itemIndex = message.IndexOf(item);

        if (itemIndex > -1)
        {
            try
            {
                var itemValue = message.Substring(
                               itemIndex + item.Length,
                                              message.Substring(itemIndex + item.Length).IndexOf("\n"));

                return itemValue;
            }
            catch 
            {
                Console.WriteLine($"[broken {item}] {message}");
            }            
        }

        return default;
    }
}