"""Code proposé dans la Pull Request : récupération et traitement des commandes en attente."""
import asyncio
import httpx


class OrderService:
    def __init__(self, client: httpx.AsyncClient):
        self.client = client

    # Récupère les commandes en attente depuis l'API et les traite
    async def fetch_and_process_orders(self):
        resultat = 0

        for i in range(3):
            try:
                response = await self.client.get("https://api.example.com/orders/pending")
                orders = response.json()
                resultat += len(orders)
                return resultat
            except Exception:
                pass

            await asyncio.sleep(30)

        return resultat
