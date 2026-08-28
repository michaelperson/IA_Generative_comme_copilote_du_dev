from notification import (
    EmailNotification,
    EmailNotificationCreator,
    NotificationChannel,
    PushNotification,
    PushNotificationCreator,
    SmsNotification,
    SmsNotificationCreator,
)


def test_email_notification_send_returns_confirmation():
    result = EmailNotification().send("hello")
    assert "Email" in result
    assert "hello" in result


def test_sms_notification_send_returns_confirmation():
    result = SmsNotification().send("hello")
    assert "SMS" in result


def test_push_notification_send_returns_confirmation():
    result = PushNotification().send("hello")
    assert "Push" in result


def test_email_creator_produces_email_notification():
    creator = EmailNotificationCreator()
    assert isinstance(creator.create_notification(), EmailNotification)


def test_sms_creator_produces_sms_notification():
    creator = SmsNotificationCreator()
    assert isinstance(creator.create_notification(), SmsNotification)


def test_push_creator_produces_push_notification():
    creator = PushNotificationCreator()
    assert isinstance(creator.create_notification(), PushNotification)


def test_creator_send_delegates_to_created_notification():
    creators = {
        NotificationChannel.EMAIL: EmailNotificationCreator(),
        NotificationChannel.SMS: SmsNotificationCreator(),
        NotificationChannel.PUSH: PushNotificationCreator(),
    }
    confirmation = creators[NotificationChannel.SMS].send("Colis expédié")
    assert "SMS" in confirmation
    assert "Colis expédié" in confirmation
