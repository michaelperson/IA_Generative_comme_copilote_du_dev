namespace OrderProcessing;

/// <summary>
/// Abstraction de journalisation injectée dans <see cref="OrderProcessor"/> afin de
/// retirer l'effet de bord <c>Console.WriteLine</c> du cœur métier (étape 3 du
/// refactoring : "retire l'effet de bord ... propose une interface ILogger injectée").
/// </summary>
public interface ILogger
{
    /// <summary>Journalise un message. L'implémentation par défaut écrit sur la console.</summary>
    void Log(string message);
}

/// <summary>
/// Implémentation par défaut de <see cref="ILogger"/>, équivalente au comportement
/// du code legacy (<c>Console.WriteLine</c>) mais désormais interchangeable — par
/// exemple par un logger de test qui capture les messages sans écrire sur la console.
/// </summary>
public class ConsoleLogger : ILogger
{
    public void Log(string message) => Console.WriteLine(message);
}
