import asyncio
from typing import Awaitable, Callable, TypeVar

T = TypeVar("T")


class RetryPolicy:
    """Fiabilise l'exécution d'un appel réseau vers un service externe instable en le
    ré-essayant automatiquement selon une stratégie de backoff exponentiel.

    Le backoff exponentiel (le délai entre deux tentatives double à chaque nouvel échec :
    1x, 2x, 4x, 8x... la valeur de ``base_delay_seconds``) existe pour une raison précise :
    lorsqu'un service externe est déjà en difficulté (surcharge, redémarrage, dégradation
    temporaire), ré-essayer immédiatement et à intervalle fixe ne fait qu'ajouter de la
    pression sur un système qui a justement besoin de temps pour se rétablir. Espacer les
    tentatives de façon croissante laisse au service la marge nécessaire pour se stabiliser
    avant le prochain appel, tout en évitant de multiplier les tentatives synchronisées de
    nombreux clients au même instant (effet de "troupeau").

    INCOHÉRENCE DÉTECTÉE (voir aussi ``INCOHERENCES.md``) : lorsque toutes les tentatives ont
    échoué, :meth:`execute` ne propage PAS la dernière exception réellement rencontrée. Elle
    lève à la place une ``RuntimeError`` générique avec le message "Unreachable", ce qui masque
    complètement la cause originale de l'échec (type d'exception, message, traceback de l'appel
    réseau). C'est un défaut réel du code, pas un choix de conception volontaire : un appelant
    qui catch cette exception ne peut ni la distinguer d'un vrai bug interne, ni savoir pourquoi
    l'appel a échoué.
    """

    def __init__(self, max_attempts: int, base_delay_seconds: float):
        """Crée une politique de retry avec backoff exponentiel.

        Args:
            max_attempts: Nombre maximum de tentatives (la tentative initiale comprise).
                Par exemple, ``max_attempts = 3`` signifie un appel initial et jusqu'à deux
                ré-essais.
            base_delay_seconds: Délai de base (en secondes) utilisé pour calculer le backoff
                exponentiel. Le délai réel avant la tentative ``n`` vaut
                ``base_delay_seconds * 2^(n-1)``.
        """
        self._max_attempts = max_attempts
        self._base_delay_seconds = base_delay_seconds

    async def execute(self, action: Callable[[], Awaitable[T]]) -> T:
        """Exécute ``action`` en la ré-essayant automatiquement, avec un délai croissant
        exponentiellement entre chaque tentative, tant que le nombre maximum de tentatives
        n'est pas atteint.

        Le backoff exponentiel entre deux tentatives protège le service externe appelé :
        plutôt que de marteler un service déjà instable à intervalle fixe, on lui laisse de
        plus en plus de temps pour se rétablir avant chaque nouvelle tentative.

        Args:
            action: L'opération asynchrone à exécuter (typiquement un appel réseau), sans
                argument, retournant un ``Awaitable[T]``.

        Returns:
            Le résultat de ``action`` dès qu'une tentative réussit.

        Raises:
            RuntimeError: Levée après épuisement de toutes les tentatives
                (``max_attempts`` échecs). ATTENTION — comportement actuel à corriger : cette
                exception est TOUJOURS générique ("Unreachable") et NE PORTE PAS la dernière
                exception réellement rencontrée pendant les tentatives. La cause originale de
                l'échec (le vrai type d'exception, son message, son traceback) est
                silencieusement perdue. Voir ``RetryPolicyFixed`` (dans
                ``retry_policy_fixed.py``) pour un correctif qui relance la dernière exception
                capturée.
        """
        for attempt in range(1, self._max_attempts + 1):
            try:
                return await action()
            except Exception:
                if attempt < self._max_attempts:
                    delay = self._base_delay_seconds * (2 ** (attempt - 1))
                    await asyncio.sleep(delay)
        raise RuntimeError("Unreachable")
