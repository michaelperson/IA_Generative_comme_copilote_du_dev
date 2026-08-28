"""Modèle SQLAlchemy partagé par order_service.py (starter et solution)."""
import enum
import uuid
from dataclasses import dataclass

from sqlalchemy import Enum, String
from sqlalchemy.orm import DeclarativeBase, Mapped, mapped_column


class OrderStatus(enum.Enum):
    PENDING = "PENDING"
    CONFIRMED = "CONFIRMED"
    SHIPPED = "SHIPPED"
    DELIVERED = "DELIVERED"
    CANCELLED = "CANCELLED"


class Base(DeclarativeBase):
    pass


class Order(Base):
    __tablename__ = "orders"

    id: Mapped[str] = mapped_column(String(36), primary_key=True, default=lambda: str(uuid.uuid4()))
    customer_name: Mapped[str] = mapped_column(String(200))
    status: Mapped[OrderStatus] = mapped_column(Enum(OrderStatus))


@dataclass(frozen=True)
class OrderSnapshot:
    """
    Instantané immuable en lecture seule d'une commande. C'est ce type — et
    non l'objet Order suivi par une Session SQLAlchemy — qui doit être placé
    dans un cache applicatif partagé entre requêtes : un dataclass simple
    n'est attaché à aucune Session et ne peut donc jamais entrer en conflit
    d'attachement (InvalidRequestError: "already attached to session").
    """

    id: str
    status: OrderStatus

    @staticmethod
    def from_entity(order: "Order") -> "OrderSnapshot":
        return OrderSnapshot(id=order.id, status=order.status)
