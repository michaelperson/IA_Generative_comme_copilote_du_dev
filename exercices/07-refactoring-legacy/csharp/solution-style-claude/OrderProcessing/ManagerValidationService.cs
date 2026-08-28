namespace OrderProcessing;

/// <summary>
/// Responsabilité unique : décider si une commande nécessite une validation manager,
/// et porter le point d'extension destiné à la notification correspondante.
/// </summary>
/// <remarks>
/// <para>
/// GAP CONNU, VOLONTAIREMENT CONSERVÉ (voir débriefing du README de cet exercice) :
/// le code legacy d'origine contenait un commentaire <c>// TODO: envoyer un email au
/// manager, jamais implémenté</c>. Ce refactoring rend cette incohérence explicitement
/// visible via <see cref="NotifyManagerForValidation"/> plutôt que de la faire
/// disparaître silencieusement ou d'inventer un envoi d'email qui n'a jamais existé
/// dans le comportement d'origine — un bon refactoring assisté par IA doit signaler ce
/// type d'incohérence, pas la masquer.
/// </para>
/// </remarks>
public class ManagerValidationService
{
    private const double ManagerValidationThreshold = 1000;

    /// <summary>
    /// Indique si le montant total de la commande dépasse le seuil nécessitant une
    /// validation manager.
    /// </summary>
    public bool RequiresManagerValidation(double total) => total > ManagerValidationThreshold;

    /// <summary>
    /// Point d'extension destiné à notifier le manager par email lorsqu'une commande
    /// dépasse le seuil de validation.
    /// </summary>
    /// <remarks>
    /// NON IMPLÉMENTÉ À DESSEIN : aucune notification n'est réellement envoyée,
    /// exactement comme dans le code legacy d'origine. Ne pas combler ce TODO sans
    /// validation explicite du métier — voir la remarque de classe ci-dessus.
    /// </remarks>
    public void NotifyManagerForValidation(double total)
    {
        // TODO: implémenter l'envoi d'email au manager (jamais fait dans le legacy d'origine).
    }
}
