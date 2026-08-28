#!/usr/bin/env python3
"""compress_logs.py — version robuste (prompt de relance avec contraintes).

Équivalent Python (bibliothèque standard uniquement) du script bash de
bash/solution/compress-logs.sh — voir README.md, section "Adaptation de
langages pour cet exercice".

Implémente toutes les contraintes du prompt de relance de l'exercice 9 :
  - exclut les fichiers déjà compressés (.gz)
  - utilise un verrou (fcntl.flock) pour éviter une exécution concurrente
  - journalise chaque fichier traité avec horodatage dans un fichier de
    log dédié du script lui-même (module `logging`)
  - s'exécute en mode "dry-run" par défaut (affiche ce qu'il ferait sans
    rien modifier), et ne compresse réellement qu'avec l'option --apply
  - gère explicitement le cas où le répertoire cible n'est pas accessible
    en écriture (os.access)

RÈGLE NON NÉGOCIABLE : même généré avec des contraintes précises, ce script
doit être testé en mode dry-run puis sur un répertoire de test isolé avant
toute exécution sur un système réel (voir README.md et TEST.md — les mêmes
scénarios de test s'appliquent à cette version Python).

Usage :
    python3 compress_logs.py [TARGET_DIR] [--apply]

    TARGET_DIR : répertoire à scanner (défaut : ./test-logs, un répertoire
                 de test local sans danger — en usage réel sur un serveur,
                 la cible visée par le prompt d'origine est /var/log).
    --apply    : compresse réellement les fichiers trouvés. Sans cette
                 option, le script tourne en mode dry-run (aucune écriture).
"""

from __future__ import annotations

import argparse
import fcntl
import gzip
import logging
import os
import shutil
import sys
from pathlib import Path

SIZE_THRESHOLD_BYTES = 10 * 1024 * 1024  # 10 Mo

SCRIPT_DIR = Path(__file__).resolve().parent
LOG_FILE = SCRIPT_DIR / "compress_logs.log"
LOCK_FILE = SCRIPT_DIR / "compress_logs.lock"


def setup_logging() -> logging.Logger:
    """Configure la journalisation horodatée vers le fichier dédié du script
    ET vers la sortie standard, pour un retour immédiat à l'utilisateur."""
    logger = logging.getLogger("compress_logs")
    logger.setLevel(logging.INFO)

    formatter = logging.Formatter(
        fmt="[%(asctime)s] %(message)s", datefmt="%Y-%m-%d %H:%M:%S"
    )

    file_handler = logging.FileHandler(LOG_FILE, encoding="utf-8")
    file_handler.setFormatter(formatter)
    logger.addHandler(file_handler)

    stream_handler = logging.StreamHandler(sys.stdout)
    stream_handler.setFormatter(formatter)
    logger.addHandler(stream_handler)

    return logger


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser(
        description=(
            "Trouve les fichiers .log de plus de 10 Mo dans un répertoire "
            "et les compresse (gzip). Mode dry-run par défaut."
        )
    )
    parser.add_argument(
        "target_dir",
        nargs="?",
        default="./test-logs",
        help=(
            "Répertoire à scanner (défaut : ./test-logs pour les tests "
            "locaux sans danger ; en usage réel : /var/log)."
        ),
    )
    parser.add_argument(
        "--apply",
        action="store_true",
        help="Compresse réellement les fichiers trouvés (sinon : dry-run).",
    )
    return parser.parse_args()


def acquire_lock() -> object:
    """Ouvre et verrouille (verrou exclusif, non bloquant) le fichier de
    verrou pour empêcher une exécution concurrente du script. Retourne le
    descripteur de fichier ouvert, à garder en vie pendant toute la durée
    du traitement (le verrou est libéré à sa fermeture)."""
    lock_fd = open(LOCK_FILE, "w")
    try:
        fcntl.flock(lock_fd, fcntl.LOCK_EX | fcntl.LOCK_NB)
    except OSError:
        lock_fd.close()
        raise RuntimeError(
            f"une autre instance du script semble déjà en cours d'exécution "
            f"(verrou {LOCK_FILE} déjà pris). Abandon."
        )
    return lock_fd


def find_candidate_files(target_dir: Path):
    """Fichiers .log de plus de 10 Mo, en excluant explicitement les
    fichiers déjà compressés (.gz)."""
    for path in sorted(target_dir.rglob("*.log")):
        if path.name.endswith(".gz"):
            continue  # déjà compressé — contrainte du prompt de relance
        if path.is_file() and path.stat().st_size > SIZE_THRESHOLD_BYTES:
            yield path


def main() -> int:
    args = parse_args()
    target_dir = Path(args.target_dir)
    logger = setup_logging()

    # --- Vérification préalable : répertoire cible accessible ---------------
    if not target_dir.is_dir():
        logger.error("ERREUR : le répertoire cible '%s' n'existe pas.", target_dir)
        return 1

    if args.apply and not os.access(target_dir, os.W_OK):
        logger.error(
            "ERREUR : le répertoire cible '%s' n'est pas accessible en "
            "écriture par l'utilisateur courant. Abandon avant toute "
            "modification.",
            target_dir,
        )
        return 1

    # --- Verrou : empêcher une exécution concurrente -------------------------
    try:
        lock_fd = acquire_lock()
    except RuntimeError as exc:
        logger.error("ERREUR : %s", exc)
        return 1

    try:
        mode = "APPLY (compression réelle)" if args.apply else "DRY-RUN (aucune modification)"
        logger.info("Démarrage — mode %s — cible : %s", mode, target_dir)

        file_count = 0
        for log_file in find_candidate_files(target_dir):
            file_count += 1
            if args.apply:
                gz_path = log_file.with_suffix(log_file.suffix + ".gz")
                try:
                    with open(log_file, "rb") as f_in, gzip.open(gz_path, "wb") as f_out:
                        shutil.copyfileobj(f_in, f_out)
                    log_file.unlink()
                    logger.info("COMPRESSÉ : %s -> %s", log_file, gz_path)
                except OSError as exc:
                    logger.error("ERREUR lors de la compression de %s : %s", log_file, exc)
            else:
                logger.info("[DRY-RUN] serait compressé : %s", log_file)

        if file_count == 0:
            logger.info("Aucun fichier .log de plus de 10 Mo trouvé dans %s.", target_dir)
        else:
            logger.info("Terminé — %d fichier(s) traité(s).", file_count)

        if not args.apply:
            logger.info(
                "Rappel : mode dry-run, aucune modification effectuée. "
                "Relancez avec --apply pour compresser réellement."
            )
    finally:
        lock_fd.close()

    return 0


if __name__ == "__main__":
    sys.exit(main())
