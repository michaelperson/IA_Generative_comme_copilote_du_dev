package be.bstorm.formation.userservice;

/**
 * Résultat brut d'une requête HTTP effectuée par un {@link HttpTransport} :
 * code de statut et corps de la réponse (JSON attendu, sous forme de chaîne).
 */
public record HttpTransportResponse(int statusCode, String body) {
}
