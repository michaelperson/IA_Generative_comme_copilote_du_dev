# Diagnostic — incident OrderService (C# / EF Core)

Corrigé de référence pour les 3 étapes de l'exercice. À ne consulter qu'après
avoir rédigé ses propres réponses dans `starter/DIAGNOSTIC.md`.

## Diagnostic à l'aveugle

En ne soumettant que la première ligne du message d'exception (« The instance
of entity type 'Order' cannot be tracked because another instance with the
same key value for {'Id'} is already being tracked. ») sans code ni contexte,
l'IA ne peut proposer qu'une explication générique du mécanisme de *change
tracking* d'Entity Framework Core : elle rappelle qu'un `DbContext` ne peut
suivre qu'une seule instance par clé primaire, et suggère des pistes
génériques (`AsNoTracking()`, détacher l'entité, vérifier qu'on ne charge pas
deux fois la même entité). Ces pistes ne sont pas fausses en soi, mais elles
ne permettent pas d'identifier *où* dans le code se produit le double
chargement, ni pourquoi le problème n'apparaît que sous charge.

## Diagnostic contextualisé

En fournissant la stack trace complète, l'extrait de `OrderService.cs` et
l'information « intermittent, uniquement sous charge », le diagnostic devient
précis : l'IA peut identifier que `_cache.GetOrCreateAsync` renvoie une
entité `Order` potentiellement mise en cache par un appel précédent, provenant
donc d'un `DbContext` d'une requête antérieure — déjà disposé ou en tout cas
distinct du `DbContext` scoped de la requête courante. Le caractère
« intermittent, sous charge » s'explique par une condition de course : le
bug ne se manifeste que lorsque deux requêtes concurrentes accèdent à la même
commande pendant la fenêtre de validité du cache (10 minutes de
`SlidingExpiration`).

## Correction proposée

**Cause racine** : l'entité mise en cache provient d'un `DbContext` scoped à
une requête précédente et n'est plus suivie par le `DbContext` courant : deux
requêtes concurrentes récupérant la même entité depuis le cache tentent de la
faire suivre par deux instances différentes de `DbContext`, d'où le conflit
de tracking (`InvalidOperationException`).

**Piste de correction retenue** : ne pas mettre en cache des entités trackées
par EF Core, mais des DTO immuables (voir `OrderSnapshot`), et recharger
l'entité depuis le `DbContext` courant avant toute modification. C'est
exactement ce qu'implémente `OrderService.cs` dans ce dossier `solution/` :
- `UpdateOrderStatusAsync` ne lit plus jamais l'entité depuis le cache : elle
  la recharge systématiquement via `_dbContext.Orders.FindAsync(orderId)`,
  qui appartient au `DbContext` scoped de la requête courante.
- Le cache ne conserve plus que des `OrderSnapshot` (record immuable, aucune
  référence à un `DbContext`), utilisés uniquement en lecture seule dans
  `GetOrderSnapshotAsync`.

**Piste alternative** (mentionnée mais non retenue ici) : utiliser
`_dbContext.Attach(order)` / `Entry(order).State = EntityState.Modified`
après un `TryGetValue` manuel sur le cache, en interceptant le cas où
l'entité serait déjà trackée (`ChangeTracker.Entries<Order>()`) — plus
défensif mais plus complexe et plus fragile dans le temps que de simplement
ne jamais mettre en cache d'entité trackée.
