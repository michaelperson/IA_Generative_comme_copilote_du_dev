using System.Text.RegularExpressions;

namespace ValidationTva;

/// <summary>
/// Fournit la validation des numéros de TVA intracommunautaires belges.
/// </summary>
/// <remarks>
/// <para>
/// Un numéro de TVA belge valide, une fois nettoyé, respecte le format
/// "BE" + 10 chiffres. Les 8 premiers chiffres constituent le numéro d'entreprise ;
/// les 2 derniers constituent la clé de contrôle, définie comme
/// <c>97 - (numéro d'entreprise modulo 97)</c>.
/// </para>
/// <para>
/// Cas limite important : si le numéro d'entreprise est un multiple exact de 97
/// (modulo 97 égal à 0), la formule donnerait une clé de 97, ce qui ne peut pas être
/// représenté par une clé de contrôle à 2 chiffres cohérente avec ce schéma. Ce cas
/// est donc traité explicitement comme invalide, quelle que soit la valeur des 2
/// derniers chiffres fournis.
/// </para>
/// <para>
/// Cette classe ne fait aucune I/O et n'a aucun effet de bord : elle est donc
/// entièrement testable unitairement.
/// </para>
/// </remarks>
public static class ValidateurTva
{
    /// <summary>Longueur attendue du numéro d'entreprise (8 chiffres).</summary>
    private const int LongueurNumeroEntreprise = 8;

    /// <summary>Longueur attendue de la clé de contrôle (2 chiffres).</summary>
    private const int LongueurCleControle = 2;

    /// <summary>Diviseur utilisé pour le calcul de la clé de contrôle.</summary>
    private const int ModuloCleControle = 97;

    /// <summary>Préfixe attendu pour un numéro de TVA belge.</summary>
    private const string PrefixePays = "BE";

    private static readonly Regex FormatNumeroBelge =
        new($"^{PrefixePays}\\d{{{LongueurNumeroEntreprise + LongueurCleControle}}}$", RegexOptions.Compiled);

    /// <summary>
    /// Valide un numéro de TVA intracommunautaire belge saisi par un utilisateur.
    /// </summary>
    /// <param name="input">
    /// Le numéro tel que saisi, potentiellement avec espaces, points ou tirets
    /// (ex : "BE 0123.456.749"), et potentiellement <c>null</c> ou vide.
    /// </param>
    /// <returns>
    /// Un <see cref="ResultatValidationTva"/> qui indique si le numéro est valide et,
    /// dans le cas contraire, pourquoi il a été rejeté.
    /// </returns>
    public static ResultatValidationTva ValiderNumeroTvaBelge(string? input)
    {
        // Défense en profondeur : on gère explicitement l'absence de valeur avant
        // toute autre opération, pour ne jamais lever d'exception sur une entrée
        // utilisateur imparfaite.
        if (input is null)
        {
            return Invalide("Le numéro de TVA est requis (valeur null).");
        }

        if (string.IsNullOrWhiteSpace(input))
        {
            return Invalide("Le numéro de TVA est requis (valeur vide).");
        }

        string numeroNettoye = Nettoyer(input);

        if (!RespecteLeFormat(numeroNettoye))
        {
            return Invalide(
                $"Format invalide : un numéro de TVA belge doit être \"{PrefixePays}\" " +
                $"suivi de {LongueurNumeroEntreprise + LongueurCleControle} chiffres " +
                "(reçu après nettoyage : \"" + numeroNettoye + "\").");
        }

        int numeroEntreprise = ExtraireNumeroEntreprise(numeroNettoye);
        int cleControleFournie = ExtraireCleControle(numeroNettoye);
        int reste = numeroEntreprise % ModuloCleControle;

        if (EstCasLimiteModuloZero(reste))
        {
            return Invalide(
                "Numéro invalide : le numéro d'entreprise est un multiple exact de 97 " +
                "(cas limite du modulo 97, aucune clé de contrôle valide ne peut être dérivée).");
        }

        int cleControleAttendue = CalculerCleAttendue(reste);

        if (cleControleFournie != cleControleAttendue)
        {
            return Invalide(
                $"Clé de contrôle invalide : attendu {cleControleAttendue:D2}, " +
                $"reçu {cleControleFournie:D2} pour le numéro d'entreprise {numeroEntreprise:D8}.");
        }

        return new ResultatValidationTva(EstValide: true, MessageErreur: null);
    }

    /// <summary>
    /// Supprime les espaces, points et tirets éventuellement saisis par l'utilisateur,
    /// puis met le résultat en majuscules pour tolérer une saisie en minuscules.
    /// </summary>
    private static string Nettoyer(string input)
    {
        return input
            .Replace(" ", string.Empty)
            .Replace(".", string.Empty)
            .Replace("-", string.Empty)
            .ToUpperInvariant();
    }

    /// <summary>
    /// Vérifie que la chaîne nettoyée respecte strictement le format "BE" + 10 chiffres.
    /// </summary>
    private static bool RespecteLeFormat(string numeroNettoye) => FormatNumeroBelge.IsMatch(numeroNettoye);

    /// <summary>
    /// Extrait les 8 chiffres du numéro d'entreprise (juste après le préfixe pays).
    /// </summary>
    private static int ExtraireNumeroEntreprise(string numeroNettoye)
    {
        string partie = numeroNettoye.Substring(PrefixePays.Length, LongueurNumeroEntreprise);
        return int.Parse(partie);
    }

    /// <summary>
    /// Extrait les 2 derniers chiffres correspondant à la clé de contrôle fournie.
    /// </summary>
    private static int ExtraireCleControle(string numeroNettoye)
    {
        string partie = numeroNettoye.Substring(PrefixePays.Length + LongueurNumeroEntreprise, LongueurCleControle);
        return int.Parse(partie);
    }

    /// <summary>
    /// Indique si le reste du modulo correspond au cas limite (numéro d'entreprise
    /// multiple exact de 97), qui doit toujours être considéré comme invalide.
    /// </summary>
    private static bool EstCasLimiteModuloZero(int reste) => reste == 0;

    /// <summary>
    /// Calcule la clé de contrôle attendue à partir du reste de la division par 97.
    /// Ne doit jamais être appelée lorsque <paramref name="reste"/> vaut 0
    /// (voir <see cref="EstCasLimiteModuloZero"/>).
    /// </summary>
    private static int CalculerCleAttendue(int reste) => ModuloCleControle - reste;

    /// <summary>
    /// Petite fabrique pour garder les retours d'erreur homogènes et lisibles.
    /// </summary>
    private static ResultatValidationTva Invalide(string message) => new(EstValide: false, MessageErreur: message);
}
