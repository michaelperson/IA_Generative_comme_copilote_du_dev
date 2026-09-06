# Contexte projet — CatalogueTarifs

> Fichier de référence de l'exercice 10. Il est volontairement court : un
> fichier de contexte qu'on ne relit pas ne sert à rien. Une dizaine de règles
> vérifiables valent mieux que quarante lignes d'intentions.

## Ce qu'est ce projet

Module de consultation des tarifs d'un catalogue produit. Il est appelé par le
back-office commercial et par l'API de devis. Il ne fait que de la lecture et du
calcul : aucune persistance, aucun appel réseau.

## Stack et versions

Voir le fichier de projet du dossier (`package.json`, `pom.xml`, `.csproj` ou
`requirements.txt` selon le langage). Le framework de test est celui déjà
présent dans le dossier `tests` — ne pas en introduire un autre.

## Conventions à respecter

- Exprimer tous les montants en **centimes, en entiers**. Aucun flottant ne doit
  apparaître dans une valeur monétaire, à aucune étape du calcul.
- Lever une **erreur métier typée** et propre au domaine pour toute condition
  métier non satisfaite. Jamais une erreur générique du langage.
- Nommer en **français** les identifiants qui portent du vocabulaire métier
  (`tarif`, `remise`, `catalogue`), et en **anglais** les identifiants purement
  techniques (`index`, `parse`, `buffer`).
- Considérer les objets de valeur comme **immuables** : une opération qui
  transforme un tarif retourne une nouvelle instance, elle ne modifie pas
  l'existante.
- Documenter toute méthode publique dans le format standard du langage, en
  expliquant les conditions d'erreur.
- Valider les paramètres en entrée de méthode publique, avec un message qui
  nomme le paramètre fautif.

## Organisation des fichiers

- Un type public par fichier, le nom du fichier reprend celui du type.
- Le code de production dans `src/`, les tests dans `tests/`, en miroir.
- Un fichier de test par fichier de production, même nom suffixé selon la
  convention du langage.

## Ce que l'assistant ne doit jamais faire

- Ajouter une dépendance externe. Ce module ne dépend que de la bibliothèque
  standard et du framework de test déjà présent.
- Introduire un flottant dans un calcul monétaire, même temporairement.
- Modifier les tests existants pour les faire passer.
- Rendre un objet de valeur modifiable pour simplifier une implémentation.
- Renommer ou déplacer un fichier existant sans que la tâche le demande.

## Comment vérifier son travail

Exécuter la suite de tests du dossier. Tous les tests existants doivent passer
sans avoir été modifiés, et la fonctionnalité ajoutée doit être couverte par
ses propres tests, cas d'erreur compris.
