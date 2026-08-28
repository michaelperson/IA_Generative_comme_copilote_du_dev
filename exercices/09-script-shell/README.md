# Exercice 9 — Générer un script shell en langage naturel

**Durée** : 30 minutes
**Module** : Module 8 — L'IA pour la « Developer Experience » (DX)

## Adaptation de langages pour cet exercice

Cet exercice est, dans le programme original de la formation, un exercice de
**script bash** (automatisation système), pas un exercice de code
métier C#/Java orienté objet. Appliquer strictement la structure
`csharp/` + `java/` + `python/` du reste du dépôt (voir `SPEC.md`) aurait
demandé de traduire cette tâche en applications console C# et Java — un
exercice artificiel, qui dénaturerait l'objectif pédagogique réel : rédiger
une demande en langage naturel pour un **script d'automatisation shell**,
puis relire ce script avec un regard de sécurité/robustesse avant toute
exécution.

En conséquence, pour ce dossier uniquement :

- **`bash/`** (starter + solution) est l'implémentation **fidèle au
  programme** : c'est elle qui correspond exactement à la consigne et aux
  prompts fournis dans le support.
- **`python/`** (starter + solution) est fourni **en bonus**, en équivalent
  fonctionnel utilisant uniquement la bibliothèque standard Python
  (`pathlib`, `gzip`, `argparse`, `logging`, `fcntl`, `os`) : l'automatisation
  de scripts système est également un usage très courant de Python, l'un
  des trois langages cibles du dépôt, et voir la même problématique
  résolue dans un second langage a une valeur pédagogique propre.
- **Il n'y a volontairement pas de dossiers `csharp/` ni `java/`** pour cet
  exercice : écrire un script de compression de fichiers de logs sous forme
  d'application console C# ou Java n'est pas ce que demande le programme, et
  aurait été un exercice de traduction artificiel plutôt qu'une illustration
  fidèle de la consigne.

C'est un choix délibéré et documenté, pas un oubli.

## Objectif pédagogique

Expérimenter la rédaction d'une demande en langage naturel pour un script
d'automatisation, puis appliquer systématiquement la relecture de sécurité
vue plus haut avant toute exécution, même en environnement de test.

## Consigne

Rédigez, en langage naturel, la demande suivante à votre assistant IA :

> « Écris un script bash qui trouve tous les fichiers .log de plus de 10 Mo
> dans le répertoire /var/log et les compresse. »

## Étapes

1. Obtenez un premier script, puis identifiez collectivement les questions
   de sécurité et de robustesse qu'il ne règle pas spontanément : que
   se passe-t-il si le fichier est activement écrit par un processus au
   moment de la compression ? Le script s'exécute-t-il en boucle infinie sur
   les fichiers `.gz` déjà produits lors d'une exécution précédente ? Les
   permissions sont-elles vérifiées avant écriture ?
2. Renvoyez un prompt de relance intégrant ces contraintes explicitement.

## Prompts fournis dans le support

Prompt initial (langage naturel, étape 1) :
```text
Écris un script bash qui trouve tous les fichiers .log de plus de 10 Mo dans le répertoire /var/log et les compresse.
```

Prompt de relance avec contraintes de robustesse et de sécurité :
```text
Écris un script bash qui trouve tous les fichiers .log de plus de 10 Mo dans /var/log et les compresse.

Contraintes :
- exclut les fichiers déjà compressés (.gz)
- utilise un verrou (flock) pour éviter une exécution concurrente
- journalise chaque fichier traité avec horodatage dans un fichier de log dédié du script lui-même
- s'exécute en mode "dry-run" par défaut (affiche ce qu'il ferait sans rien modifier), et ne compresse réellement qu'avec l'option --apply
- gère explicitement le cas où /var/log n'est pas accessible en écriture
```

## Règle non négociable

> Même généré avec des contraintes précises, ce script doit être testé en
> mode dry-run puis sur un répertoire de test isolé avant toute exécution
> sur un système réel, conformément à la règle de relecture systématique
> des scripts touchant à l'infrastructure.

## Utilisation de ce dossier

- `bash/starter/compress-logs.sh` : le script « naïf » correspondant au
  premier prompt (étape 1), annoté de commentaires pointant chacune des
  failles de robustesse à identifier.
- `bash/solution/compress-logs.sh` : le script complet répondant à toutes
  les contraintes du prompt de relance (exclusion des `.gz`, verrou `flock`,
  journalisation horodatée, dry-run par défaut / `--apply`, vérification des
  permissions d'écriture).
- `bash/solution/TEST.md` : marche à suivre pour tester ce script en toute
  sécurité sur un répertoire isolé avant tout usage réel.
- `python/starter/compress_logs.py`, `python/solution/compress_logs.py` :
  équivalents Python (bonus) du starter et de la solution bash, avec la
  même logique naïve → robuste.

Le répertoire cible par défaut de tous ces scripts est `./test-logs` (et non
`/var/log`) afin qu'ils puissent être testés sans danger dans ce dépôt
d'exercices ; passez `/var/log` (ou tout autre chemin) explicitement en
argument pour un usage réel, en respectant la règle non négociable ci-dessus.
