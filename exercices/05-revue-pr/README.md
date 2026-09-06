# Exercice 5 — Simuler une revue de Pull Request

**Module** : Module 5 — Accélérer la Qualité : Tests et QA

## Objectif pédagogique

Expérimenter l'IA comme premier filtre de revue de code avant la revue humaine, en lui fournissant un contexte de guidelines d'équipe explicite — condition indispensable à la pertinence de la revue.

## Consigne

À partir d'un diff de code (le vôtre, ou l'exemple fourni par le formateur), demandez à l'IA d'agir comme relecteur senior, en lui fournissant les coding guidelines de l'équipe.

**Particularité de cet exercice** : à la différence des autres exercices du dépôt, il ne s'agit pas d'écrire du code correct mais de produire une **revue structurée**. Pour simplifier la mise en place (pas de dépôt Git à cloner, pas d'outillage de diff), le « diff » à relire est fourni ici sous la forme d'un simple fichier source contenant le code proposé dans la PR (`starter/OrderService.{cs,java}` ou `starter/order_service.py`), plutôt que sous la forme d'un diff unifié littéral (`git diff`). Le contenu et les problèmes à identifier sont strictement équivalents à ce qu'un vrai diff de PR montrerait.

## Étapes

1. Lisez les coding guidelines de l'équipe (`starter/GUIDELINES.md`) — c'est le contexte indispensable à fournir à l'IA pour que sa revue soit pertinente et alignée sur les standards du projet.
2. Lisez le code proposé dans la PR (`starter/OrderService.cs` / `starter/OrderService.java` / `starter/order_service.py`).
3. Soumettez à l'IA le prompt de revue de Pull Request assistée ci-dessous, en y collant les guidelines et le code à relire.
4. Consignez les problèmes identifiés par l'IA dans `starter/REVIEW_TEMPLATE.md`, en respectant le format demandé : ligne concernée, guideline violée, sévérité, proposition de correction — trié par sévérité décroissante.
5. Comparez votre revue avec `solution/REVIEW.md` et la version corrigée `solution/CorrectedExample.*`.

## Contenu fourni dans le support

Guidelines d'équipe à fournir en contexte :
```text
Coding guidelines de l'équipe (extrait) :
- Toute méthode publique async doit accepter un CancellationToken
- Pas de "magic numbers" : utiliser des constantes nommées
- Les exceptions attrapées doivent être loguées avec le contexte (jamais de catch vide)
- Les noms de variables sont en anglais, les commentaires peuvent être en français
- Toute nouvelle dépendance externe doit être justifiée en commentaire
```

Prompt de revue de Pull Request assistée :
```text
Tu es un développeur senior qui effectue une revue de Pull Request.

Contexte : voici les coding guidelines de l'équipe : [coller les guidelines ci-dessus].

Tâche : relis le diff suivant comme le ferait un relecteur senior. Pour chaque problème identifié, indique : la ligne concernée, la guideline ou le principe violé, la sévérité (bloquant / suggestion / mineur), et une proposition de correction.

Format : liste structurée, un problème par ligne, triée par sévérité décroissante.

[coller le diff de code]
```

## Critères d'évaluation / débriefing

- La revue produite identifie bien les 5 violations présentes dans le code fourni (une par guideline) : absence de `CancellationToken` (ou équivalent selon le langage), magic numbers non nommés, catch vide sans log, variable nommée en français, dépendance externe non justifiée.
- Chaque remarque respecte le format demandé (ligne, guideline violée, sévérité, correction proposée) et l'ensemble est trié par sévérité décroissante.
- La sévérité attribuée est cohérente : les problèmes qui masquent des erreurs silencieusement ou empêchent une annulation propre sont bloquants ; les magic numbers et la dépendance non justifiée sont des suggestions ; le nom de variable en français est mineur.

**Débriefing** : Comparez les remarques de l'IA à celles qu'un pair réel aurait faites. Insistez sur le fait que cette revue assistée est un **premier filtre** qui accélère la revue humaine (en éliminant les remarques triviales avant même l'ouverture de la PR), et ne la remplace jamais sur les décisions d'architecture ou de compromis métier.

## Utilisation de ce dossier

- `csharp/starter`, `java/starter`, `python/starter`, `typescript/starter` : contiennent le code à relire (la « PR »), les guidelines d'équipe (`GUIDELINES.md`) et un gabarit vide (`REVIEW_TEMPLATE.md`) à compléter en suivant la consigne.
- `csharp/solution`, `java/solution`, `python/solution`, `typescript/solution` : contiennent la revue de référence complétée (`REVIEW.md`) ainsi qu'une version corrigée du code (`CorrectedExample.*`) appliquant toutes les corrections proposées — à ne consulter qu'après avoir tenté l'exercice.
