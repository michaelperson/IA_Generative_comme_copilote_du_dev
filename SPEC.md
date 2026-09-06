# Spécification — dépôt d'exercices "L'IA générative comme copilote du développeur"

Ce dépôt accompagne une formation bstorm. Il contient, pour chacun des 9 exercices pratiques du programme, un squelette de départ ("starter") et une correction de référence ("solution"), déclinés en C#, Java et Python (sauf mention contraire dans la consigne de l'exercice).

## Arborescence attendue pour CHAQUE exercice

```
exercices/NN-nom-court/
  README.md                  <- consigne complète de l'exercice (voir contenu fourni)
  csharp/
    starter/                 <- projet .NET compilable, TODO à compléter, échoue ou est incomplet tant que l'exercice n'est pas fait
    solution/                <- implémentation de référence complète et fonctionnelle
  java/
    starter/
    solution/
  python/
    starter/
    solution/
```

Si l'exercice a des variantes "style IA" demandées (voir consigne spécifique), ajoute en plus, uniquement dans le(s) langage(s) indiqué(s) :
```
    solution-style-copilot/
    solution-style-claude/
```

## Conventions par langage

### C#
- Utilise `dotnet new console` ou `dotnet new classlib` selon le cas pour le code de production, et `dotnet new xunit` pour les tests, avec un `.sln` optionnel (pas obligatoire, un simple dossier avec les .csproj suffit).
- Nom de projet = nom du dossier en PascalCase raisonnable (ex: `ValidationTva.csproj`).
- Tests avec xUnit quand l'exercice implique des tests.
- Le `starter/` doit compiler (`dotnet build` doit réussir) mais le comportement métier doit être absent ou incomplet, avec des commentaires `// TODO:` clairs indiquant quoi faire. Si des tests sont fournis dans le starter, ils doivent pouvoir s'exécuter (même en échouant en rouge) — jamais une erreur de compilation.
- La `solution/` doit être un code complet, propre, commenté (commentaires XML `<summary>` sur les méthodes publiques importantes), qui compile et dont les tests (s'il y en a) passent.

### Java
- Structure Maven simple : `pom.xml` + `src/main/java/...` + `src/test/java/...` (JUnit 5 pour les tests). Group id `be.bstorm.formation`, package par exercice (ex: `be.bstorm.formation.tva`).
- Même règle starter/solution que C# (le starter compile, TODO explicites ; la solution est complète et testée).

### Python
- Structure simple : un module `.py` (ou petit package) + `requirements.txt` si dépendance nécessaire (ex: `requests`, `pytest`). Tests avec `pytest`.
- Même règle starter/solution.

## Contenu du README.md de chaque exercice

Reprends fidèlement (en français, reformulation légère autorisée pour la clarté) : le titre de l'exercice, l'objectif pédagogique, la consigne, les étapes numérotées si présentes, le ou les prompts d'exemple fournis dans le support (à citer tels quels dans un bloc de code, avec leur légende), et les critères d'évaluation / piste de correction / débriefing s'ils existent. Structure suggérée :

```markdown
# Exercice N — Titre

**Module** : ...

## Objectif pédagogique
...

## Consigne
...

## Étapes
1. ...

## Prompts fournis dans le support
```text
(prompt cité tel quel)
```

## Critères d'évaluation / débriefing
...

## Utilisation de ce dossier
- `csharp/starter`, `java/starter`, `python/starter` : point de départ, à compléter en suivant la consigne.
- `csharp/solution`, `java/solution`, `python/solution` : correction de référence, à ne consulter qu'après avoir tenté l'exercice.
```

## Variantes "style IA" (uniquement pour les exercices concernés)

Quand la consigne demande des variantes de style, crée deux dossiers de solution supplémentaires en plus de `solution/` (qui reste la référence "neutre/pédagogique") :

- `solution-style-copilot/` : illustre un style typiquement observé avec un assistant de complétion inline dans l'IDE (GitHub Copilot et proches) — code concis, commentaires minimaux, va droit à l'implémentation la plus directe, moins de structuration défensive spontanée, nommage parfois plus générique.
- `solution-style-claude/` : illustre un style typiquement observé avec un assistant conversationnel orienté raisonnement (Claude et proches) — code plus structuré, documentation plus complète (XML docs/Javadoc/docstrings), gestion d'erreurs plus explicite et défensive, découpage en méthodes/responsabilités plus poussé, explique parfois ses choix en commentaire.

Chaque dossier de variante doit contenir un `NOTE.md` d'une à deux phrases précisant : *"Cette variante illustre un style pédagogique typique ; il ne s'agit pas d'une sortie brute réelle capturée depuis l'outil cité, mais d'une illustration des différences de style à des fins de formation."* Le code lui-même doit rester correct et fonctionnel dans les deux variantes — seul le style change, pas la justesse.

## Vérification avant de terminer

Pour chaque langage de chaque exercice que tu traites :
1. Le starter doit être syntaxiquement valide dans son langage (tu n'as pas besoin d'exécuter `dotnet build`/`mvn compile`/`pytest` réellement si l'environnement ne le permet pas, mais relis attentivement le code pour t'assurer qu'il n'a pas d'erreur de syntaxe évidente).
2. La solution doit être complète et correcte par rapport à la consigne (pas de `TODO` oublié, pas de faute de syntaxe).
3. N'invente pas de contenu pédagogique qui ne soit pas dans la consigne fournie — tu peux structurer et nommer les fichiers à ta discrétion, mais le fond doit rester fidèle.

Termine ta réponse par la liste des fichiers créés (chemins relatifs) pour cet exercice.

## Ajout d'un exercice en TypeScript

Chaque dossier `typescript/{starter,solution}` est un projet Node autonome :

- `package.json` (`type: module`, scripts `test` et `typecheck`), avec pour seules
  dépendances de développement `vitest` et `typescript` ;
- `tsconfig.json` en mode `strict` avec `noUncheckedIndexedAccess` ;
- le code de production dans `src/`, les tests dans `tests/`, suffixés `.test.ts` ;
- les imports internes portent l'extension `.js` (résolution ESM), même depuis
  des fichiers `.ts`.

Aucune dépendance externe au-delà de vitest et typescript : le `fetch` natif de
Node 20+ suffit pour l'exercice 2.

## Variantes de style IA

Les exercices 1, 4 et 7 proposent, dans les quatre langages, deux variantes
`solution-style-copilot/` et `solution-style-claude/`. Règle à respecter si l'on
en ajoute : le **comportement observable doit être strictement identique** à
celui de `solution/` — mêmes valeurs de retour, mêmes erreurs levées, mêmes
tests qui passent. Seuls changent la granularité du découpage, le niveau de
documentation et la richesse du typage. Chaque variante porte un `NOTE.md`
rappelant qu'il s'agit d'une illustration pédagogique et non d'une capture
réelle.
