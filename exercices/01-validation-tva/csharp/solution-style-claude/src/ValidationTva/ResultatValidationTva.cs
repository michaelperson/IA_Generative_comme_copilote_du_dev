namespace ValidationTva;

/// <summary>
/// Représente le résultat de la validation d'un numéro de TVA intracommunautaire.
/// </summary>
/// <remarks>
/// On retourne volontairement un type structuré plutôt qu'un simple <see cref="bool"/> :
/// cela permet à l'appelant (formulaire web, API, script de migration, etc.) d'afficher
/// un message d'erreur pertinent à l'utilisateur sans avoir à ré-implémenter la logique
/// de validation pour savoir "pourquoi" le numéro est refusé.
/// </remarks>
/// <param name="EstValide">Vrai si et seulement si le numéro de TVA respecte le format et la clé de contrôle.</param>
/// <param name="MessageErreur">
/// Message d'erreur destiné à l'utilisateur ou aux logs, expliquant la raison du rejet.
/// Vaut toujours <c>null</c> lorsque <see cref="EstValide"/> est <c>true</c>.
/// </param>
public record ResultatValidationTva(bool EstValide, string? MessageErreur);
