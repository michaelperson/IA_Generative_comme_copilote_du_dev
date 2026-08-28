# Exercices — L'IA générative comme copilote du développeur

Dépôt d'exercices pratiques accompagnant la formation bstorm *« L'IA générative comme copilote du développeur »* (3 jours). Il couvre les 9 exercices du programme, chacun décliné en **C#**, **Java** et **Python** (sauf l'exercice 9, voir sa remarque dédiée), avec un squelette de départ (`starter/`) et une correction de référence (`solution/`).

## Structure du dépôt

```
exercices/
  01-validation-tva/
    README.md              <- consigne complète de l'exercice
    csharp/{starter,solution}/
    java/{starter,solution}/
    python/{starter,solution}/
  02-userservice-crud/
  03-factory-pattern/
  04-tests-unitaires/
  05-revue-pr/
  06-debugging-stacktrace/
  07-refactoring-legacy/
  08-documentation-retrypolicy/
  09-script-shell/            <- bash + python uniquement (voir son README)
```

Chaque `starter/` est un point de départ volontairement incomplet (méthode à compléter, code fautif à corriger, ou suite de tests à écrire selon l'exercice) : à utiliser en le lisant, PAS en consultant `solution/` avant d'avoir essayé. Chaque `solution/` est une correction de référence, à ne consulter qu'après avoir tenté l'exercice.

Trois exercices (**01**, **04**, **07**) proposent en plus, uniquement en C#, deux variantes de style de solution générée par IA : `solution-style-copilot/` et `solution-style-claude/`. Elles illustrent des différences de style pédagogiques (concision vs documentation exhaustive, défensivité du code) — voir le `NOTE.md` de chaque dossier de variante : ce ne sont pas des sorties brutes réelles capturées depuis ces outils, mais des illustrations à des fins de formation.

## Prérequis par langage

### C#
- [.NET SDK 8.0+](https://dotnet.microsoft.com/download)
- Pour lancer les tests d'un exercice : `dotnet test` depuis le dossier `csharp/solution` (ou `starter`) concerné, ou depuis le sous-dossier `*.Tests` s'il existe séparément.

### Java
- JDK 17+ et [Maven](https://maven.apache.org/) 3.9+
- Pour compiler/tester : `mvn test` depuis le dossier `java/solution` (ou `starter`) concerné.

### Python
- Python 3.10+
- Chaque dossier `python/*` a son propre `requirements.txt`. Installer avec :
  ```bash
  pip install -r requirements.txt
  ```
- Pour lancer les tests : `pytest` depuis le dossier concerné.

### Bash (exercice 9 uniquement)
- Bash 4+, `flock`, `gzip` (présents par défaut sur toute distribution Linux courante).

## Index des exercices

| # | Nom | Module | Durée | Variantes IA |
|---|-----|--------|-------|--------------|
| 1 | Validation TVA belge (itération de prompt) | Module 3 | 45 min | Oui (C#) |
| 2 | UserService CRUD | Module 4 | 40 min | — |
| 3 | Factory Method (notifications) | Module 4 | 30 min | — |
| 4 | Suite de tests unitaires complète | Module 5 | 45 min | Oui (C#) |
| 5 | Revue de Pull Request assistée | Module 5 | 45 min | — |
| 6 | Debugging à partir d'une stack trace | Module 6 | 45 min | — |
| 7 | Refactoring legacy (SOLID/DRY) | Module 7 | 60 min | Oui (C#) |
| 8 | Documentation d'une classe (RetryPolicy) | Module 8 | 30 min | — |
| 9 | Script shell en langage naturel | Module 8 | 30 min | — (bash + python) |

Le détail de chaque exercice (objectif pédagogique, consigne complète, prompts fournis, critères d'évaluation ou piste de correction) est dans le `README.md` de son propre dossier.

## Note sur `SPEC.md`

Le fichier `SPEC.md` à la racine documente les conventions utilisées pour construire ce dépôt (structure des dossiers, conventions par langage, format des README). Il est utile pour qui souhaite ajouter un exercice ou une variante supplémentaire dans le même esprit, mais n'est pas nécessaire pour simplement faire les exercices.
