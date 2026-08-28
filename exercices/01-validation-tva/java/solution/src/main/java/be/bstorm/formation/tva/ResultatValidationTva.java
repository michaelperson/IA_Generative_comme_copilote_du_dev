package be.bstorm.formation.tva;

/**
 * Résultat structuré de la validation d'un numéro de TVA.
 * Volontairement plus riche qu'un simple booléen afin de rester exploitable
 * directement par un appelant (formulaire web, API, etc.).
 *
 * @param estValide     vrai si le numéro de TVA est valide.
 * @param messageErreur message d'erreur explicite si invalide, {@code null} si valide.
 */
public record ResultatValidationTva(boolean estValide, String messageErreur) {
}
