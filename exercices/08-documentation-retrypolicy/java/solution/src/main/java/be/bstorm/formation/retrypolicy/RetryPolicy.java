package be.bstorm.formation.retrypolicy;

import java.util.concurrent.Callable;

/**
 * Fiabilise l'exécution d'un appel réseau vers un service externe instable en le
 * ré-essayant automatiquement selon une stratégie de backoff exponentiel.
 *
 * <p>Le backoff exponentiel (le délai entre deux tentatives double à chaque nouvel échec :
 * 1x, 2x, 4x, 8x... la valeur de {@code baseDelayMillis}) existe pour une raison précise :
 * lorsqu'un service externe est déjà en difficulté (surcharge, redémarrage, dégradation
 * temporaire), ré-essayer immédiatement et à intervalle fixe ne fait qu'ajouter de la pression
 * sur un système qui a justement besoin de temps pour se rétablir. Espacer les tentatives de
 * façon croissante laisse au service la marge nécessaire pour se stabiliser avant le prochain
 * appel, tout en évitant de multiplier les tentatives synchronisées de nombreux clients au même
 * instant (effet de "troupeau").</p>
 *
 * <p><b>INCOHÉRENCE DÉTECTÉE</b> (voir aussi {@code INCOHERENCES.md}) : lorsque toutes les
 * tentatives ont échoué, {@link #execute(Callable)} ne propage PAS la dernière exception
 * réellement rencontrée. Elle lève à la place une {@link IllegalStateException} générique avec
 * le message "Unreachable", ce qui masque complètement la cause originale de l'échec (message,
 * type d'exception, stack trace du service appelé). C'est un défaut réel du code, pas un choix
 * de conception volontaire : un appelant qui catch cette exception ne peut ni la distinguer d'un
 * vrai bug interne, ni savoir pourquoi l'appel a échoué.</p>
 */
public class RetryPolicy {

    private final int maxAttempts;
    private final long baseDelayMillis;

    /**
     * Crée une politique de retry avec backoff exponentiel.
     *
     * @param maxAttempts     nombre maximum de tentatives (la tentative initiale comprise).
     *                        Par exemple, {@code maxAttempts = 3} signifie un appel initial et
     *                        jusqu'à deux ré-essais.
     * @param baseDelayMillis délai de base (en millisecondes) utilisé pour calculer le backoff
     *                        exponentiel. Le délai réel avant la tentative {@code n} vaut
     *                        {@code baseDelayMillis * 2^(n-1)}.
     */
    public RetryPolicy(int maxAttempts, long baseDelayMillis) {
        this.maxAttempts = maxAttempts;
        this.baseDelayMillis = baseDelayMillis;
    }

    /**
     * Exécute {@code action} en la ré-essayant automatiquement, avec un délai croissant
     * exponentiellement entre chaque tentative, tant que le nombre maximum de tentatives n'est
     * pas atteint.
     *
     * <p>Le backoff exponentiel entre deux tentatives protège le service externe appelé : plutôt
     * que de marteler un service déjà instable à intervalle fixe, on lui laisse de plus en plus
     * de temps pour se rétablir avant chaque nouvelle tentative.</p>
     *
     * @param <T>    type du résultat retourné par {@code action}.
     * @param action l'opération à exécuter (typiquement un appel réseau).
     * @return le résultat de {@code action} dès qu'une tentative réussit.
     * @throws IllegalStateException levée après épuisement de toutes les tentatives
     *     ({@code maxAttempts} échecs). <b>ATTENTION — comportement actuel à corriger :</b> cette
     *     exception est TOUJOURS générique ("Unreachable") et NE PORTE PAS la dernière exception
     *     réellement rencontrée pendant les tentatives. La cause originale de l'échec (le vrai
     *     type d'exception, son message, sa stack trace) est silencieusement perdue. Voir
     *     {@code RetryPolicyFixed} pour un correctif qui repropage la dernière exception capturée.
     * @throws Exception si {@code action} lève une exception vérifiée ; celle-ci est capturée en
     *     interne à chaque tentative (voir l'incohérence ci-dessus) et n'est actuellement jamais
     *     relancée telle quelle.
     */
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
