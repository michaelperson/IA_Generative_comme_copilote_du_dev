"""Validation de numéros de TVA intracommunautaires belges.

Règle métier :
    - Format attendu après nettoyage : "BE" suivi de 10 chiffres.
    - Les 8 premiers chiffres forment le numéro d'entreprise.
    - Les 2 derniers chiffres forment la clé de contrôle, qui doit être égale
      à 97 - (numéro d'entreprise modulo 97).
    - Cas limite : si (numéro d'entreprise modulo 97) == 0, la clé attendue
      vaudrait 97, ce qui n'est pas un cas valide pour ce schéma sur 2
      chiffres : le numéro est alors considéré comme invalide, quelle que
      soit la clé fournie.
"""

import re
from dataclasses import dataclass
from typing import Optional

_FORMAT_BELGE = re.compile(r"^BE\d{10}$")
_MODULO = 97


@dataclass(frozen=True)
class ResultatValidationTva:
    """Résultat structuré de la validation d'un numéro de TVA.

    Volontairement plus riche qu'un simple booléen afin de rester
    exploitable directement par un appelant (formulaire web, API, etc.).

    Attributes:
        est_valide: True si le numéro de TVA est valide.
        message_erreur: Message d'erreur explicite si invalide, None si valide.
    """

    est_valide: bool
    message_erreur: Optional[str]


def _nettoyer(entree: str) -> str:
    """Supprime espaces, points et tirets, puis met en majuscule."""
    return entree.replace(" ", "").replace(".", "").replace("-", "").upper()


def valider_numero_tva_belge(entree: Optional[str]) -> ResultatValidationTva:
    """Valide un numéro de TVA intracommunautaire belge.

    Args:
        entree: le numéro de TVA saisi par l'utilisateur. Peut contenir des
            espaces, points ou tirets (ex : "BE 0123.456.749") et être None
            ou vide.

    Returns:
        Un ResultatValidationTva indiquant si le numéro est valide et, si ce
        n'est pas le cas, un message d'erreur explicite.
    """
    if entree is None or entree.strip() == "":
        return ResultatValidationTva(False, "Le numéro de TVA ne peut pas être vide.")

    nettoye = _nettoyer(entree)

    if not _FORMAT_BELGE.match(nettoye):
        return ResultatValidationTva(
            False,
            "Format invalide : un numéro de TVA belge doit être \"BE\" suivi de 10 chiffres.",
        )

    chiffres = nettoye[2:]
    numero_entreprise = int(chiffres[:8])
    cle_controle = int(chiffres[8:10])

    reste = numero_entreprise % _MODULO

    if reste == 0:
        # Cas limite du modulo 97 : la clé attendue vaudrait 97 (97 - 0), ce
        # qui ne correspond à aucune clé de contrôle valide sur 2 chiffres
        # pour ce schéma. On traite donc explicitement ce cas comme invalide
        # plutôt que de laisser la clé attendue "déborder" à 97.
        return ResultatValidationTva(
            False,
            "Numéro invalide : le numéro d'entreprise est un multiple de 97 (cas limite du modulo 97).",
        )

    cle_attendue = _MODULO - reste

    if cle_controle != cle_attendue:
        return ResultatValidationTva(
            False,
            f"Clé de contrôle invalide : attendu {cle_attendue:02d}, reçu {cle_controle:02d}.",
        )

    return ResultatValidationTva(True, None)
