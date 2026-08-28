package be.bstorm.formation.notification;

/**
 * Contrat commun à tous les canaux de notification.
 */
public interface Notification {

    /**
     * Envoie le message via le canal concret et retourne une confirmation
     * décrivant l'envoi (simulation, aucun appel réseau réel n'est effectué).
     */
    String send(String message);
}
