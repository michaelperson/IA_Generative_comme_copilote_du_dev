# Revue de code — OrderService.cs

Revue structurée triée par sévérité décroissante (bloquant > suggestion > mineur), produite en suivant le prompt de revue de PR assistée fourni dans le support (README.md du dossier).

| # | Ligne(s) | Guideline / principe violé | Sévérité | Proposition de correction |
|---|----------|------------------------------|----------|----------------------------|
| 1 | 35-37 | « Les exceptions attrapées doivent être loguées avec le contexte (jamais de catch vide) » : le `catch (Exception)` ne fait rien — l'échec de l'appel réseau est totalement silencieux. | **Bloquant** | Logger l'exception avec son contexte (numéro de tentative, message d'erreur) via un `ILogger` injecté, par exemple `_logger.LogError(ex, "Échec tentative {Attempt}/{Max}", attempt, MaxRetryAttempts);`. |
| 2 | 21 | « Toute méthode publique async doit accepter un CancellationToken » : `FetchAndProcessOrdersAsync()` ne peut pas être annulée par l'appelant, ni propager l'annulation aux appels réseau qu'elle effectue. | **Bloquant** | Ajouter un paramètre `CancellationToken cancellationToken`, le transmettre à `GetAsync`, `ReadAsStringAsync` et `Task.Delay`, et vérifier `cancellationToken.ThrowIfCancellationRequested()` en tête de boucle. |
| 3 | 25, 39 | « Pas de "magic numbers" : utiliser des constantes nommées » : `3` (nombre de tentatives) et `30000` (délai en ms) sont des littéraux sans nom, dont la signification n'est pas évidente à la lecture. | Suggestion | Extraire deux constantes nommées, ex. `private const int MaxRetryAttempts = 3;` et `private const int RetryDelayMilliseconds = 30_000;`. |
| 4 | 7 | « Toute nouvelle dépendance externe doit être justifiée en commentaire » : `Newtonsoft.Json` est importé sans commentaire, alors que le projet cible un .NET moderne qui inclut nativement `System.Text.Json` pour ce même besoin. | Suggestion | Soit ajouter un commentaire justifiant pourquoi Newtonsoft.Json est nécessaire (fonctionnalité manquante côté `System.Text.Json`), soit remplacer par `System.Text.Json.JsonSerializer` qui couvre ce cas simple sans dépendance supplémentaire. |
| 5 | 23 | « Les noms de variables sont en anglais » : `resultat` est un nom de variable en français. | Mineur | Renommer en `processedCount` (ou équivalent anglais explicite). |

## Synthèse

Sur 5 remarques, 2 sont bloquantes (gestion des erreurs silencieuse, absence de support d'annulation) et concernent des risques réels en production (masquage de pannes, impossibilité d'arrêter proprement l'opération lors d'un arrêt applicatif). Les 3 autres sont des non-conformités aux guidelines mais sans risque fonctionnel immédiat. Voir `CorrectedExample.cs` pour la version corrigeant l'ensemble de ces points.
