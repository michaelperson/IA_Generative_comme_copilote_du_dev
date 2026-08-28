# Exercice 6 — Identifier la cause racine à partir d'une stack trace

**Durée** : 45 minutes
**Module** : Module 6 — L'IA comme Assistant de Debugging

## Objectif pédagogique

Constater par la pratique l'effet de la richesse du contexte fourni sur la pertinence du diagnostic, en comparant un prompt pauvre et un prompt riche sur le même incident.

## Consigne

Un scénario de bug est fourni ci-dessous (ou utilisez un incident réel de votre backlog si vous en disposez).

```csharp
// OrderService.cs
public async Task UpdateOrderStatusAsync(Guid orderId, OrderStatus newStatus)
{
    var order = await _cache.GetOrCreateAsync(orderId, async entry =>
    {
        entry.SlidingExpiration = TimeSpan.FromMinutes(10);
        return await _dbContext.Orders.FindAsync(orderId);
    });

    order.Status = newStatus;
    _dbContext.Orders.Update(order); // ligne 47
    await _dbContext.SaveChangesAsync();
}
```
*(Extrait de code source de l'incident, à fournir en contexte)*

```
System.InvalidOperationException: The instance of entity type 'Order' cannot be tracked because another instance with the same key value for {'Id'} is already being tracked.
   at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.InternalEntityEntry...
   at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.StartTrackingFromQuery...
   at OrderService.UpdateOrderStatusAsync(Guid orderId, OrderStatus newStatus) in OrderService.cs:line 47
   at OrderController.PatchStatus(Guid id, OrderStatus status) in OrderController.cs:line 22
```
*(Exemple de stack trace à fournir — Entity Framework Core)*

Prompt fourni (légende « Prompt d'explication de stack trace ») :
```text
Tu es un développeur .NET senior expert en Entity Framework Core.

Contexte : cette exception survient de façon intermittente en production,
uniquement sous charge.

Tâche : explique en langage simple ce que signifie cette exception, quelle
est sa cause la plus probable dans un contexte ASP.NET Core avec un
DbContext scoped, et propose 2 pistes de correction.

[coller la stack trace ci-dessus]

Format : 1) explication en 3-4 phrases, 2) cause la plus probable,
3) pistes de correction classées par probabilité de résolution.
```

## Étapes

1. Diagnostic à l'aveugle : soumettez uniquement le message d'exception (première ligne de la stack trace) sans code, et observez la généricité de la réponse.
2. Diagnostic contextualisé : ajoutez la stack trace complète, l'extrait de code ci-dessus, et l'information « intermittent, uniquement sous charge ». Comparez la précision du diagnostic obtenu.
3. Demandez explicitement une correction proposée, en précisant la contrainte « le cache est partagé entre plusieurs requêtes concurrentes, le DbContext est scoped par requête ».

## Piste de correction attendue (pour le formateur)

L'entité mise en cache provient d'un DbContext scoped à une requête précédente et n'est plus suivie par le DbContext courant : deux requêtes concurrentes récupérant la même entité depuis le cache tentent de la faire suivre par deux instances différentes de DbContext, d'où le conflit de tracking. Piste : ne pas mettre en cache des entités trackées par EF Core, mais des DTO immuables, et recharger l'entité depuis le DbContext courant avant modification.

## Adaptation par langage

Le bug exact (cache applicatif partagé + entité ORM "trackée" par un contexte/session scopé par requête) est spécifique à Entity Framework Core en C#. Le même *pattern* de cause racine est adapté aux ORM idiomatiques des deux autres langages :

- **Java** : Spring Data JPA / Hibernate — le cache Caffeine (singleton applicatif) conserve une entité `Order` suivie par une session Hibernate liée à une requête précédente ; une deuxième requête tente de la faire suivre par une autre session, d'où `org.hibernate.NonUniqueObjectException`.
- **Python** : SQLAlchemy — un dictionnaire de cache au niveau module conserve un objet `Order` ORM encore attaché à une `Session` d'une requête précédente ; une deuxième requête tente de l'ajouter à sa propre `Session`, d'où `sqlalchemy.exc.InvalidRequestError: ... is already attached to session`.

Dans les trois langages, la correction retenue est identique dans son principe : ne jamais mettre en cache l'entité/objet suivi par l'ORM, mais un DTO/snapshot immuable, et toujours recharger l'entité depuis le contexte/session/EntityManager courant avant toute écriture.

## Utilisation de ce dossier

- `csharp/starter`, `java/starter`, `python/starter` : le code fautif (reproduisant le bug), le message d'erreur/stack trace correspondant (`STACKTRACE.txt` / `ERROR.txt`), et un `DIAGNOSTIC.md` vide à compléter en suivant les 3 étapes.
- `csharp/solution`, `java/solution`, `python/solution` : le code corrigé et un `DIAGNOSIS.md` rempli (corrigé de référence pour les 3 étapes), à ne consulter qu'après avoir tenté l'exercice.
