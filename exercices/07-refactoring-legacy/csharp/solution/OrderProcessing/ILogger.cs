namespace OrderProcessing;

/// <summary>
/// Abstraction de journalisation injectée dans <see cref="OrderProcessor"/> pour retirer
/// l'effet de bord Console.WriteLine du cœur métier.
/// </summary>
public interface ILogger
{
    void Log(string message);
}
