using System.Text.RegularExpressions;

namespace ValidationTva;

/// <summary>
/// Validation de numéros de TVA intracommunautaires belges.
/// </summary>
/// <remarks>
/// Règle métier :
/// - Format attendu après nettoyage : "BE" suivi de 10 chiffres.
/// - Les 8 premiers chiffres forment le numéro d'entreprise.
/// - Les 2 derniers chiffres forment la clé de contrôle, qui doit être égale à
///   97 - (numéro d'entreprise modulo 97).
/// - Cas limite : si (numéro d'entreprise modulo 97) == 0, la clé attendue vaudrait 97,
///   ce qui n'est pas un cas valide pour ce schéma sur 2 chiffres : le numéro est
///   alors considéré comme invalide, quelle que soit la clé fournie.
/// </remarks>
public static class ValidateurTva
{
    private static readonly Regex FormatBelge = new(@"^BE\d{10}$", RegexOptions.Compiled);

    /// <summary>
    /// Valide un numéro de TVA intracommunautaire belge.
    /// </summary>
    /// <param name="input">
    /// Le numéro de TVA saisi par l'utilisateur. Peut contenir des espaces, points ou tirets
    /// (ex : "BE 0123.456.749") et être null ou vide.
    /// </param>
    /// <returns>
    /// Un <see cref="ResultatValidationTva"/> indiquant si le numéro est valide et,
    /// si ce n'est pas le cas, un message d'erreur explicite.
    /// </returns>
    public static ResultatValidationTva ValiderNumeroTvaBelge(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return new ResultatValidationTva(false, "Le numéro de TVA ne peut pas être vide.");
        }

        string nettoye = Nettoyer(input);

        if (!FormatBelge.IsMatch(nettoye))
        {
            return new ResultatValidationTva(
                false,
                "Format invalide : un numéro de TVA belge doit être \"BE\" suivi de 10 chiffres.");
        }

        string chiffres = nettoye.Substring(2);
        int numeroEntreprise = int.Parse(chiffres.Substring(0, 8));
        int cleControle = int.Parse(chiffres.Substring(8, 2));

        int reste = numeroEntreprise % 97;

        if (reste == 0)
        {
            // Cas limite du modulo 97 : la clé attendue vaudrait 97 (97 - 0), ce qui ne
            // correspond à aucune clé de contrôle valide sur 2 chiffres pour ce schéma.
            // On traite donc explicitement ce cas comme invalide plutôt que de laisser
            // la clé attendue "déborder" à 97.
            return new ResultatValidationTva(
                false,
                "Numéro invalide : le numéro d'entreprise est un multiple de 97 (cas limite du modulo 97).");
        }

        int cleAttendue = 97 - reste;

        if (cleControle != cleAttendue)
        {
            return new ResultatValidationTva(
                false,
                $"Clé de contrôle invalide : attendu {cleAttendue:D2}, reçu {cleControle:D2}.");
        }

        return new ResultatValidationTva(true, null);
    }

    /// <summary>
    /// Nettoie l'entrée utilisateur : supprime espaces, points et tirets, puis met en majuscule.
    /// </summary>
    private static string Nettoyer(string input)
    {
        return input
            .Replace(" ", string.Empty)
            .Replace(".", string.Empty)
            .Replace("-", string.Empty)
            .ToUpperInvariant();
    }
}
