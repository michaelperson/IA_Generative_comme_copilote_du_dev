# Incohérences détectées dans `RetryPolicy`

C'est le cœur de l'exercice : un bon prompt de documentation, poussé à signaler les
incohérences, doit faire remonter ce défaut plutôt que de le documenter comme un comportement
voulu.

## Le défaut

La classe fournie a un défaut réel : après épuisement des tentatives, elle lève une
`InvalidOperationException("Unreachable")` au lieu de propager la dernière exception rencontrée,
ce qui masque la cause originale de l'échec.

Concrètement, dans `ExecuteAsync` :

```csharp
catch (Exception) when (attempt < _maxAttempts)
{
    var delay = TimeSpan.FromMilliseconds(_baseDelay.TotalMilliseconds * Math.Pow(2, attempt - 1));
    await Task.Delay(delay);
}
```

l'exception capturée (`Exception`) n'est jamais conservée. Une fois la boucle terminée sans
retour réussi, le code exécute :

```csharp
throw new InvalidOperationException("Unreachable");
```

qui ne porte aucune information sur ce qui a réellement échoué (pas de type d'exception
d'origine, pas de message, pas de stack trace du service appelé).

## Pourquoi c'est un problème

- L'appelant qui catch cette exception ne peut pas distinguer un échec réseau transitoire d'un
  bug de programmation interne : les deux remontent comme une `InvalidOperationException`
  générique portant le message trompeur "Unreachable" (qui, ironiquement, prétend qu'un état
  jugé impossible a été atteint, alors qu'il s'agit du chemin normal d'échec définitif).
- Toute logique de supervision / alerting / retry en amont qui inspecte le type ou le message de
  l'exception perd l'information nécessaire pour agir correctement (par exemple distinguer un
  timeout d'une erreur d'authentification).
- Le diagnostic en production devient beaucoup plus difficile : la stack trace pointe vers
  `RetryPolicy`, pas vers l'appel réseau qui a réellement échoué en dernier.

## Correctif de référence

Voir `RetryPolicyFixed.cs` dans ce même dossier : la dernière exception capturée est conservée
(via `ExceptionDispatchInfo.Capture`) puis repropagée telle quelle (`ExceptionDispatchInfo.Throw`)
après épuisement des tentatives, au lieu d'être remplacée par une exception générique.

## Point de vigilance pédagogique

Un prompt de documentation qui se contente de décrire fidèlement le comportement existant
("lève une `InvalidOperationException` en cas d'échec de toutes les tentatives") documente le
bug comme s'il s'agissait d'un comportement voulu, sans jamais le signaler comme suspect. Un
prompt de documentation exigeant, qui demande explicitement de relever les incohérences,
doit faire remonter ce défaut — c'est exactement ce que ce prompt fourni dans le README impose
via sa dernière contrainte.
