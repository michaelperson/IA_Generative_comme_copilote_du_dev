"""
Version corrigée du service, voir DIAGNOSIS.md pour l'explication complète
de l'incident et le raisonnement menant à cette correction.

Principe du correctif : on ne met plus en cache l'objet Order suivi par une
Session SQLAlchemy (il appartient à la Session de la requête qui l'a chargé,
et devient source de conflit dès qu'une autre Session tente de le réattacher).
Le cache ne conserve désormais qu'un OrderSnapshot immuable, et toute
opération d'écriture recharge systématiquement l'entité depuis la Session
courante avant de la modifier.
"""
from sqlalchemy.orm import sessionmaker

from models import Order, OrderSnapshot, OrderStatus

# Cache applicatif partagé entre toutes les "requêtes" (singleton), qui ne
# conserve désormais plus qu'un OrderSnapshot immuable — jamais l'entité
# suivie par SQLAlchemy.
_order_snapshot_cache: dict[str, OrderSnapshot] = {}


def update_order_status(session_factory: sessionmaker, order_id: str, new_status: OrderStatus) -> None:
    """
    Met à jour le statut d'une commande. L'entité est toujours rechargée
    depuis la Session courante : on n'essaie jamais de réattacher une entité
    mise en cache par une requête précédente (et donc potentiellement encore
    attachée à une autre Session), ce qui éliminait la cause de
    l'InvalidRequestError observée en production.
    """
    with session_factory() as session:
        order = session.get(Order, order_id)
        if order is None:
            raise LookupError(f"Commande '{order_id}' introuvable.")

        order.status = new_status
        session.add(order)
        session.commit()

        # Le cache ne conserve qu'un DTO immuable, jamais l'entité suivie par SQLAlchemy.
        _order_snapshot_cache[order_id] = OrderSnapshot.from_entity(order)


def get_order_snapshot(session_factory: sessionmaker, order_id: str) -> OrderSnapshot | None:
    """
    Récupère un instantané en lecture seule d'une commande, en passant par le
    cache applicatif. C'est le seul endroit où le cache est utilisé : il ne
    sert que pour de la lecture, jamais pour réattacher une entité à des fins
    d'écriture.
    """
    cached = _order_snapshot_cache.get(order_id)
    if cached is not None:
        return cached

    with session_factory() as session:
        order = session.get(Order, order_id)
        if order is None:
            return None
        snapshot = OrderSnapshot.from_entity(order)
        _order_snapshot_cache[order_id] = snapshot
        return snapshot
