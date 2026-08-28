package be.bstorm.formation.userservice;

/**
 * Exception typée levée par {@link UserServiceImpl} lorsque l'API /users répond
 * avec un code de statut HTTP non-2xx. Porte le code de statut et le corps de la
 * réponse pour permettre à l'appelant de diagnostiquer l'erreur sans avoir à
 * intercepter une exception HTTP générique.
 */
public class UserServiceException extends RuntimeException {

    private final int statusCode;
    private final String responseBody;

    public UserServiceException(int statusCode, String responseBody) {
        super("L'API /users a répondu avec le code HTTP " + statusCode + ".");
        this.statusCode = statusCode;
        this.responseBody = responseBody;
    }

    /** Code de statut HTTP renvoyé par l'API (ex: 404, 500). */
    public int getStatusCode() {
        return statusCode;
    }

    /** Corps brut de la réponse HTTP, tel que renvoyé par l'API. */
    public String getResponseBody() {
        return responseBody;
    }
}
