package be.bstorm.formation.orderprocessing;

/**
 * Implémentation par défaut de {@link OrderLogger} qui écrit sur la console,
 * équivalente au System.out.println du code legacy mais désormais interchangeable.
 */
public class ConsoleOrderLogger implements OrderLogger {
    @Override
    public void log(String message) {
        System.out.println(message);
    }
}
