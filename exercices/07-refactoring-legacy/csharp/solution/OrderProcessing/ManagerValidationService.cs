namespace OrderProcessing;

/// <summary>
/// Responsabilité unique : décider si une commande nécessite une validation manager,
/// et déclencher la notification correspondante.
/// </summary>
public class ManagerValidationService
{
    private const double ManagerValidationThreshold = 1000;

    /// <summary>
    /// Indique si le montant total dépasse le seuil nécessitant une validation manager.
    /// </summary>
    public bool RequiresManagerValidation(double total) => total > ManagerValidationThreshold;

    // TODO: envoyer un email au manager - jamais implémenté dans le code legacy d'origine.
    // Ce refactoring rend volontairement ce manque visible plutôt que de le faire
    // disparaître silencieusement ou d'inventer un envoi d'email qui n'a jamais existé :
    // voir le débriefing du README de cet exercice.
    /// <summary>
    /// Point d'extension destiné à notifier le manager par email. Non implémenté :
    /// aucune notification n'est réellement envoyée (incohérence conservée à dessein).
    /// </summary>
    public void NotifyManagerForValidation(double total)
    {
        // TODO: implémenter l'envoi d'email au manager.
    }
}
