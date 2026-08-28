#!/usr/bin/env python3
"""compress_logs.py — premier jet "naïf" (équivalent Python de l'étape 1).

Correspond au script obtenu après le premier prompt, très simple, de
l'étape 1 de l'exercice :

    « Écris un script [bash] qui trouve tous les fichiers .log de plus de
      10 Mo dans le répertoire /var/log et les compresse. »

Ici transposé en Python avec la bibliothèque standard (pathlib, gzip,
shutil), à titre d'équivalent bonus — voir README.md, section
"Adaptation de langages pour cet exercice".

Il fait exactement ce qui est demandé et rien de plus, volontairement :
le but de l'exercice est d'identifier ses manques avant de renvoyer un
prompt de relance plus précis (voir python/solution/ pour la version
robuste correspondant au prompt de relance).

NOTE (adaptation dépôt) : en usage réel, ce script cible /var/log. Pour ne
jamais risquer de toucher un vrai /var/log en testant ce dépôt d'exercices,
le répertoire cible par défaut ici est ./test-logs (répertoire de test local,
sans danger). Passez un autre chemin en argument si besoin.
"""

import gzip
import shutil
import sys
from pathlib import Path

SIZE_THRESHOLD_BYTES = 10 * 1024 * 1024  # 10 Mo


def main() -> None:
    target_dir = Path(sys.argv[1]) if len(sys.argv) > 1 else Path("./test-logs")

    for log_file in target_dir.rglob("*.log"):
        if log_file.stat().st_size > SIZE_THRESHOLD_BYTES:
            gz_path = log_file.with_suffix(log_file.suffix + ".gz")
            with open(log_file, "rb") as f_in, gzip.open(gz_path, "wb") as f_out:
                shutil.copyfileobj(f_in, f_out)
            log_file.unlink()
            print(f"Compressé : {log_file} -> {gz_path}")

    print("Terminé.")


if __name__ == "__main__":
    main()


# -----------------------------------------------------------------------
# Failles de robustesse et de sécurité à identifier (voir README, étape 1) :
#
# 1. Fichier activement écrit : le script lit puis supprime le fichier
#    source pendant qu'un processus peut encore être en train d'y écrire —
#    risque de .gz tronqué et de perte de données pour l'écrivain concurrent.
#
# 2. Boucle sur les .gz déjà produits : rglob("*.log") ne matche pas
#    directement les fichiers ".log.gz", mais rien n'empêche un fichier .log
#    restant d'être retraité à chaque exécution, et rien ne protège contre
#    deux exécutions concurrentes du script sur les mêmes fichiers.
#
# 3. Permissions non vérifiées : si target_dir n'est pas accessible en
#    écriture, l'appel à gzip.open()/unlink() lèvera une PermissionError
#    non gérée, avec une trace Python brute au lieu d'un message clair.
#
# 4. Aucune trace de ce qui a été fait : pas de journalisation horodatée
#    dans un fichier de log dédié.
#
# 5. Aucun mode d'essai ("dry-run") : le script modifie immédiatement le
#    système de fichiers dès son exécution.
# -----------------------------------------------------------------------
