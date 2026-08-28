"""Modèle SQLAlchemy partagé par order_service.py (starter et solution)."""
import enum
import uuid

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
