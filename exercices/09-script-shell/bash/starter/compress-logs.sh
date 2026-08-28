#!/usr/bin/env bash
#
# compress-logs.sh — premier jet "naïf"
#
# Ceci correspond au script obtenu après le premier prompt, très simple, de
# l'étape 1 de l'exercice :
#
#   « Écris un script bash qui trouve tous les fichiers .log de plus de 10 Mo
#     dans le répertoire /var/log et les compresse. »
#
# Il fait exactement ce qui est demandé et rien de plus. C'est volontaire :
# le but de l'exercice est d'identifier, en le relisant, tout ce qu'il ne
# règle PAS spontanément avant de renvoyer un prompt de relance plus précis
# (voir README.md, étape 2, et le script robuste dans bash/solution/).
#
# NOTE (adaptation dépôt) : en usage réel, ce script cible /var/log. Pour ne
# jamais risquer de toucher un vrai /var/log en testant ce dépôt d'exercices,
# le répertoire cible par défaut ici est ./test-logs (répertoire de test
# local, sans danger). Passez un autre chemin en premier argument si besoin.

TARGET_DIR="${1:-./test-logs}"

# On cherche les fichiers .log de plus de 10 Mo et on les compresse avec gzip.
find "$TARGET_DIR" -name "*.log" -size +10M -exec gzip {} \;

echo "Terminé."

# ---------------------------------------------------------------------------
# Failles de robustesse et de sécurité à identifier (voir README, étape 1) :
#
# 1. Fichier activement écrit : gzip peut compresser un fichier .log pendant
#    qu'un processus est encore en train d'écrire dedans. Le résultat est un
#    .gz potentiellement tronqué/corrompu, et le processus écrivain peut se
#    retrouver à écrire dans un fichier qui vient d'être supprimé/renommé par
#    gzip (gzip supprime le fichier source par défaut après compression).
#
# 2. Boucle sur les .gz déjà produits : si le script est relancé, les fichiers
#    déjà compressés (*.log.gz) ne sont pas exclus par le `find` — un fichier
#    .log restant supérieur à 10 Mo sera recompressé, et surtout rien n'exclut
#    qu'un ancien .log.gz de plus de 10 Mo pose problème à une exécution
#    concurrente ou répétée (aucune protection contre une deuxième instance
#    du script tournant en parallèle sur les mêmes fichiers).
#
# 3. Permissions non vérifiées : si /var/log (ou TARGET_DIR) n'est pas
#    accessible en écriture par l'utilisateur qui exécute le script, gzip
#    échouera fichier par fichier avec des erreurs peu claires en sortie
#    standard, sans message explicite ni arrêt propre du script.
#
# 4. Aucune trace de ce qui a été fait : pas de journalisation horodatée des
#    fichiers traités, impossible de vérifier après coup ce qui a été
#    compressé et quand.
#
# 5. Aucun mode d'essai ("dry-run") : le script modifie immédiatement le
#    système de fichiers dès son exécution, sans possibilité de prévisualiser
#    son effet avant de l'appliquer réellement.
# ---------------------------------------------------------------------------
