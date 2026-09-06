package be.bstorm.formation.orderprocessing;

/**
 * Journalisation injectée, extraite du cœur métier.
 *
 * <p>Le code d'origine appelait {@code System.out.println} directement. Deux
 * conséquences : impossible de tester la méthode sans polluer la sortie, et
 * impossible de rediriger les traces vers l'infrastructure de logs réelle sans
 * modifier la logique métier.</p>
 */
public interface OrderLogger {
    void log(String message);
}
