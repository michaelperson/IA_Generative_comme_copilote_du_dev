# Revue de code — orderService.ts

Revue structurée triée par sévérité décroissante (bloquant > suggestion > mineur),
produite en suivant le prompt de revue de Pull Request assistée du support.

| # | Ligne(s) | Guideline / principe violé | Sévérité | Proposition de correction |
|---|----------|-----------------------------|----------|----------------------------|
| 1 | 18-20 | « Les exceptions attrapées doivent être loguées avec le contexte (jamais de catch vide) » : le `catch {}` ne fait rien — l'échec de l'appel réseau est totalement silencieux. | **Bloquant** | Journaliser l'erreur avec son contexte, par exemple `logger.error({ err, attempt: i + 1, maxAttempts: MAX_RETRY_ATTEMPTS }, "Échec de récupération des commandes")`. |
| 2 | 10 | « Toute méthode publique asynchrone doit accepter un moyen d'annulation » : `fetchAndProcessOrders` n'offre aucun moyen d'interrompre l'opération, en particulier pendant l'attente de 30 secondes entre deux tentatives. | **Bloquant** | Ajouter un paramètre `signal?: AbortSignal`, le passer à `fetch` et à `sleep`, et vérifier `signal?.aborted` en tête de boucle. |
| 3 | 13, 22 | « Pas de "magic numbers" » : `3` (nombre de tentatives) et `30000` (délai en millisecondes) sont des littéraux sans nom. | Suggestion | Extraire deux constantes nommées, par exemple `const MAX_RETRY_ATTEMPTS = 3` et `const RETRY_DELAY_MS = 30_000`. |
| 4 | 4 | « Toute nouvelle dépendance externe doit être justifiée en commentaire » : `import fetch from "node-fetch"` n'est justifié nulle part — et Node fournit désormais `fetch` nativement, ce qui rend la dépendance discutable. | Suggestion | Supprimer la dépendance au profit du `fetch` natif, ou justifier explicitement son maintien en commentaire. |
| 5 | 11 | « Les noms de variables sont en anglais » : `resultat` est en français. | Mineur | Renommer en `processedCount`. |

## Synthèse

Sur cinq remarques, deux sont bloquantes : la gestion silencieuse des erreurs
et l'absence de mécanisme d'annulation. Toutes deux ont un coût réel en
production — une panne masquée et une opération impossible à arrêter proprement,
qui immobilise un worker pendant une minute et demie au pire cas. Les trois
autres sont des non-conformités sans risque fonctionnel immédiat.

Voir `correctedExample.ts` pour la version corrigeant l'ensemble de ces points.
