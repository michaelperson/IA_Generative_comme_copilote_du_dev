# Exercice 10 — Rédiger le fichier de contexte projet et déléguer une tâche

**Module** : Module 4 — L'IA comme Assistant de Code

## Objectif pédagogique

Faire l'expérience du levier le plus rentable du Module 4 : le fichier
d'instructions versionné à la racine du dépôt. L'exercice se joue en deux
temps — déléguer une tâche **sans** contexte, puis **avec** — et l'apprentissage
vient entièrement de l'écart entre les deux résultats.

C'est aussi le seul exercice du dépôt dont le livrable principal n'est pas du
code mais un **document** : le fichier de contexte que vous emporterez dans
votre propre dépôt.

## Le projet fourni

Un mini-projet `CatalogueTarifs` de trois fichiers, volontairement petit mais
porteur de conventions d'équipe **implicites** — celles qu'aucun commentaire
n'énonce et qu'un nouvel arrivant découvre en revue de code.

Votre premier travail est de les identifier en lisant le code.

## Étapes

### Étape 1 — Déléguer sans contexte (le témoin)

Ouvrez `TACHE.md` et soumettez la tâche telle quelle à votre assistant, sans
rien lui dire du projet au-delà des fichiers.

Notez le résultat obtenu, sans le corriger. C'est votre point de comparaison.

### Étape 2 — Identifier les conventions implicites

En lisant les fichiers du dossier `starter/`, listez tout ce qu'un nouvel
arrivant devrait savoir avant d'écrire sa première ligne. Cherchez notamment :

- comment les montants sont représentés, et pourquoi ;
- ce qui est levé en cas d'erreur métier ;
- la langue utilisée pour les noms de domaine et pour les noms techniques ;
- l'organisation des fichiers et des tests ;
- la politique en matière de dépendances externes ;
- le format de documentation attendu.

Visez entre six et dix règles. Au-delà, vous décrivez le code au lieu de
l'expliquer.

### Étape 3 — Rédiger le fichier de contexte

Complétez `CONTEXTE_PROJET_TEMPLATE.md`. Une règle par ligne, formulée à
l'impératif, vérifiable en relecture. Une règle qu'on ne peut pas vérifier dans
un diff n'a pas sa place dans ce fichier.

N'oubliez pas la section la plus utile et la plus souvent absente : **ce que
l'assistant ne doit jamais faire**.

### Étape 4 — Déléguer à nouveau, avec le contexte

Soumettez la même tâche, en fournissant cette fois votre fichier de contexte.
Selon votre outil, il se place à la racine du dépôt sous le nom attendu par
celui-ci, ou se colle simplement en tête de prompt — le principe est identique.

### Étape 5 — Comparer et relire

Comparez les deux diffs à l'aide de la grille de relecture. Comptez les
retouches nécessaires dans chaque cas : c'est le résultat chiffré de
l'exercice.

## Auto-évaluation

- Combien de retouches le résultat de l'étape 1 demandait-il ? Et celui de l'étape 4 ?
- Votre fichier de contexte contient-il au moins une interdiction explicite ?
- Chacune de vos règles est-elle vérifiable dans un diff, ou certaines sont-elles
  des intentions vagues (« écrire du code propre ») ?
- Sauriez-vous transposer ce fichier à votre projet réel en moins d'une heure ?

## Utilisation de ce dossier

- `starter/<langage>/` : le mini-projet avec ses conventions implicites,
  `TACHE.md` (la tâche à déléguer) et `CONTEXTE_PROJET_TEMPLATE.md` à compléter.
- `solution/<langage>/` : un fichier `CONTEXTE_PROJET.md` de référence, la
  `GRILLE_RELECTURE.md`, et l'implémentation attendue de la tâche avec ses tests.

Langages fournis : C#, Java, Python et TypeScript. Les conventions du projet
sont identiques dans les quatre ; seule leur expression idiomatique change.

## Note sur la numérotation

Cet exercice complète le Module 4, qui présente les agents en terminal et le
fichier de contexte projet. Il porte le numéro 10 parce qu'il a été ajouté
après les neuf premiers, dont la numérotation est utilisée dans le support et
dans les documents distribués aux participants.
