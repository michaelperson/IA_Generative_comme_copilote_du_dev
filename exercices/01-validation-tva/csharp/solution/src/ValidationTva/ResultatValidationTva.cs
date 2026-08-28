namespace ValidationTva;

/// <summary>
/// Résultat structuré de la validation d'un numéro de TVA.
/// Volontairement plus riche qu'un simple booléen afin de rester exploitable
/// directement par un appelant (formulaire web, API, etc.).
/// </summary>
/// <param name="EstValide">Vrai si le numéro de TVA est valide.</param>
/// <param name="MessageErreur">Message d'erreur explicite si invalide, null si valide.</param>
public record ResultatValidationTva(bool EstValide, string? MessageErreur);
