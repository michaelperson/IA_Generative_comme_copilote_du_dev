namespace OrderProcessing;

/// <summary>
/// Implémentation par défaut de <see cref="ILogger"/> qui écrit sur la console,
/// équivalente au Console.WriteLine du code legacy mais désormais interchangeable.
/// </summary>
public class ConsoleLogger : ILogger
{
    public void Log(string message) => Console.WriteLine(message);
}
