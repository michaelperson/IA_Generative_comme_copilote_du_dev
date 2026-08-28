import asyncio
from typing import Awaitable, Callable, Optional, TypeVar

T = TypeVar("T")


class RetryPolicyFixed:
    """BONUS — hors périmètre strict de l'exercice (qui porte sur la documentation, pas la
    correction). Variante corrigée de :class:`RetryPolicy` qui montre à quoi ressemble le
    correctif du défaut signalé dans ``INCOHERENCES.md`` : au lieu de lever une ``RuntimeError``
    générique après épuisement des tentatives, on conserve la dernière exception rencontrée et
    on la relance (``raise ... from`` non nécessaire ici : on relance l'objet exception
    d'origine tel quel, traceback compris) une fois les tentatives épuisées.
    """

    def __init__(self, max_attempts: int, base_delay_seconds: float):
        self._max_attempts = max_attempts
        self._base_delay_seconds = base_delay_seconds

    async def execute(self, action: Callable[[], Awaitable[T]]) -> T:
        """Identique à :meth:`RetryPolicy.execute`, à ceci près qu'en cas d'échec de toutes
        les tentatives, c'est la DERNIÈRE exception réellement rencontrée qui est relancée, et
        non plus une ``RuntimeError`` générique masquant la cause originale.
        """
        last_exception: Optional[BaseException] = None

        for attempt in range(1, self._max_attempts + 1):
            try:
                return await action()
            except Exception as exc:
                last_exception = exc
                if attempt < self._max_attempts:
                    delay = self._base_delay_seconds * (2 ** (attempt - 1))
                    await asyncio.sleep(delay)

        # Toutes les tentatives ont échoué : on relance la dernière exception rencontrée
        # plutôt que d'en masquer la cause derrière une exception générique.
        if last_exception is not None:
            raise last_exception
        raise RuntimeError("Unreachable")  # ne devrait jamais être atteint si max_attempts >= 1
