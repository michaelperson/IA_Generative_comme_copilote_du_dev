"""Validation de numéros de TVA belges."""

import re
from dataclasses import dataclass
from typing import Optional

FORMAT = re.compile(r"^BE\d{10}$")


@dataclass(frozen=True)
class ResultatValidationTva:
    est_valide: bool
    message_erreur: Optional[str]


def valider_numero_tva_belge(entree: Optional[str]) -> ResultatValidationTva:
    if not entree or not entree.strip():
        return ResultatValidationTva(False, "Le numéro de TVA ne peut pas être vide.")

    nettoye = entree.replace(" ", "").replace(".", "").replace("-", "").upper()

    if not FORMAT.match(nettoye):
        return ResultatValidationTva(
            False,
            'Format invalide : un numéro de TVA belge doit être "BE" suivi de 10 chiffres.',
        )

    numero_entreprise = int(nettoye[2:10])
    cle_controle = int(nettoye[10:12])
    reste = numero_entreprise % 97

    if reste == 0:
        return ResultatValidationTva(
            False,
            "Numéro invalide : le numéro d'entreprise est un multiple de 97 (cas limite du modulo 97).",
        )

    cle_attendue = 97 - reste
    if cle_controle != cle_attendue:
        return ResultatValidationTva(
            False, f"Clé de contrôle invalide : attendu {cle_attendue:02d}, reçu {cle_controle:02d}."
        )

    return ResultatValidationTva(True, None)
