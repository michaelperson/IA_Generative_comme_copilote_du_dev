package be.bstorm.formation.orderprocessing;

/**
 * Abstraction de journalisation injectée dans {@link OrderProcessor} pour
 * retirer l'effet de bord System.out.println du cœur métier.
 */
public interface OrderLogger {
    void log(String message);
}
