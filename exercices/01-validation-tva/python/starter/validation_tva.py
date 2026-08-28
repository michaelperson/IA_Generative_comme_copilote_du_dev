"""Validation de numéros de TVA intracommunautaires belges.

Exercice 01 - à compléter (voir README.md du dossier de l'exercice).
"""

from dataclasses import dataclass
from typing import Optional


@dataclass(frozen=True)
class ResultatValidationTva:
    """Résultat structuré de la validation d'un numéro de TVA.

    Attributes:
        est_valide: True si le numéro de TVA est valide.
        message_erreur: Message d'erreur si invalide, None si valide.
    """

    est_valide: bool
    message_erreur: Optional[str]


def valider_numero_tva_belge(entree: Optional[str]) -> ResultatValidationTva:
    """Valide un numéro de TVA intracommunautaire belge.

    Format attendu : "BE" + 10 chiffres, avec vérification de la clé de
    contrôle modulo 97 sur les 8 premiers chiffres du numéro d'entreprise.

    Args:
        entree: le numéro de TVA saisi (peut contenir espaces, points, tirets).

    Returns:
        Un ResultatValidationTva structuré indiquant la validité et un
        éventuel message d'erreur.
    """
    # TODO: 1. Nettoyer l'entrée (espaces, points, tirets, majuscules).
    # TODO: 2. Vérifier le format "BE" + 10 chiffres.
    # TODO: 3. Extraire numéro d'entreprise (8 premiers chiffres) et clé de contrôle (2 derniers).
    # TODO: 4. Calculer la clé attendue = 97 - (numéro d'entreprise modulo 97).
    #          Cas limite : si (numéro d'entreprise modulo 97) == 0, le numéro est INVALIDE.
    # TODO: 5. Gérer explicitement le cas d'une entrée None ou vide.
    # TODO: 6. Retourner un ResultatValidationTva (jamais un simple booléen).
    raise NotImplementedError("À implémenter : voir consigne de l'exercice 01-validation-tva.")
