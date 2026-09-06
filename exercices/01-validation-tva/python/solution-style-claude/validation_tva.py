"""Validation de numéros de TVA intracommunautaires belges.

Choix de conception, explicités volontairement :

1. Le résultat est un objet structuré plutôt qu'un booléen. Un appelant qui
   valide une saisie de formulaire a besoin de savoir POURQUOI le numéro est
   refusé, pas seulement qu'il l'est.
2. Le motif de rejet est typé (``MotifRejet``) en plus du message. Le message
   s'adresse à l'humain, le motif au code appelant, qui peut ainsi réagir
   selon la cause sans faire de correspondance sur du texte — fragile et non
   traduisible.
3. Aucune exception n'est levée pour une entrée invalide : une saisie
   utilisateur incorrecte est un cas nominal, pas une situation exceptionnelle.
"""

import re
from dataclasses import dataclass
from enum import Enum
from typing import Optional

_FORMAT_BELGE = re.compile(r"^BE\d{10}$")
_MODULO = 97
_LONGUEUR_NUMERO_ENTREPRISE = 8


class MotifRejet(str, Enum):
    """Motif de rejet, exploitable par le code appelant."""

    ENTREE_VIDE = "ENTREE_VIDE"
    FORMAT_INVALIDE = "FORMAT_INVALIDE"
    CAS_LIMITE_MODULO = "CAS_LIMITE_MODULO"
    CLE_CONTROLE_INVALIDE = "CLE_CONTROLE_INVALIDE"


@dataclass(frozen=True)
class ResultatValidationTva:
    """Résultat structuré de la validation.

    Attributes:
        est_valide: True si le numéro est valide.
        message_erreur: Message destiné à l'affichage, None si valide.
        motif: Motif de rejet typé, None si valide.
    """

    est_valide: bool
    message_erreur: Optional[str]
    motif: Optional[MotifRejet] = None


def _valide() -> ResultatValidationTva:
    return ResultatValidationTva(True, None, None)


def _rejet(motif: MotifRejet, message: str) -> ResultatValidationTva:
    return ResultatValidationTva(False, message, motif)


def _normaliser(entree: str) -> str:
    """Retire les séparateurs usuels et met en majuscules.

    Les utilisateurs saisissent indifféremment "BE 0123.456.749",
    "be-0123456749" ou "BE0123456749". Refuser ces variantes serait une rigueur
    inutile, qui se paierait en support.
    """
    return entree.replace(" ", "").replace(".", "").replace("-", "").upper()


def valider_numero_tva_belge(entree: Optional[str]) -> ResultatValidationTva:
    """Valide un numéro de TVA intracommunautaire belge.

    Args:
        entree: saisie utilisateur, éventuellement None, vide ou formatée avec
            des séparateurs.

    Returns:
        Un ResultatValidationTva : validité, message d'affichage et motif typé.
    """
    if entree is None or not entree.strip():
        return _rejet(MotifRejet.ENTREE_VIDE, "Le numéro de TVA ne peut pas être vide.")

    nettoye = _normaliser(entree)

    if not _FORMAT_BELGE.match(nettoye):
        return _rejet(
            MotifRejet.FORMAT_INVALIDE,
            'Format invalide : un numéro de TVA belge doit être "BE" suivi de 10 chiffres.',
        )

    chiffres = nettoye[2:]
    numero_entreprise = int(chiffres[:_LONGUEUR_NUMERO_ENTREPRISE])
    cle_controle = int(chiffres[_LONGUEUR_NUMERO_ENTREPRISE:])

    reste = numero_entreprise % _MODULO

    # Cas limite mathématique : si le reste est nul, la clé attendue vaudrait
    # 97, valeur impossible à représenter sur les deux chiffres prévus par le
    # schéma. Ce n'est pas une erreur de saisie mais une impossibilité
    # structurelle : on la distingue donc d'une clé simplement fausse.
    if reste == 0:
        return _rejet(
            MotifRejet.CAS_LIMITE_MODULO,
            "Numéro invalide : le numéro d'entreprise est un multiple de 97 (cas limite du modulo 97).",
        )

    cle_attendue = _MODULO - reste

    if cle_controle != cle_attendue:
        return _rejet(
            MotifRejet.CLE_CONTROLE_INVALIDE,
            f"Clé de contrôle invalide : attendu {cle_attendue:02d}, reçu {cle_controle:02d}.",
        )

    return _valide()
