package be.bstorm.formation.orderprocessing;

public class ConsoleOrderLogger implements OrderLogger {
    @Override
    public void log(String message) {
        System.out.println(message);
    }
}
