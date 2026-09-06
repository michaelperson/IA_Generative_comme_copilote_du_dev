package be.bstorm.formation.tva;

/**
 * Résultat structuré de la validation d'un numéro de TVA.
 *
 * @param estValide     vrai si le numéro est valide.
 * @param messageErreur message destiné à l'affichage, {@code null} si valide.
 * @param motif         motif de rejet typé, {@code null} si valide.
 */
public record ResultatValidationTva(boolean estValide, String messageErreur, MotifRejet motif) {

    /** Constructeur de compatibilité, sans motif typé. */
    public ResultatValidationTva(boolean estValide, String messageErreur) {
        this(estValide, messageErreur, null);
    }
}
