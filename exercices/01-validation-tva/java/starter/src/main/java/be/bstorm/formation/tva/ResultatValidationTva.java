package be.bstorm.formation.tva;

/**
 * Résultat structuré de la validation d'un numéro de TVA.
 *
 * @param estValide     vrai si le numéro de TVA est valide.
 * @param messageErreur message d'erreur si invalide, {@code null} si valide.
 */
public record ResultatValidationTva(boolean estValide, String messageErreur) {
}
