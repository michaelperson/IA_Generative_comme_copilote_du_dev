package be.bstorm.formation.catalogue;

/** Levée lorsque le pourcentage de remise sort de l'intervalle autorisé. */
public class PourcentageRemiseInvalideException extends RuntimeException {

    private final int pourcentage;

    public PourcentageRemiseInvalideException(int pourcentage) {
        super("Le pourcentage de remise doit être un entier de 0 à 100, reçu " + pourcentage + ".");
        this.pourcentage = pourcentage;
    }

    public int getPourcentage() {
        return pourcentage;
    }
}
