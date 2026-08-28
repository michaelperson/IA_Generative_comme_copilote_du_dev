"""
order_service.py
Extrait de code source de l'incident (à fournir en contexte) — adaptation
Python/SQLAlchemy du même incident que la version C#/EF Core de cet exercice
(voir README.md à la racine de l'exercice).

Contexte production : cette exception survient de façon intermittente,
uniquement sous charge (plusieurs requêtes concurrentes sur la même commande).
Chaque requête HTTP ouvre sa propre Session SQLAlchemy (scope "par requête",
pattern classique Flask/FastAPI) ; le cache ci-dessous est un dictionnaire
au niveau module, partagé par toute l'application (l'équivalent Python d'un
singleton).

BUG connu (à des fins pédagogiques) : voir ERROR.txt et DIAGNOSTIC.md dans ce
même dossier. Ce module est volontairement fautif — ne pas le corriger avant
d'avoir terminé l'étape 3 de l'exercice.
"""
from sqlalchemy.orm import Session, sessionmaker

from models import Order, OrderStatus

# Cache applicatif partagé entre toutes les "requêtes" (singleton), qui
# conserve directement l'instance ORM Order — c'est là que se situe le bug.
_order_cache: dict[str, Order] = {}


def update_order_status(session_factory: sessionmaker, order_id: str, new_status: OrderStatus) -> None:
    """Met à jour le statut d'une commande. Passe par un cache mémoire pour
    éviter une lecture base de données à chaque appel."""
    with session_factory() as session:  # session "scoped" à cette requête
        order = _order_cache.get(order_id)
        if order is None:
            order = session.get(Order, order_id)
            _order_cache[order_id] = order

        order.status = new_status
        session.add(order)  # ligne du plantage (voir ERROR.txt)
        session.commit()
