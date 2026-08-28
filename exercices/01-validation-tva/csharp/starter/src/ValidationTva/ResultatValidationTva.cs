namespace ValidationTva;

/// <summary>
/// Résultat structuré de la validation d'un numéro de TVA.
/// </summary>
/// <param name="EstValide">Vrai si le numéro de TVA est valide.</param>
/// <param name="MessageErreur">Message d'erreur si invalide, null si valide.</param>
public record ResultatValidationTva(bool EstValide, string? MessageErreur);
