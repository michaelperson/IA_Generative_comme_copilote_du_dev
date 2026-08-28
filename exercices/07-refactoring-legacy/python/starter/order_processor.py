"""
order_processor.py
Fonction legacy fournie pour l'exercice.
TODO (exercice) : appliquer la méthode "expliquer avant de modifier, puis refactorer
par étapes nommées" décrite dans le README de cet exercice. Ne rien changer ici tant
que les étapes 1 et 2 (explication + tests de caractérisation) n'ont pas été faites.
"""


def process_order(customer_type: str, amount: float, country: str, is_first_order: bool) -> str:
    discount = 0.0
    if customer_type == "VIP":
        discount = 0.20
    elif customer_type == "REGULAR":
        discount = 0.05

    if is_first_order:
        discount += 0.10

    if country == "BE":
        tax = 0.21
    elif country == "FR":
        tax = 0.20
    elif country == "DE":
        tax = 0.19
    else:
        tax = 0.15

    total = amount * (1 - discount) * (1 + tax)

    if total > 1000:
        result = f"Commande de {total} EUR - necessite validation manager"
        # TODO: envoyer un email au manager, jamais implémenté
    else:
        result = f"Commande de {total} EUR - validee automatiquement"

    print(f"LOG: order processed for {customer_type} total={total}")

    return result
