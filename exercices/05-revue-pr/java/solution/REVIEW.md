# Revue de code — OrderService.java

Revue structurée triée par sévérité décroissante (bloquant > suggestion > mineur), produite en suivant le prompt de revue de PR assistée fourni dans le support (README.md du dossier).

| # | Ligne(s) | Guideline / principe violé | Sévérité | Proposition de correction |
|---|----------|------------------------------|----------|----------------------------|
| 1 | 36-37 | « Les exceptions attrapées doivent être loguées avec le contexte (jamais de catch vide) » : le `catch (Exception e)` ne fait rien — l'échec de l'appel réseau est totalement silencieux. | **Bloquant** | Logger l'exception avec son contexte via un `Logger` (SLF4J), par exemple `LOGGER.error("Échec tentative {}/{}", attempt + 1, MAX_RETRY_ATTEMPTS, e);`. |
| 2 | 23 | « Toute méthode publique async doit accepter un CancellationToken » : `fetchAndProcessOrdersAsync()` n'offre aucun moyen à l'appelant d'interrompre l'opération de façon coopérative (l'équivalent Java d'un CancellationToken, par exemple un `BooleanSupplier isCancelled`, est absent). | **Bloquant** | Ajouter un paramètre permettant l'annulation coopérative (ex. `BooleanSupplier isCancelled`), vérifié en tête de boucle, et lever une `CancellationException` si l'annulation est demandée. |
| 3 | 27, 40 | « Pas de "magic numbers" : utiliser des constantes nommées » : `3` (nombre de tentatives) et `30000` (délai en ms) sont des littéraux sans nom. | Suggestion | Extraire deux constantes nommées, ex. `private static final int MAX_RETRY_ATTEMPTS = 3;` et `private static final long RETRY_DELAY_MILLIS = 30_000L;`. |
| 4 | 9-10 | « Toute nouvelle dépendance externe doit être justifiée en commentaire » : `Gson` est importé sans commentaire, alors qu'une bibliothèque JSON (Jackson) est déjà présente ailleurs dans le projet pour le même besoin. | Suggestion | Soit justifier en commentaire pourquoi Gson est nécessaire en plus de la dépendance JSON déjà en place, soit réutiliser `ObjectMapper` (Jackson) pour éviter une dépendance redondante. |
| 5 | 25 | « Les noms de variables sont en anglais » : `resultat` est un nom de variable en français. | Mineur | Renommer en `processedCount` (ou équivalent anglais explicite). |

## Synthèse

Sur 5 remarques, 2 sont bloquantes (gestion des erreurs silencieuse, absence de mécanisme d'annulation) et concernent des risques réels en production (masquage de pannes, impossibilité d'arrêter proprement l'opération). Les 3 autres sont des non-conformités aux guidelines mais sans risque fonctionnel immédiat. Voir `CorrectedExample.java` pour la version corrigeant l'ensemble de ces points.
