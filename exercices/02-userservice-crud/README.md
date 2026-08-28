# Exercice 2 — Générer une classe client API (UserService)

**Durée** : 40 minutes
**Module** : Module 4 — L'IA comme Assistant de Code

## Objectif pédagogique

Pratiquer la spécification précise d'une tâche de génération de code structurée (CRUD) en insistant sur le pilier « Contraintes », en particulier la gestion d'erreurs, souvent oubliée dans les prompts spontanés.

## Consigne

Demandez à votre assistant de générer une classe `UserService` qui consomme une API REST `/users`, avec les méthodes CRUD complètes (`GetByIdAsync`, `GetAllAsync`, `CreateAsync`, `UpdateAsync`, `DeleteAsync`), en spécifiant explicitement :

- le client HTTP à utiliser (`HttpClient` injecté via `IHttpClientFactory` en C#, ou l'équivalent dans votre langage) ;
- la gestion des codes de statut HTTP non-2xx (exception typée personnalisée, pas une exception générique) ;
- la sérialisation JSON attendue (camelCase côté API, PascalCase côté modèle C#) ;
- le comportement attendu en cas de timeout réseau (retry ? propagation ? valeur par défaut ?).

## Étapes

1. Lisez la consigne et, si besoin, adaptez le prompt fourni ci-dessous au langage cible (C#, Java ou Python).
2. Soumettez le prompt à votre assistant IA et récupérez le code généré.
3. Comparez le résultat à la liste de contraintes énumérées dans la consigne : client HTTP injecté, exception typée pour les codes non-2xx, sérialisation JSON camelCase/PascalCase, comportement de timeout.
4. Complétez le squelette `starter/` de votre langage avec le code obtenu (ou adaptez-le) et faites passer les tests fournis.
5. En débriefing, comparez votre résultat avec la `solution/` de référence et discutez du point de vigilance ci-dessous.

## Prompts fournis dans le support

Prompt de génération du UserService :

```text
Tu es un développeur C#/.NET senior.

Contexte : API REST externe exposant /users (GET liste, GET par id, POST, PUT, DELETE). Réponses JSON en camelCase. Le projet utilise IHttpClientFactory et System.Text.Json.

Tâche : génère une classe UserService implémentant IUserService avec les méthodes CRUD : GetByIdAsync, GetAllAsync, CreateAsync, UpdateAsync, DeleteAsync.

Format : interface IUserService puis classe UserService, code C# complet, avec commentaires XML.

Contraintes :
- lève une exception UserServiceException (à définir) pour tout code HTTP non-2xx, avec le code de statut et le corps de la réponse en propriétés
- timeout de 10 secondes, propage une TimeoutException explicite
- toutes les méthodes sont asynchrones et acceptent un CancellationToken
- aucune logique métier dans le service : uniquement l'accès aux données
```

## Critères d'évaluation / débriefing

Point de vigilance à soulever en débriefing : demandez au groupe si l'IA a bien respecté *toutes* les contraintes énumérées, ou si certaines ont été silencieusement ignorées (cas fréquent lorsque le prompt contient plus de 5-6 contraintes simultanées). C'est l'occasion d'introduire la pratique de l'itération : renvoyer un message de suivi ciblé uniquement sur la contrainte manquante plutôt que de tout regénérer.

## Utilisation de ce dossier

- `csharp/starter`, `java/starter`, `python/starter` : point de départ, à compléter en suivant la consigne.
- `csharp/solution`, `java/solution`, `python/solution` : correction de référence, à ne consulter qu'après avoir tenté l'exercice.

Note d'adaptation inter-langages : le prompt du support cible explicitement C#/.NET (`IHttpClientFactory`, `System.Text.Json`, `CancellationToken`). Les versions Java et Python de cet exercice transposent les mêmes quatre contraintes (client HTTP injecté, exception typée sur non-2xx, sérialisation JSON, comportement de timeout explicite) avec les mécanismes idiomatiques de chaque langage :
- **Java** n'a pas d'équivalent direct de `CancellationToken` : les méthodes du service sont asynchrones (`CompletableFuture`) et le timeout est porté par un délai explicite de 10 secondes sur chaque requête HTTP (voir le commentaire dans `UserServiceImpl.java`).
- **Python** exploite l'annulation coopérative native d'`asyncio` (annulation d'une tâche) comme équivalent du `CancellationToken`, combinée à un timeout explicite de 10 secondes sur le client HTTP.
