package be.bstorm.formation.retrypolicy;

import java.util.concurrent.Callable;

// TODO: cette classe n'a aucune documentation. C'est l'objet de l'exercice :
// utilisez le prompt fourni dans le README pour générer une documentation
// professionnelle (Javadoc complet : @param, @param <T>, @return, @throws) qui
// explique le POURQUOI (backoff exponentiel) et pas seulement le QUOI, et qui
// signale toute incohérence détectée dans le code ci-dessous.
//
// Ne modifiez pas la logique de cette classe : l'exercice porte sur la
// documentation d'un code existant, pas sur sa correction.
public class RetryPolicy {

    private final int maxAttempts;
    private final long baseDelayMillis;

    public RetryPolicy(int maxAttempts, long baseDelayMillis) {
        this.maxAttempts = maxAttempts;
        this.baseDelayMillis = baseDelayMillis;
    }

    public <T> T execute(Callable<T> action) throws Exception {
        for (int attempt = 1; attempt <= maxAttempts; attempt++) {
            try {
                return action.call();
            } catch (Exception e) {
                if (attempt < maxAttempts) {
                    long delay = (long) (baseDelayMillis * Math.pow(2, attempt - 1));
                    Thread.sleep(delay);
                }
            }
        }
        throw new IllegalStateException("Unreachable");
    }
}
