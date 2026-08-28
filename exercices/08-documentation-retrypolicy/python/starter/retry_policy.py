import asyncio
from typing import Awaitable, Callable, TypeVar

T = TypeVar("T")

# TODO: cette classe n'a aucune documentation. C'est l'objet de l'exercice :
# utilisez le prompt fourni dans le README pour générer une documentation
# professionnelle (docstrings au format PEP 257) qui explique le POURQUOI
# (backoff exponentiel) et pas seulement le QUOI, et qui signale toute
# incohérence détectée dans le code ci-dessous.
#
# Ne modifiez pas la logique de cette classe : l'exercice porte sur la
# documentation d'un code existant, pas sur sa correction.
class RetryPolicy:
    def __init__(self, max_attempts: int, base_delay_seconds: float):
        self._max_attempts = max_attempts
        self._base_delay_seconds = base_delay_seconds

    async def execute(self, action: Callable[[], Awaitable[T]]) -> T:
        for attempt in range(1, self._max_attempts + 1):
            try:
                return await action()
            except Exception:
                if attempt < self._max_attempts:
                    delay = self._base_delay_seconds * (2 ** (attempt - 1))
                    await asyncio.sleep(delay)
        raise RuntimeError("Unreachable")
