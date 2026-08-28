"""Version corrigée à la suite de la revue de code (voir REVIEW.md)."""
import asyncio
import logging

# httpx est le client HTTP asynchrone déjà utilisé par le reste du service
# pour appeler l'API des commandes (pas de nouvelle dépendance introduite ici).
import httpx

logger = logging.getLogger(__name__)

# Nombre de tentatives avant abandon en cas d'échec de l'appel API.
MAX_RETRY_ATTEMPTS = 3

# Délai d'attente entre deux tentatives, en secondes.
RETRY_DELAY_SECONDS = 30


class OrderService:
    """Récupère les commandes en attente depuis l'API des commandes et les traite."""

    def __init__(self, client: httpx.AsyncClient):
        self.client = client

    async def fetch_and_process_orders(
        self, cancellation_event: asyncio.Event | None = None
    ) -> int:
        """Récupère les commandes en attente et retourne le nombre de commandes traitées.

        Args:
            cancellation_event: équivalent Python d'un CancellationToken. Si l'événement
                est déclenché par l'appelant, l'opération est annulée avant la tentative
                suivante.
        """
        processed_count = 0
        cancellation_event = cancellation_event or asyncio.Event()

        for attempt in range(MAX_RETRY_ATTEMPTS):
            if cancellation_event.is_set():
                raise asyncio.CancelledError("Opération annulée par l'appelant")

            try:
                response = await self.client.get("https://api.example.com/orders/pending")
                orders = response.json()
                processed_count += len(orders)
                return processed_count
            except Exception:
                logger.exception(
                    "Échec de la tentative %s/%s de récupération des commandes en attente",
                    attempt + 1,
                    MAX_RETRY_ATTEMPTS,
                )

            await asyncio.sleep(RETRY_DELAY_SECONDS)

        return processed_count
