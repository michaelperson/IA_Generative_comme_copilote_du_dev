package be.bstorm.formation.orderprocessing;

/**
 * Responsabilité unique : décider si une commande nécessite une validation manager,
 * et déclencher la notification correspondante.
 *
 * <p>TODO: envoyer un email au manager — jamais implémenté dans le code legacy
 * d'origine. Ce refactoring rend volontairement ce manque visible plutôt que de
 * le faire disparaître silencieusement ou d'inventer un envoi d'email qui n'a
 * jamais existé : voir le débriefing du README de cet exercice.</p>
 */
public class ManagerValidationService {

    private static final double MANAGER_VALIDATION_THRESHOLD = 1000;

    /** Indique si le montant total dépasse le seuil nécessitant une validation manager. */
    public boolean requiresManagerValidation(double total) {
        return total > MANAGER_VALIDATION_THRESHOLD;
    }

    /**
     * Point d'extension destiné à notifier le manager par email. Non implémenté :
     * aucune notification n'est réellement envoyée (incohérence conservée à dessein).
     */
    public void notifyManagerForValidation(double total) {
        // TODO: implémenter l'envoi d'email au manager.
    }
}
