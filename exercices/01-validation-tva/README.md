# Exercice 1 — Affiner itérativement un prompt (validation de numéro de TVA)

**Durée** : 45 minutes (30' individuel ou en binôme + 15' débriefing collectif)
**Module** : Module 3 — Le Prompt Engineering du développeur

## Objectif pédagogique

Expérimenter concrètement l'itération sur un prompt en appliquant les 5 piliers (Rôle, Contexte, Tâche, Format, Contraintes), et observer l'effet du pilier « Contraintes » et « Format de sortie » sur la qualité et la réutilisabilité du code généré.

## Consigne

Vous devez obtenir, par itérations successives d'un prompt, une fonction de validation d'un numéro de TVA intracommunautaire belge (format `BE` + 10 chiffres, avec vérification de la clé de contrôle modulo 97 sur les 8 premiers chiffres du numéro d'entreprise).

### Règle métier exacte à implémenter

C'est la vraie règle, à coder dans le starter/solution, pas seulement dans le prompt :

1. Nettoyer l'entrée (supprimer espaces, points, tirets, mettre en majuscule).
2. Vérifier le format "BE" + 10 chiffres.
3. Les 8 premiers chiffres forment le "numéro d'entreprise", les 2 derniers sont la "clé de contrôle".
4. La clé de contrôle attendue = 97 - (numéro d'entreprise modulo 97). Cas limite : si (numéro d'entreprise modulo 97) == 0, alors le numéro doit être considéré comme INVALIDE (cas limite mathématique explicitement à gérer, ne pas laisser la clé attendue devenir 97).
5. Retourner un résultat structuré, pas juste un booléen : un type "ResultatValidationTva" avec un booléen EstValide et un message d'erreur optionnel (null si valide).
6. Gérer explicitement le cas d'entrée null ou vide (retourne invalide avec message clair, ne lève pas d'exception non gérée).

## Étapes

Ces trois prompts illustrent une progression : un prompt naïf, un prompt enrichi (contexte + tâche), puis un prompt final appliquant les 5 piliers du prompt engineering.

### Étape 1 — Prompt naïf

Prompt naïf (à essayer en premier, pour constater les limites) :

```text
Écris une fonction qui valide un numéro de TVA.
```

### Étape 2 — Prompt enrichi (contexte + tâche)

```text
Tu es un développeur C#. J'ai besoin d'une fonction qui valide un numéro de TVA intracommunautaire belge. Le format est "BE" suivi de 10 chiffres. Les 8 premiers chiffres forment le numéro d'entreprise, les 2 derniers sont une clé de contrôle calculée par modulo 97 (clé = 97 - (numéro d'entreprise modulo 97)).
```

### Étape 3 — Prompt final respectant les 5 piliers

```text
Tu es un développeur C#/.NET senior.

Contexte : je dois valider des numéros de TVA intracommunautaires belges saisis par des utilisateurs dans un formulaire web, avec des espaces ou tirets possibles (ex: "BE 0123.456.749" ou "BE0123456749").

Tâche : écris une méthode statique ValiderNumeroTvaBelge(string input) qui :
1. Nettoie l'entrée (supprime espaces, points, tirets, met en majuscule)
2. Vérifie le format "BE" + 10 chiffres
3. Vérifie la clé de contrôle : les 2 derniers chiffres doivent être égaux à 97 - (les 8 premiers chiffres modulo 97)
4. Retourne un objet de résultat structuré, pas juste un booléen

Format de sortie : uniquement le code C#, avec :
- un record ResultatValidationTva(bool EstValide, string? MessageErreur)
- la méthode ValiderNumeroTvaBelge qui retourne ce record
- des commentaires XML de documentation

Contraintes :
- pas de dépendance externe (regex de base uniquement si besoin)
- gérer explicitement le cas d'une entrée null ou vide
- gérer explicitement le cas où le numéro d'entreprise finirait par 00 après troncature (cas limite du modulo 97, à traiter comme invalide)
- code testable unitairement (aucun effet de bord, aucune I/O)
```

## Consigne de travail pour les participants

1. Testez le prompt de l'étape 1 dans votre assistant IA habituel et notez au moins deux défauts du résultat obtenu.
2. Construisez votre propre version intermédiaire (étape 2) sans regarder l'exemple ci-dessus, puis comparez avec un binôme.
3. Faites évoluer votre prompt jusqu'à obtenir un code que vous accepteriez de committer tel quel dans votre projet.
4. Écrivez au moins 3 cas de test (un cas valide, un cas avec une clé de contrôle fausse, un cas de format incorrect) et demandez à l'IA de les générer dans le format de votre framework de test habituel (xUnit, NUnit, JUnit, pytest, Jest...).

## Critères d'évaluation / débriefing

- Le code compile-t-il sans modification ?
- La clé de contrôle est-elle correctement implémentée (et pas approximée par une regex) ?
- Les cas limites (entrée vide, format incorrect, modulo donnant 0) sont-ils gérés ?
- Le participant a-t-il relu et compris chaque ligne avant de l'accepter (rappel de l'anti-pattern du copier-coller aveugle) ?

## Utilisation de ce dossier

- `csharp/starter`, `java/starter`, `python/starter` : point de départ, à compléter en suivant la consigne.
- `csharp/solution`, `java/solution`, `python/solution` : correction de référence, à ne consulter qu'après avoir tenté l'exercice.
- `csharp/solution-style-copilot`, `csharp/solution-style-claude` : deux variantes de la solution C#, illustrant des différences de style typiquement observées selon l'outil IA utilisé (voir le `NOTE.md` de chacune). Ces variantes ne sont fournies qu'en C# pour cet exercice ; Java et Python n'ont que `starter/` et `solution/`.
