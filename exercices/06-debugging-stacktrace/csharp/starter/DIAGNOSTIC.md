# Diagnostic — incident OrderService (C# / EF Core)

À compléter en suivant les 3 étapes de la consigne (voir README.md de l'exercice).

## Diagnostic à l'aveugle

*(Réponse obtenue en ne soumettant que la première ligne du message d'exception, sans code ni contexte.)*

## Diagnostic contextualisé

*(Réponse obtenue en soumettant la stack trace complète + `OrderService.cs` + l'information « intermittent, uniquement sous charge ».)*

## Correction proposée

*(Réponse obtenue en demandant explicitement une correction, en précisant que le cache est partagé entre requêtes concurrentes et que le `DbContext` est scoped par requête.)*
