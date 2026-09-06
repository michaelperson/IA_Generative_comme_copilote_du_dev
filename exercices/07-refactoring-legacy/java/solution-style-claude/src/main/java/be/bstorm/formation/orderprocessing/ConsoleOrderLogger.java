package be.bstorm.formation.orderprocessing;

/** Implémentation par défaut, équivalente au comportement d'origine. */
public class ConsoleOrderLogger implements OrderLogger {
    @Override
    public void log(String message) {
        System.out.println(message);
    }
}
