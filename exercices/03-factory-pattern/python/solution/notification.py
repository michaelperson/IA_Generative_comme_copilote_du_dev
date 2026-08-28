"""
Exercice 3 — Factory Method appliqué à une fabrique de notifications.

Implémentation de référence : une véritable application du Factory Method
(une méthode de fabrication abstraite `create_notification`, surchargée par
chaque créateur concret), pas un simple if/elif ou match déguisé en "factory".
"""

from __future__ import annotations

from abc import ABC, abstractmethod
from enum import Enum, auto


class Notification(ABC):
    """Contrat commun à tous les canaux de notification."""

    @abstractmethod
    def send(self, message: str) -> str:
        """Envoie le message via le canal concret et retourne une confirmation
        décrivant l'envoi (simulation, aucun appel réseau réel n'est effectué)."""
        raise NotImplementedError


class EmailNotification(Notification):
    """Notification envoyée par email."""

    def send(self, message: str) -> str:
        return f"[Email] envoyé : {message}"


class SmsNotification(Notification):
    """Notification envoyée par SMS."""

    def send(self, message: str) -> str:
        return f"[SMS] envoyé : {message}"


class PushNotification(Notification):
    """Notification envoyée par push (application mobile/desktop)."""

    def send(self, message: str) -> str:
        return f"[Push] envoyé : {message}"


class NotificationChannel(Enum):
    """Canal de notification choisi par la préférence utilisateur."""

    EMAIL = auto()
    SMS = auto()
    PUSH = auto()


class NotificationCreator(ABC):
    """
    Créateur abstrait du pattern Factory Method.

    Chaque sous-classe concrète sait fabriquer UN type de notification via la
    méthode de fabrication `create_notification`.
    """

    @abstractmethod
    def create_notification(self) -> Notification:
        """Méthode de fabrication (factory method) : chaque sous-classe
        concrète retourne l'implémentation de Notification qui lui correspond."""
        raise NotImplementedError

    def send(self, message: str) -> str:
        """Logique commune qui s'appuie sur le produit fabriqué par la
        sous-classe, sans jamais connaître le type concret de notification
        instancié."""
        notification = self.create_notification()
        return notification.send(message)


class EmailNotificationCreator(NotificationCreator):
    """Créateur concret produisant des EmailNotification."""

    def create_notification(self) -> Notification:
        return EmailNotification()


class SmsNotificationCreator(NotificationCreator):
    """Créateur concret produisant des SmsNotification."""

    def create_notification(self) -> Notification:
        return SmsNotification()


class PushNotificationCreator(NotificationCreator):
    """Créateur concret produisant des PushNotification."""

    def create_notification(self) -> Notification:
        return PushNotification()


def main() -> None:
    """Démo : sélectionne dynamiquement le créateur selon la préférence
    utilisateur, sans jamais instancier directement une classe de
    notification concrète en dehors des créateurs."""
    creators: dict[NotificationChannel, NotificationCreator] = {
        NotificationChannel.EMAIL: EmailNotificationCreator(),
        NotificationChannel.SMS: SmsNotificationCreator(),
        NotificationChannel.PUSH: PushNotificationCreator(),
    }
    preferred_channel = NotificationChannel.PUSH
    confirmation = creators[preferred_channel].send("Votre commande a été expédiée.")
    print(confirmation)


if __name__ == "__main__":
    main()
