"""
Exercice 3 — Factory Method appliqué à une fabrique de notifications.

Ce module (starter) contient :
- le protocole Notification (interface commune)
- les trois implémentations concrètes (EmailNotification, SmsNotification,
  PushNotification), à compléter
- l'enum NotificationChannel
- le créateur abstrait NotificationCreator (déjà fourni)

TODO : créer les créateurs concrets (un par canal) qui héritent de
NotificationCreator et surchargent create_notification(), PUIS compléter
la démo en bas de fichier.

Attention : le but est une véritable application du Factory Method
(une méthode de fabrication abstraite surchargée par sous-classe),
PAS un simple if/elif ou match déguisé en "factory".
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
        # TODO: simuler l'envoi d'un email et retourner une chaine de confirmation,
        # par exemple : f"[Email] envoyé : {message}"
        raise NotImplementedError("TODO: implémenter EmailNotification.send")


class SmsNotification(Notification):
    """Notification envoyée par SMS."""

    def send(self, message: str) -> str:
        # TODO: simuler l'envoi d'un SMS et retourner une chaine de confirmation,
        # par exemple : f"[SMS] envoyé : {message}"
        raise NotImplementedError("TODO: implémenter SmsNotification.send")


class PushNotification(Notification):
    """Notification envoyée par push (application mobile/desktop)."""

    def send(self, message: str) -> str:
        # TODO: simuler l'envoi d'une notification push et retourner une chaine
        # de confirmation, par exemple : f"[Push] envoyé : {message}"
        raise NotImplementedError("TODO: implémenter PushNotification.send")


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


# TODO: créer les créateurs concrets qui héritent de NotificationCreator,
# un par canal, en surchargeant create_notification() :
#   - EmailNotificationCreator : retourne un EmailNotification
#   - SmsNotificationCreator   : retourne un SmsNotification
#   - PushNotificationCreator  : retourne un PushNotification


def main() -> None:
    # TODO: une fois les créateurs concrets écrits, construire une table de
    # correspondance NotificationChannel -> NotificationCreator et l'utiliser
    # pour envoyer un message sur le canal préféré de l'utilisateur, par
    # exemple :
    #
    # creators = {
    #     NotificationChannel.EMAIL: EmailNotificationCreator(),
    #     NotificationChannel.SMS: SmsNotificationCreator(),
    #     NotificationChannel.PUSH: PushNotificationCreator(),
    # }
    # preferred_channel = NotificationChannel.PUSH
    # confirmation = creators[preferred_channel].send("Votre commande a été expédiée.")
    # print(confirmation)
    print("TODO: démo non implémentée.")


if __name__ == "__main__":
    main()
