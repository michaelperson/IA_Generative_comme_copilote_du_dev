#!/usr/bin/env bash
#
# compress-logs.sh — version robuste (prompt de relance avec contraintes)
#
# Implémente toutes les contraintes du prompt de relance de l'exercice 9 :
#   - exclut les fichiers déjà compressés (.gz)
#   - utilise un verrou (flock) pour éviter une exécution concurrente
#   - journalise chaque fichier traité avec horodatage dans un fichier de
#     log dédié du script lui-même
#   - s'exécute en mode "dry-run" par défaut (affiche ce qu'il ferait sans
#     rien modifier), et ne compresse réellement qu'avec l'option --apply
#   - gère explicitement le cas où le répertoire cible n'est pas accessible
#     en écriture
#
# RÈGLE NON NÉGOCIABLE : même généré avec des contraintes précises, ce script
# doit être testé en mode dry-run puis sur un répertoire de test isolé avant
# toute exécution sur un système réel (voir README.md et TEST.md).
#
# Usage :
#   ./compress-logs.sh [TARGET_DIR] [--apply]
#
#   TARGET_DIR : répertoire à scanner (défaut : ./test-logs, un répertoire de
#                test local sans danger — en usage réel sur un serveur, la
#                cible visée par le prompt d'origine est /var/log).
#   --apply    : compresse réellement les fichiers trouvés. Sans cette
#                option, le script tourne en mode dry-run (aucune écriture).

set -euo pipefail

# --- Analyse des arguments -------------------------------------------------

TARGET_DIR="./test-logs"
APPLY=0

for arg in "$@"; do
    case "$arg" in
        --apply)
            APPLY=1
            ;;
        *)
            TARGET_DIR="$arg"
            ;;
    esac
done

readonly TARGET_DIR APPLY
readonly SIZE_THRESHOLD="+10M"

# Fichier de log dédié du script (à côté du script lui-même), et fichier de
# verrou pour empêcher deux exécutions concurrentes.
readonly SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
readonly LOG_FILE="${SCRIPT_DIR}/compress-logs.log"
readonly LOCK_FILE="${SCRIPT_DIR}/compress-logs.lock"

# --- Fonctions utilitaires ---------------------------------------------------

log() {
    # Journalise un message avec horodatage dans le fichier de log dédié
    # ET sur la sortie standard, pour un retour immédiat à l'utilisateur.
    local timestamp
    timestamp="$(date '+%Y-%m-%d %H:%M:%S')"
    echo "[${timestamp}] $*" | tee -a "$LOG_FILE"
}

# --- Vérification préalable : répertoire cible accessible en écriture ------
#
# Contrainte : gérer explicitement le cas où le répertoire cible n'est pas
# accessible en écriture, plutôt que de laisser gzip échouer fichier par
# fichier avec des erreurs peu claires.

if [[ ! -d "$TARGET_DIR" ]]; then
    log "ERREUR : le répertoire cible '${TARGET_DIR}' n'existe pas."
    exit 1
fi

if (( APPLY == 1 )) && [[ ! -w "$TARGET_DIR" ]]; then
    log "ERREUR : le répertoire cible '${TARGET_DIR}' n'est pas accessible en écriture par l'utilisateur courant ($(whoami)). Abandon avant toute modification."
    exit 1
fi

# --- Verrou : empêcher une exécution concurrente ----------------------------
#
# Contrainte : utiliser un verrou (flock) pour éviter que deux instances du
# script ne traitent les mêmes fichiers en même temps.

exec 200>"$LOCK_FILE"
if ! flock -n 200; then
    log "ERREUR : une autre instance de ${0} semble déjà en cours d'exécution (verrou ${LOCK_FILE} déjà pris). Abandon."
    exit 1
fi

# --- Traitement --------------------------------------------------------------

if (( APPLY == 1 )); then
    log "Démarrage — mode APPLY (compression réelle) — cible : ${TARGET_DIR}"
else
    log "Démarrage — mode DRY-RUN (aucune modification) — cible : ${TARGET_DIR}"
fi

file_count=0

# Contrainte : exclure les fichiers déjà compressés (.gz). On cherche donc
# les *.log strictement (l'extension .log.gz ne matche pas le motif "*.log"
# de find, mais on le documente explicitement pour que ce soit sans ambiguïté
# — voir aussi le test associé dans TEST.md).
while IFS= read -r -d '' file; do
    file_count=$((file_count + 1))

    if (( APPLY == 1 )); then
        if gzip "$file"; then
            log "COMPRESSÉ : ${file} -> ${file}.gz"
        else
            log "ERREUR lors de la compression de : ${file}"
        fi
    else
        log "[DRY-RUN] serait compressé : ${file}"
    fi
done < <(find "$TARGET_DIR" -type f -name "*.log" ! -name "*.log.gz" -size "$SIZE_THRESHOLD" -print0)

if (( file_count == 0 )); then
    log "Aucun fichier .log de plus de 10 Mo trouvé dans ${TARGET_DIR}."
else
    log "Terminé — ${file_count} fichier(s) traité(s)."
fi

if (( APPLY == 0 )); then
    log "Rappel : mode dry-run, aucune modification effectuée. Relancez avec --apply pour compresser réellement."
fi
