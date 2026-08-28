# Exercice 3 — Expliquer un design pattern puis l'implémenter (Factory)

**Durée** : 30 minutes
**Module** : Module 4 — L'IA comme Assistant de Code

## Objectif pédagogique

Utiliser l'IA comme outil pédagogique (explication conceptuelle) avant de l'utiliser comme outil de production (implémentation), et vérifier la cohérence entre les deux réponses.

## Consigne

1. Demandez d'abord une explication conceptuelle du pattern Factory : quand l'utiliser, quel problème il résout, quelle est la différence entre Simple Factory, Factory Method et Abstract Factory.
2. Sur la base de la réponse obtenue, demandez une implémentation dans votre langage cible, appliquée à un cas concret : une fabrique de notifications (Email, SMS, Push) sélectionnée dynamiquement selon la préférence utilisateur.
3. Vérifiez que l'implémentation correspond réellement à la variante du pattern décrite à l'étape 1, et pas à une simplification qui en dénature l'intérêt (piège fréquent : l'IA propose un simple `switch` déguisé en « Factory »).

## Étapes

1. Demandez d'abord une explication conceptuelle du pattern Factory : quand l'utiliser, quel problème il résout, quelle est la différence entre Simple Factory, Factory Method et Abstract Factory.
2. Sur la base de la réponse obtenue, demandez une implémentation dans votre langage cible, appliquée à un cas concret : une fabrique de notifications (Email, SMS, Push) sélectionnée dynamiquement selon la préférence utilisateur.
3. Vérifiez que l'implémentation correspond réellement à la variante du pattern décrite à l'étape 1, et pas à une simplification qui en dénature l'intérêt (piège fréquent : l'IA propose un simple `switch` déguisé en « Factory »).

## Prompts fournis dans le support

Prompt étape 1 — explication conceptuelle :
```text
Tu es un développeur senior expert en design patterns.

Tâche (étape 1) : explique le pattern Factory Method en 10 lignes maximum : problème résolu, structure des classes, différence avec l'Abstract Factory. Ne donne pas encore de code.
```

Prompt étape 2 — implémentation fidèle au pattern :
```text
Sur la base du pattern Factory Method que tu viens d'expliquer, implémente-le en C# pour le cas suivant : une fabrique de notifications (EmailNotification, SmsNotification, PushNotification) qui implémentent une interface INotification avec une méthode SendAsync(string message). Le type de notification à créer est déterminé par une valeur d'enum NotificationChannel.

Contrainte : la structure doit être une véritable application du Factory Method (une méthode de fabrication abstraite surchargée par sous-classe), pas un simple switch dans une méthode statique.
```

## Critères d'évaluation / débriefing

- L'explication obtenue à l'étape 1 distingue correctement Simple Factory, Factory Method et Abstract Factory.
- L'implémentation de l'étape 2 est une véritable application du Factory Method : une classe créatrice abstraite avec une méthode de fabrication abstraite, surchargée par une sous-classe créatrice concrète par type de notification — pas un `switch`/`if` unique qui instancie directement les classes concrètes.
- La sélection du créateur concret selon `NotificationChannel` (ex. via une petite table de correspondance enum → créateur) reste externe à la logique du pattern lui-même : c'est un point d'entrée pratique, pas une resimplification du Factory Method en Simple Factory déguisé.

**Point de vigilance** : Vérifiez que votre implémentation n'est pas un switch déguisé — c'est l'erreur la plus fréquente sur cet exercice.

## Utilisation de ce dossier

- `csharp/starter`, `java/starter`, `python/starter` : point de départ, à compléter en suivant la consigne.
- `csharp/solution`, `java/solution`, `python/solution` : correction de référence, à ne consulter qu'après avoir tenté l'exercice.
