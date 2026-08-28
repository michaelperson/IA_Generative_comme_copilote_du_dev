package be.bstorm.formation.retrypolicy;

import java.util.concurrent.Callable;

/**
 * BONUS — hors périmètre strict de l'exercice (qui porte sur la documentation, pas la
 * correction). Variante corrigée de {@link RetryPolicy} qui montre à quoi ressemble le correctif
 * du défaut signalé dans {@code INCOHERENCES.md} : au lieu de lever une
 * {@link IllegalStateException} générique après épuisement des tentatives, on conserve la
 * dernière exception rencontrée et on la relance telle quelle une fois les tentatives épuisées.
 */
public class RetryPolicyFixed {

    private final int maxAttempts;
    private final long baseDelayMillis;

    public RetryPolicyFixed(int maxAttempts, long baseDelayMillis) {
        this.maxAttempts = maxAttempts;
        this.baseDelayMillis = baseDelayMillis;
    }

    /**
     * Identique à {@link RetryPolicy#execute(Callable)}, à ceci près qu'en cas d'échec de
     * toutes les tentatives, c'est la DERNIÈRE exception réellement rencontrée qui est relancée,
     * et non plus une {@link IllegalStateException} générique masquant la cause originale.
     */
    public <T> T execute(Callable<T> action) throws Exception {
        Exception lastException = null;

        for (int attempt = 1; attempt <= maxAttempts; attempt++) {
            try {
                return action.call();
            } catch (Exception e) {
                lastException = e;
                if (attempt < maxAttempts) {
                    long delay = (long) (baseDelayMillis * Math.pow(2, attempt - 1));
                    Thread.sleep(delay);
                }
            }
        }

        // Toutes les tentatives ont échoué : on relance la dernière exception rencontrée
        // plutôt que d'en masquer la cause derrière une exception générique.
        if (lastException != null) {
            throw lastException;
        }
        throw new IllegalStateException("Unreachable"); // ne devrait jamais être atteint si maxAttempts >= 1
    }
}
