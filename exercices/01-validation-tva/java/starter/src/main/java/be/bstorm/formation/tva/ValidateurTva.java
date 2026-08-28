package be.bstorm.formation.tva;

/**
 * Validation de numéros de TVA intracommunautaires belges.
 */
public final class ValidateurTva {

    private ValidateurTva() {
    }

    /**
     * Valide un numéro de TVA intracommunautaire belge (format "BE" + 10 chiffres,
     * avec vérification de la clé de contrôle modulo 97 sur les 8 premiers chiffres
     * du numéro d'entreprise).
     *
     * @param input le numéro de TVA saisi (peut contenir espaces, points, tirets).
     * @return un résultat structuré indiquant la validité et un éventuel message d'erreur.
     */
    public static ResultatValidationTva validerNumeroTvaBelge(String input) {
        // TODO: 1. Nettoyer l'entrée (espaces, points, tirets, majuscules).
        // TODO: 2. Vérifier le format "BE" + 10 chiffres.
        // TODO: 3. Extraire numéro d'entreprise (8 premiers chiffres) et clé de contrôle (2 derniers).
        // TODO: 4. Calculer la clé attendue = 97 - (numéro d'entreprise modulo 97).
        //          Cas limite : si (numéro d'entreprise modulo 97) == 0, le numéro est INVALIDE.
        // TODO: 5. Gérer explicitement le cas d'une entrée null ou vide.
        // TODO: 6. Retourner un ResultatValidationTva (jamais un simple booléen).
        throw new UnsupportedOperationException("À implémenter : voir consigne de l'exercice 01-validation-tva.");
    }
}
