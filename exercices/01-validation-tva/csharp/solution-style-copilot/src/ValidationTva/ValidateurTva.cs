using System.Text.RegularExpressions;

namespace ValidationTva;

public static class ValidateurTva
{
    private static readonly Regex Format = new(@"^BE\d{10}$");

    // Valide un numéro de TVA belge (BE + 10 chiffres, clé de contrôle modulo 97).
    public static ResultatValidationTva ValiderNumeroTvaBelge(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return new ResultatValidationTva(false, "Numéro vide.");

        var num = input.Replace(" ", "").Replace(".", "").Replace("-", "").ToUpperInvariant();

        if (!Format.IsMatch(num))
            return new ResultatValidationTva(false, "Format invalide.");

        int entreprise = int.Parse(num.Substring(2, 8));
        int cle = int.Parse(num.Substring(10, 2));
        int reste = entreprise % 97;

        if (reste == 0)
            return new ResultatValidationTva(false, "Numéro invalide (modulo 97 = 0).");

        int cleAttendue = 97 - reste;

        return cle == cleAttendue
            ? new ResultatValidationTva(true, null)
            : new ResultatValidationTva(false, "Clé de contrôle invalide.");
    }
}
