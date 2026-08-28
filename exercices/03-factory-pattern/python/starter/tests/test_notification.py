import pytest

from notification import EmailNotification, PushNotification, SmsNotification


def test_email_notification_send_returns_confirmation():
    notification = EmailNotification()

    # TODO (starter) : ce test échoue tant que EmailNotification.send
    # n'est pas implémenté (il lève NotImplementedError).
    result = notification.send("hello")

    assert "Email" in result


def test_sms_notification_send_returns_confirmation():
    notification = SmsNotification()

    # TODO (starter) : ce test échoue tant que SmsNotification.send
    # n'est pas implémenté (il lève NotImplementedError).
    result = notification.send("hello")

    assert "SMS" in result


def test_push_notification_send_returns_confirmation():
    notification = PushNotification()

    # TODO (starter) : ce test échoue tant que PushNotification.send
    # n'est pas implémenté (il lève NotImplementedError).
    result = notification.send("hello")

    assert "Push" in result


# TODO: une fois les créateurs concrets écrits (EmailNotificationCreator,
# SmsNotificationCreator, PushNotificationCreator), ajouter des tests qui
# vérifient que chaque créateur produit bien le type de notification attendu
# (véritable Factory Method, pas un switch/if déguisé).
