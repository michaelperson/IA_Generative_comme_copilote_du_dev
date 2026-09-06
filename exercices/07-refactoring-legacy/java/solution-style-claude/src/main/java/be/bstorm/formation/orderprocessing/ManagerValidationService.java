package be.bstorm.formation.orderprocessing;

/**
 * Décide si une commande requiert une validation manager.
 *
 * <h2>Gap connu, hérité du code d'origine</h2>
 *
 * <p>Le code legacy portait le commentaire
 * {@code // TODO: envoyer un email au manager, jamais implémenté}. Ce
 * comportement n'a <strong>jamais existé</strong>. Deux tentations se présentent
 * pendant un refactoring assisté, toutes deux mauvaises :</p>
 *
 * <ol>
 *   <li>faire disparaître le TODO avec le code qui l'entourait — l'information
 *       est alors perdue, et personne ne saura plus que la notification
 *       manquait ;</li>
 *   <li>implémenter l'envoi de sa propre initiative — on introduit un
 *       comportement nouveau que les tests de caractérisation ne détecteront
 *       pas, puisqu'ils ne le testaient pas.</li>
 * </ol>
 *
 * <p>Troisième voie, retenue ici : rendre le manque explicite et bloquant si
 * quelqu'un l'appelle, pour qu'il soit spécifié avant d'être implémenté.</p>
 */
public class ManagerValidationService {

    private static final double SEUIL_VALIDATION_MANAGER = 1000;

    public boolean requiresValidation(double total) {
        return total > SEUIL_VALIDATION_MANAGER;
    }

    public void notifyManager(double total) {
        throw new UnsupportedOperationException(
                "Notification du manager non implémentée — comportement absent du code "
                        + "d'origine, à spécifier avant toute implémentation.");
    }
}
