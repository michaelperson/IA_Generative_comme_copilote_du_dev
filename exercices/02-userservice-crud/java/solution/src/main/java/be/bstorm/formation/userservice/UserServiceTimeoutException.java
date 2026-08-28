package be.bstorm.formation.userservice;

/**
 * Exception typée levée lorsque l'appel à l'API /users dépasse le délai
 * de 10 secondes. Équivalent Java du {@code TimeoutException} explicite
 * demandé dans la consigne côté C#.
 */
public class UserServiceTimeoutException extends RuntimeException {

    public UserServiceTimeoutException(String message, Throwable cause) {
        super(message, cause);
    }
}
